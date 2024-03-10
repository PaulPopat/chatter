const { v4 } = require("uuid");
const auth = require("../clients/auth");
const server = require("../clients/server");
const url = require("../clients/url");
const addUserToServer = require("../presets/add-user-to-server");
const { PROFILE_MIME, PROFILE_PICTURE } = require("../presets/statics");

describe("metadata", () => {
  it("updates the server metadata", async () => {
    const authenticated = await addUserToServer(true);
    const serverName = v4();

    await server.put(
      url("/api/v1/server/metadata"),
      {
        ServerName: serverName,
        IconBase64: PROFILE_PICTURE,
        IconMimeType: PROFILE_MIME,
      },
      auth(authenticated.local_token)
    );

    const metadata = await server.get("/api/v1/server/metadata");

    expect(metadata.data).toEqual({
      ServerName: serverName,
      Icon: {
        Base64Data: PROFILE_PICTURE,
        MimeType: PROFILE_MIME,
      },
    });
  });
});
