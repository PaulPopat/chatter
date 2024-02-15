using System.Text.Json;
using Effuse.Core.AWS.Integration;
using Effuse.Core.Handlers.Contracts;
using Effuse.Core.Integration.Contracts;
using Effuse.Core.Stubs;
using Effuse.SSO.Handlers.Controllers;
using Effuse.SSO.Handlers.Models.Register;
using Effuse.SSO.Integration.Clients.Jwt;
using Effuse.SSO.Integration.Clients.User;
using ParameterStore = Effuse.Core.Stubs.ParameterStore;
using EffuseParameterStore = Effuse.Core.AWS.Integration.ParameterStore;
using Effuse.SSO.Handlers.Models.Invite;
using Moq;
using Amazon.DynamoDBv2.Model;
using Effuse.SSO.Tests.Utilities;

namespace Effuse.SSO.Tests;

[TestClass]
public class RegisterTests
{
  private readonly Lazy<DynamoDB> lazyDb = new(() => new DynamoDB());

  private DynamoDB AwsDatabase => this.lazyDb.Value;

  private ParameterStore AwsParameterStore => new ParameterStore(new Dictionary<string, string>
  {
    ["JWT_CERTIFICATE"] = @"
-----BEGIN CERTIFICATE-----
MIIEtjCCAp4CCQCtQeyFdWWqmTANBgkqhkiG9w0BAQ0FADAdMQswCQYDVQQGEwJV
SzEOMAwGA1UEAwwFbG9jYWwwHhcNMjQwMjE1MDk1MTMwWhcNMjUwMjE0MDk1MTMw
WjAdMQswCQYDVQQGEwJVSzEOMAwGA1UEAwwFbG9jYWwwggIiMA0GCSqGSIb3DQEB
AQUAA4ICDwAwggIKAoICAQDW5HANsF94IxC6YaxE61UagO1nv+eebkToGzEttHPq
vd52677Tgajr0XFeZJbyPWnk8HLkfh3gU4Rg3VCVtvsipVWaLRA2xb3a1GDVgmTT
ucJfIrTWerFs1lvEFD4E9Qck4QhoAkBR59hBamgzm7tEZxCy9B1q0jGvjZtWnB4w
RelMKM1gDsUGYHgdVXLeZw8q1xOyDZSUq/HYTdJ6T7N9m8ulZKrdwhV6qJnJYpPl
l6E7tVDa0dotsDk2/qcOojMCTDD8hhUSUW9xWfJI7mkBjzqzV30R+GTGBV2LRyU7
q0hsAiKP3eRHGY6g7aSdiWlvmGkxdw4Y4Kkmiesikdzdt5FIXQH+nUoB7pNOpuB5
93bg9U+ZtQEZXcLgfKg8VE6X2A4NHKuhA2Y/Ff2tWR7gb9tWtj8wrHAzoQ11uOYT
UmvJ1XNQjtiB9nXr/7yBcv2PbfHlCQpEY90LVAXxqmB92Ew/gFYITNiOWk/3vo4I
ZbTNDgzeMo39H5W/pdV64eKfabAsWjG0A8NW0IpTE909/1ZX2DlaA4rhmiE0K8aT
7vgYHTQy+rVtoAHHhFG1r77W4+/fQCe+4dG5xaKTszlW0zO8czod7JKv0eJBQwV+
rsaRbdNIfUjZEMHUN1f6KkGM8yXsVuJP/YKKqD+AcQjlW61wDoEo5MaPPeaLng/S
lwIDAQABMA0GCSqGSIb3DQEBDQUAA4ICAQBlt1nkBkRztct0SXoTgOlzOeTwGIuz
NyDCj2POQ+SWzSGNoCbxlg+u/BAPwOfl71O7epoc8bv9ZEc9H/YJCfUQ130ArztK
xcDFodHJm5FIdafGJRL2JpqYiVaNBnxRblqNSleFzsByjfyS++NKgE0YZ1G27xvq
9ULWQDU/k5N4i/PT7ftktFyPzlPKnhEsG5hYoP53xRvd8/wqBlz1PDG4HalspwSQ
w6T23LVDCoLuvHFq0pNGyp3T8KIe/QrE4ee4evdMHQ15jNapSTlfLG5PmTlyewrT
74WGPKE9s3JTahX4nucSPmdmKd9Bs3h+ul5CtLk6+nw+zkAuPsr8gRbnlFzetSoV
V2sESoc9mtpf073O7hfpozUjPi7MGaMk2UDJCWQ6DI5KC2wpksvsMofEx9V4gYP2
SnuwXg5h9MgvldLf+AcjQM+SLYGRkM4YS7/JQUF8d9cRIyHGLqzoffQxZX6Ym8cC
K0Jea1oU33Mp0nol0m5aFux05q7ltkncaZO0Ad1zkp6BgwP/gv4sEub4+URUW1eY
e8MwJdYAvVlKTuJGdVVNdZgrUE+i35gtFoYWxnv5tZ8ucJnBBttnn8SGqhcmniqv
s53dwJt1NIeUtY9LrUiDyP9Yi1hlCPVPGrqN7fpfJhMOnmWnwqwFSDPF3FjH64dc
MzcKqTjwtvScjw==
-----END CERTIFICATE-----
".Trim(),
    ["JWT_SECRET"] = @"
-----BEGIN PRIVATE KEY-----
MIIJQgIBADANBgkqhkiG9w0BAQEFAASCCSwwggkoAgEAAoICAQDW5HANsF94IxC6
YaxE61UagO1nv+eebkToGzEttHPqvd52677Tgajr0XFeZJbyPWnk8HLkfh3gU4Rg
3VCVtvsipVWaLRA2xb3a1GDVgmTTucJfIrTWerFs1lvEFD4E9Qck4QhoAkBR59hB
amgzm7tEZxCy9B1q0jGvjZtWnB4wRelMKM1gDsUGYHgdVXLeZw8q1xOyDZSUq/HY
TdJ6T7N9m8ulZKrdwhV6qJnJYpPll6E7tVDa0dotsDk2/qcOojMCTDD8hhUSUW9x
WfJI7mkBjzqzV30R+GTGBV2LRyU7q0hsAiKP3eRHGY6g7aSdiWlvmGkxdw4Y4Kkm
iesikdzdt5FIXQH+nUoB7pNOpuB593bg9U+ZtQEZXcLgfKg8VE6X2A4NHKuhA2Y/
Ff2tWR7gb9tWtj8wrHAzoQ11uOYTUmvJ1XNQjtiB9nXr/7yBcv2PbfHlCQpEY90L
VAXxqmB92Ew/gFYITNiOWk/3vo4IZbTNDgzeMo39H5W/pdV64eKfabAsWjG0A8NW
0IpTE909/1ZX2DlaA4rhmiE0K8aT7vgYHTQy+rVtoAHHhFG1r77W4+/fQCe+4dG5
xaKTszlW0zO8czod7JKv0eJBQwV+rsaRbdNIfUjZEMHUN1f6KkGM8yXsVuJP/YKK
qD+AcQjlW61wDoEo5MaPPeaLng/SlwIDAQABAoICAGQVUqbdfSlb03+q2+vhWuqU
H33RoBNmsgsUFwx9Xft3YLQsSd0CJ2VlT5Kx8KdzuO0am7gVkN6Ypy/iA5Um+sIB
FORQlpub7zeK8GqgZ0tA2ekrJeQ6koXpNCYXc2clo6UmdZ7TZZeADnUxFMTshARw
qBNgpI7KxL9JiY6F+X1sIiMJYdWaK2Mz7N1knbI9d4/xScgEkS3JA0NFw4CZvDQF
KoymXPoZWZ2eBroHgnBM9Zf0c2rmj+H1kEAvzCiBnJY2XB5QaGH2sZuuU6xp9Vf3
mFRMrACBmpWrjAzBHvMya/F58RTmOFpw6b/DzoZcqFGEP4H9PACUKdO8KloYay+x
SZNLS4cVc+JnSRSdmlrA6dzvoCpCIvRw6seTurrml1fhOLe22kZQZ3Fw13XVR6U8
DYyrOaSyIJyFmlAW2gG4AOHtigotvQi+1nTxiiaz+fHG7Xtq+sU4YM0sQ+kDxADC
5VSgVSSPPRXe0TgYGGjKOpPubEPamwdwWHaM9kHk/d3f6AhCYP1prS2mqmsLAJmt
KoqWGo3L6QbYf5jlbeb6VC/XJaN7guf3Kuu6IsjrFZMBJ5V4f6fCtNZBLHMFs1O6
yH4I0lsBetpGuQD+rrKaDwnA15hCRA5hmc362NyjSyWUa3oRfHN1DIFv77Ia5yjd
z6/fy7x48fXBu3f6jDPxAoIBAQD6kqeilXwepkSsgwzyPJS6LaCuL3ORHVKszud2
N5Z04d0+EVJpoRL9f+3LY1dccdoc0hruavFuj4YIQMZMjgRMEE20QJ5oeMlnfxH/
XG/qPG0+dR/HN5J0NQWM30VjNA9+LSWDpRYXopz2/6ArU75XCKtg6VT7lJJ48j78
76NCwPzzBtaNaAfAoSEsgNAFOjFFuVB8GQ0YsPgRcOGbzq2XGpypqX5KObl58BIU
anDtLODwYqSC6O15xgNtuxS/CoLZq0rRnr2loGLbeISt5/nI7oUszRT+tFPB8N8z
jGLreqpu3dhOiC7XXGelZR0jJ5vTaV3s2+8p3e22aXhQGLbvAoIBAQDbi/ImxSiN
KHEEijkT86D5cioqUKLJeY6MhqJXC8mdrzfRApWDtnsc38BxiWWJ9hSgfi+SIdQb
SV56uYgaYDkT5+k1gOZWHrd6Q158AkxpI1CxcY/8lfZXGlWY+rJrjGAR0uImAhE2
OoEtcVAGKRi+fQuAMvDY2L1yBvDtGFeh0ThVt7e5eFyNynGpCQlMQG2j4WtcyZ7U
mgeUoNhPv/iXdpq7hBu3KhwdY7O764DuKYpWqmt1lbTUF4v9A/w9RhLRiG/gMDcf
eDHTTV2K069Y2eLfeqfKmS21EgciSKWlBIXXu5+cgZMXmyQwpUfDqHmOoq4ej+2p
YEJJqCdxH17ZAoIBAQDc5k1C1+YEXzBLpXzUUPQM7gypgsuvMtqf3gfQAFFz2Wri
PQMafBXxxcYtd/acWQKgRdnYNg+CGKVgwToQY2MGa5kVP6JnF5T22N9U8Hj5Vyw3
06mgRy2lpNivyGWzf0HpSoO3+uHvFDysw87AY6N5tvrfBNUWtXQri+RbuCeFwf09
gtnC7+NlvqcwRJ8e/J5TiXua67rP+bO4LHu4kAwZGizipbngVeNdzHcj6HKFpPro
8Q13G0HACQEcy8EOaoXQv2HGStuGGE8OjMDrT4a1zRE63dGUegUdBOzA1RKf5hQ6
iPkYwpYI5j25Ydq+Ez0cs9dJ6Np/XlciaPjYI/GhAoIBABz7BTT+85JmkzyhLlWm
5EnpOw2o5UQpKpr8LrPE15FcDsclSy0+ylOOaa3TEDc8544j+g2VL5WGgtU6Zm/s
4bvx8gPhBwa5OUkHWZKPDx0Vz1INBo+2D/WBgWkXNrQhrJDNwJJ51WHOKT0hZwnZ
JO4IPZtnnglR2vgRWH5Dp1Wx83jFLphp/fWtkoFYswSAwLhQSkbOSowP5Q5GiSdF
1P/RZS15i9sK0PlELvaQaM2HaD52cobsAxm4Hf+BJivEczl7hCEY3D3oNNOKs91e
ghf9cwhC/aEtxS3QumZZJpx4014d4zzakdsc3JMbTjTSqXEdolRgdWPVVAq7CrHR
DUkCggEARso2upa2ToMa+XJNlQ0GelJcc4GHD4gUZkpxPjlz9wrRDh/Ko/fndJfJ
Ain2bu5a8PpxDM0ux+7GQqnbpTllb0xrXJpVnnkuSYtsVzy3JWervw4T2E9V+nKU
9ZpHrbOSAm2AsEWeZacJhwRnkOKKXAbwQzi62XBd7auPq7woxmFusMpZR625BLGu
xwwVZ6lEo1q3kcEenfnQzVpEsjXRrN0a7VltjUpkph3qZbccM6NejhvqVlQXC0uC
odBGNTSAycOKCYTlrZ0I8UIyY8vhTFu/NyW84fP0DQJFF2+FLzVlP9B7aUK112NI
FmgfjO/k9rJr5GA+goiVW05g8f8dVw==
-----END PRIVATE KEY-----
".Trim()
  });

  private Register AwsSut => new(
    new(
      new DbUserClient(
        new DynamoDBDatabase(this.AwsDatabase),
        (IStatic)null),
      new ParameterJwtClient(
        new EffuseParameterStore(
          this.AwsParameterStore))));

  private Invite AwsInvite => new Invite(
    new(
      new DbUserClient(
        new DynamoDBDatabase(this.AwsDatabase),
        (IStatic)null),
      new ParameterJwtClient(
        new EffuseParameterStore(
          this.AwsParameterStore))));

  [TestInitialize]
  public void Setup()
  {
    TestSetup.Env();
  }

  [TestMethod]
  public async Task UserCanRegister()
  {
    var inviter = this.AwsInvite;
    var inviterResponse = await inviter.Handle(new HandlerProps(
        path: "",
        method: "",
        connectionId: "",
        pathParameters: new Dictionary<string, string>(),
        queryParameter: new Dictionary<string, string>
        {
          ["email"] = "testemail"
        },
        headers: new Dictionary<string, string>(),
        body: string.Empty));
    if (inviterResponse.Body is not InviteResponse r) throw new Exception("Bad invite response");
    var inviteCode = r.Code;

    var sut = this.AwsSut;

    var response = await sut.Handle(new HandlerProps(
        path: "",
        method: "",
        connectionId: "",
        pathParameters: new Dictionary<string, string>(),
        queryParameter: new Dictionary<string, string>(),
        headers: new Dictionary<string, string>(),
        body: JsonSerializer.Serialize(new RegisterForm
        {
          UserName = "testuser",
          Email = "testemail",
          Password = "testpassword",
          InviteToken = inviteCode
        })));

    if (response.Body is not RegisterResponse g) throw new Exception("Bad response from register");

    Assert.AreEqual(response.StatusCode, 201);

    this.AwsDatabase.AssertItems(
      new PutItem
      {
        TableName = "usertable",
        Item = new Dictionary<string, AttributeValue>
        {
          ["UserId"] = new AttributeValue()
          {
            S = g.UserId
          },
          ["UserName"] = new AttributeValue()
          {
            S = "testuser"
          },
          ["Email"] = new AttributeValue()
          {
            S = "testemail"
          },
          ["EncryptedPassword"] = new AttributeValue()
          {
            S = IsString.PasswordMatching("testpassword"),
          },
          ["RegisteredAt"] = new AttributeValue()
          {
            S = IsString.DateNear(DateTime.Now, 2000)
          },
          ["LastSignIn"] = new AttributeValue()
          {
            S = IsString.DateNear(DateTime.Now, 2000)
          },
          ["Servers"] = new AttributeValue()
          {
            L = new List<AttributeValue>()
          },
          ["Biography"] = new AttributeValue()
          {
            S = string.Empty
          }
        }
      }
    );
  }
}