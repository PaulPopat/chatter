const sleep = require("../clients/sleep");
const webSocket = require("../clients/web-socket");
const server = require("../clients/server");
const url = require("../clients/url");
const auth = require("../clients/auth");
const createTwoUsersAndChannel = require("../presets/create-two-users-and-channel");

describe("chat", () => {
  let adminCon;
  let localCon;

  afterEach(() => {
    adminCon?.close();
    adminCon = undefined;
    localCon?.close();
    localCon = undefined;
  });

  it("may post to chat channel", async () => {
    const data = await createTwoUsersAndChannel();

    adminCon = await webSocket("/ws/chat/:channelId", {
      token: data.adminUser.local_token,
      channelId: data.createdChannel.ChannelId,
    });

    localCon = await webSocket("/ws/chat/:channelId", {
      token: data.localUser.local_token,
      channelId: data.createdChannel.ChannelId,
    });

    await sleep(1000);

    await Promise.all([
      localCon.waitForMessage(),
      server.post(
        url("/api/v1/channels/:channel/messages", {
          channel: data.createdChannel.ChannelId,
        }),
        {
          Text: "test message",
        },
        auth(data.adminUser.local_token)
      ),
    ]);

    expect(localCon.messages).toEqual([
      {
        Text: "test message",
        Type: "Message",
        When: expect.any(String),
        Who: expect.any(String),
      },
    ]);
  });

  it("retrieves a backlog", async () => {
    const data = await createTwoUsersAndChannel();

    await sleep(100);
    await server.post(
      url("/api/v1/channels/:channel/messages", {
        channel: data.createdChannel.ChannelId,
      }),
      {
        Text: "test message 1",
      },
      auth(data.adminUser.local_token)
    );
    await server.post(
      url("/api/v1/channels/:channel/messages", {
        channel: data.createdChannel.ChannelId,
      }),
      {
        Text: "test message 2",
      },
      auth(data.adminUser.local_token)
    );

    const { data: backlog } = await server.get(
      url("/api/v1/channels/:channel/messages", {
        channel: data.createdChannel.ChannelId,
        offset: "0",
      }),
      auth(data.localUser.local_token)
    );

    expect(backlog).toEqual([
      {
        Text: "test message 2",
        When: expect.any(String),
        Who: expect.any(String),
      },
      {
        Text: "test message 1",
        When: expect.any(String),
        Who: expect.any(String),
      },
    ]);
  });

  it("paginates backlog", async () => {
    const data = await createTwoUsersAndChannel();

    for (let i = 0; i < 400; i++) {
      await server.post(
        url("/api/v1/channels/:channel/messages", {
          channel: data.createdChannel.ChannelId,
        }),
        {
          Text: `test message ${i}`,
        },
        auth(data.adminUser.local_token)
      );
    }

    const { data: backlog1 } = await server.get(
      url("/api/v1/channels/:channel/messages", {
        channel: data.createdChannel.ChannelId,
        offset: "0",
      }),
      auth(data.localUser.local_token)
    );

    const { data: backlog2 } = await server.get(
      url("/api/v1/channels/:channel/messages", {
        channel: data.createdChannel.ChannelId,
        offset: "25",
      }),
      auth(data.localUser.local_token)
    );

    expect(backlog1).toEqual(
      Array.apply(null, Array(20)).map((_, i) => ({
        Text: `test message ${399 - i}`,
        When: expect.any(String),
        Who: expect.any(String),
      }))
    );

    expect(backlog2).toEqual(
      Array.apply(null, Array(20)).map((_, i) => ({
        Text: `test message ${374 - i}`,
        When: expect.any(String),
        Who: expect.any(String),
      }))
    );
  });
});
