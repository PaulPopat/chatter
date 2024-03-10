require("dotenv").config({ path: "../.env" });

const server = require("../clients/server");
const auth = require("../clients/auth");
const sso = require("../clients/sso");
const url = require("../clients/url");
const createUser = require("./create-user");

module.exports = async function (isAdmin = false) {
  const user = await createUser();

  await sso.post(
    url("/api/v1/user/servers"),
    {
      ServerToken: user.server_token,
      ServerUrl: process.env.SERVER_BASE_URL,
      Password: isAdmin
        ? process.env.SERVER_ADMIN_PASSWORD
        : process.env.SERVER_PASSWORD,
    },
    auth(user.admin_token)
  );

  const response = await server.get(
    url("/api/v1/auth/token", { token: user.server_token })
  );

  return {
    ...user,
    local_token: response.data.LocalToken,
  };
};
