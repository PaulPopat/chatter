const auth = require("../clients/auth");
const sso = require("../clients/sso");
const url = require("../clients/url");
const createUser = require("../presets/create-user");

describe("push subscriptions", () => {
  it("Adds a subscription", async () => {
    const user = await createUser();

    await sso.post(
      url("/api/v1/user/push-subscriptions"),
      {
        Endpoint: "https://app.effuse.cloud/",
        Expires: 123456,
        Keys: { Test: "Hello" },
      },
      auth(user.admin_token)
    );

    const subscriptions = await sso.get(
      url("/api/v1/user/push-subscriptions"),
      auth(user.admin_token)
    );

    expect(subscriptions.data).toEqual([
      {
        Endpoint: "https://app.effuse.cloud/",
        Expires: "1970-01-02T10:17:36Z",
        Keys: { Test: "Hello" },
      },
    ]);
  });
});
