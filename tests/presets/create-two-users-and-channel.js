const server = require("../clients/server");
const url = require("../clients/url");
const addUserToServer = require("./add-user-to-server");
const { v4 } = require("uuid");
const auth = require('../clients/auth');

module.exports = async function () {
  const adminUser = await addUserToServer(true);
  const localUser = await addUserToServer(false);

  const channelName = v4();

  const { data: createdChannel } = await server.post(
    url("/api/v1/channels"),
    { Name: channelName, Public: false },
    auth(adminUser.local_token)
  );

  await server.post(
    url("/api/v1/channels/:channel/users", {
      channel: createdChannel.ChannelId,
    }),
    {
      UserId: localUser.user_id,
      AllowWrite: true,
    },
    auth(adminUser.local_token)
  );

  return {
    adminUser,
    localUser,
    createdChannel,
  };
};
