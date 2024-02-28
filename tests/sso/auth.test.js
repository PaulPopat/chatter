const auth = require("../clients/auth");
const sso = require("../clients/sso");
const url = require("../clients/url");
const createUser = require("../presets/create-user");

describe("auth", () => {
  it("Creats a basic user", async () => {
    const user = await createUser();

    const profile = await sso.get(
      url("/api/v1/users/:user_id/profile", {
        user_id: user.user_id,
      })
    );

    expect(profile.data).toEqual({
      UserId: user.user_id,
      UserName: user.username,
      Biography: "",
    });
  });

  it("Allows private actions with admin", async () => {
    const user = await createUser();

    const profile = await sso.get(
      url("/api/v1/user/profile"),
      auth(user.admin_token)
    );

    expect(profile.data).toEqual({
      UserId: user.user_id,
      UserName: user.username,
      Biography: "",
      Servers: [],
      LastSignIn: expect.any(String),
      RegisteredAt: expect.any(String),
    });
  });

  it("Allows login", async () => {
    const user = await createUser();

    const logInResponse = await sso.get(
      url("/api/v1/auth/token", { email: user.email, password: user.password })
    );

    const profile = await sso.get(
      url("/api/v1/user/profile"),
      auth(logInResponse.data.AdminToken)
    );

    expect(profile.data).toEqual({
      UserId: user.user_id,
      UserName: user.username,
      Biography: "",
      Servers: [],
      LastSignIn: expect.any(String),
      RegisteredAt: expect.any(String),
    });
  });

  it("Allows refresh", async () => {
    const user = await createUser();

    const logInResponse = await sso.get(
      url("/api/v1/auth/refresh-token", { token: user.refresh_token })
    );

    const profile = await sso.get(
      url("/api/v1/user/profile"),
      auth(logInResponse.data.AdminToken)
    );

    expect(profile.data).toEqual({
      UserId: user.user_id,
      UserName: user.username,
      Biography: "",
      Servers: [],
      LastSignIn: expect.any(String),
      RegisteredAt: expect.any(String),
    });
  });
});
