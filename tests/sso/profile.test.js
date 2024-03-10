const auth = require("../clients/auth");
const sso = require("../clients/sso");
const url = require("../clients/url");
const createUser = require("../presets/create-user");
const addUserToServer = require("../presets/add-user-to-server");
const { PROFILE_MIME, PROFILE_PICTURE } = require("../presets/statics");

describe("profile", () => {
  it("Updates the public profile", async () => {
    const user = await createUser();

    await sso.put(
      url("/api/v1/user/profile"),
      {
        UserName: "test user",
        Biography: "I love my wife!!!",
        Picture: {
          MimeType: PROFILE_MIME,
          Base64Data: PROFILE_PICTURE,
        },
      },
      auth(user.admin_token)
    );

    const profile = await sso.get(
      url("/api/v1/users/:user_id/profile", {
        user_id: user.user_id,
      })
    );

    expect(profile.data).toEqual({
      UserId: user.user_id,
      UserName: "test user",
      Biography: "I love my wife!!!",
    });
  });

  it("Updates the private profile", async () => {
    const user = await createUser();

    await sso.put(
      url("/api/v1/user/profile"),
      {
        UserName: "test user",
        Biography: "I love my wife!!!",
        Picture: {
          MimeType: PROFILE_MIME,
          Base64Data: PROFILE_PICTURE,
        },
      },
      auth(user.admin_token)
    );

    const profile = await sso.get(
      url("/api/v1/user/profile"),
      auth(user.admin_token)
    );

    expect(profile.data).toEqual({
      UserId: user.user_id,
      UserName: "test user",
      Biography: "I love my wife!!!",
      Servers: [],
      LastSignIn: expect.any(String),
      RegisteredAt: expect.any(String),
    });
  });

  it("Joins a server", async () => {
    const user = await addUserToServer();

    const profile = await sso.get(
      url("/api/v1/user/profile"),
      auth(user.admin_token)
    );

    expect(profile.data).toEqual({
      UserId: user.user_id,
      UserName: user.username,
      Biography: "",
      Servers: [
        {
          Url: process.env.SERVER_BASE_URL,
          JoinedAt: expect.any(String),
        },
      ],
      LastSignIn: expect.any(String),
      RegisteredAt: expect.any(String),
    });
  });
});
