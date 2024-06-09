const { v4 } = require("uuid");
const auth = require("../clients/auth");
const server = require("../clients/server");
const url = require("../clients/url");
const addUserToServer = require("../presets/add-user-to-server");
const createRole = require("../presets/create-role");

describe("roles", () => {
  it("allows role creation", async () => {
    const authenticated = await addUserToServer(true);
    const { name } = await createRole(authenticated);

    const roles = await server.get("/api/v1/roles", auth(authenticated.local_token));

    expect(roles.data).toEqual(
      expect.arrayContaining([
        expect.objectContaining({
          Name: name,
          Admin: false,
          Policies: [],
        }),
      ])
    );
  });

  it("adds a role to a channel", async () => {
    const admin = await addUserToServer(true);
    const channelName = v4();

    const channelResponse = await server.post(
      url("/api/v1/channels"),
      { Name: channelName, Public: false },
      auth(admin.local_token)
    );

    const user = await addUserToServer(false);
    const role = await createRole(admin);
    await role.add_user(user);
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

    await role.add_channel(channelResponse.data);

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

  it("kicks a role from a channel", async () => {
    const admin = await addUserToServer(true);
    const channelName = v4();

    const channelResponse = await server.post(
      url("/api/v1/channels"),
      { Name: channelName, Public: false },
      auth(admin.local_token)
    );

    const user = await addUserToServer(false);
    const role = await createRole(admin);
    await role.add_user(user);

    await role.add_channel(channelResponse.data);
    await role.kick_channel(channelResponse.data);

    const channels = await server.get(
      "/api/v1/channels",
      auth(user.local_token)
    );

    expect(channels.data).not.toEqual(
      expect.arrayContaining([
        expect.objectContaining({
          Name: channelName,
          Type: "Messages",
        }),
      ])
    );
  });
});
