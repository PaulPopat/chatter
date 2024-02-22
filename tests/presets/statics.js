const fs = require("fs");
const path = require("path");

module.exports = {
  PROFILE_PICTURE: Buffer.from(
    fs.readFileSync(path.resolve(__dirname, "profile-picture.png"))
  ).toString("base64"),
  PROFILE_MIME: "picture/png",
};
