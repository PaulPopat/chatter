const auth = require("../clients/auth");
const server = require("../clients/server");
const url = require("../clients/url");
const addUserToServer = require("../presets/add-user-to-server");

describe("auth", () => {
  it("registers with the server", async () => {
    const authenticated = await addUserToServer();
    expect(authenticated.local_token).toEqual(expect.any(String));
  });

  it("blocks admin tasks", async () => {
    const authenticated = await addUserToServer();

    expect(
      server.post(
        url("/api/v1/channels", { Name: "test", Public: false }),
        auth(authenticated.local_token)
      )
    ).rejects.toThrow();
  });
});
