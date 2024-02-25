const sleep = require("../clients/sleep");
const webSocket = require("../clients/web-socket");
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

    adminCon = await webSocket("/ws/chat", {
      token: data.adminUser.local_token,
      channelId: data.createdChannel.ChannelId,
    });

    localCon = await webSocket("/ws/chat", {
      token: data.localUser.local_token,
      channelId: data.createdChannel.ChannelId,
    });

    await sleep(100);

    await Promise.all([
      localCon.waitForMessage(),
      adminCon.sendMessage("test message"),
    ]);

    expect(adminCon.messages).toEqual([
      { Type: "Info", Message: "MESSAGE_SENT" },
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

    adminCon = await webSocket("/ws/chat", {
      token: data.adminUser.local_token,
      channelId: data.createdChannel.ChannelId,
    });

    await sleep(100);
    await adminCon.sendMessage("test message 1");
    await adminCon.sendMessage("test message 2");

    localCon = await webSocket("/ws/chat", {
      token: data.localUser.local_token,
      channelId: data.createdChannel.ChannelId,
    });

    await sleep(100);

    await localCon.sendBacklog(0);

    expect(localCon.messages).toEqual([
      [
        {
          Text: "test message 2",
          Type: "Message",
          When: expect.any(String),
          Who: expect.any(String),
        },
        {
          Text: "test message 1",
          Type: "Message",
          When: expect.any(String),
          Who: expect.any(String),
        },
      ],
    ]);
  });

  it("paginates backlog", async () => {
    const data = await createTwoUsersAndChannel();

    adminCon = await webSocket("/ws/chat", {
      token: data.adminUser.local_token,
      channelId: data.createdChannel.ChannelId,
    });

    await sleep(100);
    for (let i = 0; i < 400; i++) {
      await adminCon.sendMessage(`test message ${i}`);
    }

    localCon = await webSocket("/ws/chat", {
      token: data.localUser.local_token,
      channelId: data.createdChannel.ChannelId,
    });

    await sleep(1000);

    await localCon.sendBacklog(0);
    await localCon.sendBacklog(25);

    expect(localCon.messages).toEqual([
      Array.apply(null, Array(20)).map((_, i) => ({
        Text: `test message ${399 - i}`,
        Type: "Message",
        When: expect.any(String),
        Who: expect.any(String),
      })),
      Array.apply(null, Array(20)).map((_, i) => ({
        Text: `test message ${374 - i}`,
        Type: "Message",
        When: expect.any(String),
        Who: expect.any(String),
      })),
    ]);
  }, 65_000);
});
