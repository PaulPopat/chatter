require("dotenv").config({ path: "../.env" });

const { default: axios } = require("axios");
const { aws4Interceptor } = require("aws4-axios");

const instance = axios.create({
  baseURL: process.env.SSO_BASE_URL,
});

const interceptor = aws4Interceptor({
  options: {
    region: "eu-west-2",
  },
});

instance.interceptors.request.use(interceptor);

module.exports = instance;
