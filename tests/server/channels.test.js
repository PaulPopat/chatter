const { v4 } = require("uuid");
const auth = require("../clients/auth");
const server = require("../clients/server");
const url = require("../clients/url");
const addUserToServer = require("../presets/add-user-to-server");

describe("channels", () => {
  it("allows channel creation", async () => {
    const authenticated = await addUserToServer(true);
    const channelName = v4();

    await server.post(
      url("/api/v1/channels"),
      { Name: channelName, Public: false },
      auth(authenticated.local_token)
    );

    const channels = await server.get(
      "/api/v1/channels",
      auth(authenticated.local_token)
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

  it("adds a user to a channel", async () => {
    const admin = await addUserToServer(true);
    const channelName = v4();

    const channelResponse = await server.post(
      url("/api/v1/channels"),
      { Name: channelName, Public: false },
      auth(admin.local_token)
    );

    const user = await addUserToServer(false);
    const channelsBefore = await server.get(
      "/api/v1/channels",
      auth(user.local_token)
    );

    expect(channelsBefore.data).not.toEqual(
      expect.arrayContaining([
        expect.objectContaining({
          Name: channelName,
          Type: "Messages",
        }),
      ])
    );

    await server.post(
      url("/api/v1/channels/:channel/users", {
        channel: channelResponse.data.ChannelId,
      }),
      {
        UserId: user.user_id,
        AllowWrite: true,
      },
      auth(admin.local_token)
    );

    const channelsAfter = await server.get(
      "/api/v1/channels",
      auth(user.local_token)
    );

    expect(channelsAfter.data).toEqual(
      expect.arrayContaining([
        expect.objectContaining({
          Name: channelName,
          Type: "Messages",
        }),
      ])
    );
  });
});
