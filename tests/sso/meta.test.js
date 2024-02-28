require("dotenv").config({ path: "../.env" });

const sso = require("../clients/sso");
const url = require("../clients/url");

describe("meta", () => {
  it("Displays the correct cors headers", async () => {
    const options = await sso.options(url("/api/v1/heartbeat"));

    expect(options.headers).toEqual(
      expect.objectContaining({
        "access-control-allow-origin": process.env.UI_URL,
        "access-control-allow-methods": "OPTIONS, GET, PUT, POST, DELETE",
        "access-control-allow-headers": "Authorization, Content-Type",
      })
    );

    const get = await sso.get(url("/api/v1/heartbeat"));

    expect(get.headers).toEqual(
      expect.objectContaining({
        "access-control-allow-origin": process.env.UI_URL,
        "access-control-allow-methods": "OPTIONS, GET, PUT, POST, DELETE",
        "access-control-allow-headers": "Authorization, Content-Type",
      })
    );
  });
});
