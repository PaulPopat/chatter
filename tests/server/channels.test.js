const auth = require("../clients/auth");
const server = require("../clients/server");
const url = require("../clients/url");
const addUserToServer = require("../presets/add-user-to-server");

describe("channels", () => {
  it("allows channel creation", async () => {
    const authenticated = await addUserToServer(true);

    await server.post(
      url("/api/v1/channels"),
      { Name: "test", Public: false },
      auth(authenticated.local_token)
    );

    const channels = await server.get(
      "/api/v1/channels",
      auth(authenticated.local_token)
    );

    expect(channels.data).toEqual(
      expect.arrayContaining([
        expect.objectContaining({
          Name: "test",
          Type: "Messages",
        }),
      ])
    );
  });
});
