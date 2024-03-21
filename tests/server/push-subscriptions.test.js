const auth = require("../clients/auth");
const url = require("../clients/url");
const server = require("../clients/server");
const addUserToServer = require("../presets/add-user-to-server");

describe("push subscriptions", () => {
  // TODO: Update this to create a listener for when we support notification pushes
  it("Adds a subscription", async () => {
    const user = await addUserToServer(true);

    await server.post(
      url("/api/v1/user/push-subscriptions"),
      {
        Endpoint: "https://app.effuse.cloud/",
        Expires: "1970-01-02T10:17:36Z",
        Keys: { Test: "Hello" },
      },
      auth(user.local_token)
    );
  });
});
