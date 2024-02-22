const sso = require("../clients/sso");
const url = require("../clients/url");
const createUser = require("../presets/create-user");

describe("identity", () => {
  it("Identifies a user", async () => {
    const user = await createUser();

    const response = await sso.get(
      url("/api/v1/auth/user", { token: user.server_token })
    );

    expect(response.data).toEqual({ UserId: user.user_id });
  });
});
