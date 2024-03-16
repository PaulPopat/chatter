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

  it("gets a list of users", async () => {
    const admin = await addUserToServer(true);
    const user = await addUserToServer(false);
    const bannedUser = await addUserToServer(false);

    await server.post(
      url("/api/v1/banned-users"),
      {
        UserId: bannedUser.user_id,
      },
      auth(admin.local_token)
    );

    const { data } = await server.get(
      url("/api/v1/users"),
      auth(admin.local_token)
    );

    expect(data).toEqual(
      expect.arrayContaining([
        {
          UserId: admin.user_id,
          Admin: true,
          Banned: false,
        },
        {
          UserId: user.user_id,
          Admin: false,
          Banned: false,
        },
        {
          UserId: bannedUser.user_id,
          Admin: false,
          Banned: true,
        },
      ])
    );
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

  it("gets a users permissions", async () => {
    const admin = await addUserToServer(true);
    const channelName = v4();

    const { data: Channel1Data } = await server.post(
      url("/api/v1/channels"),
      { Name: channelName, Public: false },
      auth(admin.local_token)
    );

    const { data: Channel2Data } = await server.post(
      url("/api/v1/channels"),
      { Name: channelName, Public: false },
      auth(admin.local_token)
    );

    const user = await addUserToServer(false);

    await server.post(
      url("/api/v1/channels/:channel/users", {
        channel: Channel1Data.ChannelId,
      }),
      {
        UserId: user.user_id,
        AllowWrite: false,
      },
      auth(admin.local_token)
    );
    await server.post(
      url("/api/v1/channels/:channel/users", {
        channel: Channel2Data.ChannelId,
      }),
      {
        UserId: user.user_id,
        AllowWrite: true,
      },
      auth(admin.local_token)
    );

    const { data } = await server.get(
      url("/api/v1/users/:user_id/permissions", { user_id: user.user_id }),
      auth(admin.local_token)
    );

    expect(data).toEqual(
      expect.arrayContaining([
        {
          ChannelId: Channel1Data.ChannelId,
          Write: false,
        },
        {
          ChannelId: Channel2Data.ChannelId,
          Write: true,
        },
      ])
    );
  });
});
