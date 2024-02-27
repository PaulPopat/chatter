const sso = require("../clients/sso");
const { v4: guid } = require("uuid");
const url = require("../clients/url");

module.exports = async function () {
  const email = guid() + "@test.com";
  const password = guid();
  const username = guid();

  const createdUser = await sso.post(url("/api/v1/users"), {
    UserName: username,
    Email: email,
    Password: password,
  });

  return {
    email,
    password,
    username,
    admin_token: createdUser.data.AdminToken,
    server_token: createdUser.data.ServerToken,
    user_id: createdUser.data.UserId,
  };
};
