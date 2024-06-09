require("dotenv").config({ path: "../.env" });
const { v4 } = require("uuid");

const server = require("../clients/server");
const auth = require("../clients/auth");
const url = require("../clients/url");

module.exports = async function (authenticated) {
  const roleName = v4();

  const response = await server.post(
    url("/api/v1/roles"),
    { Name: roleName, Public: false },
    auth(authenticated.local_token)
  );

  return {
    role_id: response.data.RoleId,
    name: response.data.Name,
    async add_channel(channelData) {
      await server.post(
        url("/api/v1/roles/:role/channels", {
          role: response.data.RoleId,
        }),
        {
          ChannelId: channelData.ChannelId,
          AllowWrite: true,
        },
        auth(authenticated.local_token)
      );
    },
    async kick_channel(channelData) {
      await server.delete(
        url("/api/v1/roles/:role/channels/:channel", {
          role: response.data.RoleId,
          channel: channelData.ChannelId,
        }),
        auth(authenticated.local_token)
      );
    },
    async add_user(user) {
      await server.put(
        url("/api/v1/users/:user/role", { user: user.user_id }),
        { RoleId: response.data.RoleId },
        auth(authenticated.local_token)
      );
    },
  };
};
