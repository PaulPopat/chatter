const auth = require("../clients/auth");
const server = require("../clients/server");
const url = require("../clients/url");
const addUserToServer = require("../presets/add-user-to-server");
const { v4 } = require("uuid");

describe("users", () => {
  it("bans a user", async () => {
    const admin = await addUserToServer(true);
    const user = await addUserToServer(false);

    await server.post(
      url("/api/v1/banned-users"),
      {
        UserId: user.user_id,
      },
      auth(admin.local_token)
    );

    expect(
      server.get(url("/api/v1/channels"), auth(user.local_token))
    ).rejects.toThrow();
  });

  it("makes a user admin", async () => {
    const admin = await addUserToServer(true);
    const user = await addUserToServer(false);
    const channelName = v4();

    await server.post(
      url("/api/v1/admin-users"),
      {
        UserId: user.user_id,
      },
      auth(admin.local_token)
    );

    await server.post(
      url("/api/v1/channels"),
      { Name: channelName, Public: false },
      auth(user.local_token)
    );

    const channels = await server.get(
      "/api/v1/channels",
      auth(user.local_token)
    );

    expect(channels.data).toEqual(
      expect.arrayContaining([
        expect.objectContaining({
          Name: channelName,
          Type: "Messages",
        }),
      ])
    );
  });
});
