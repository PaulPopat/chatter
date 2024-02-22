require("dotenv").config({ path: "../.env" });

const server = require("../clients/server");
const url = require("../clients/url");
const createUser = require("./create-user");

module.exports = async function (isAdmin = false) {
  const user = await createUser();

  const response = await server.get(
    url("/api/v1/auth/token", {
      token: user.server_token,
      password: isAdmin
        ? process.env.SERVER_ADMIN_PASSWORD
        : process.env.SERVER_PASSWORD,
    })
  );

  return {
    ...user,
    local_token: response.data.LocalToken,
  };
};
