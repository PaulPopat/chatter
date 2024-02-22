require("dotenv").config({ path: "../.env" });

const { default: axios } = require("axios");

module.exports = axios.create({
  baseURL: process.env.SERVER_BASE_URL,
});
