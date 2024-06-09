require("dotenv").config({ path: "../.env" });

const { default: axios } = require("axios");

const instance = axios.create({
  baseURL: process.env.SSO_BASE_URL,
});

instance.interceptors.response.use(
  (r) => r,
  (error) => {
    /** @type {import('axios').AxiosResponse} */
    const response = error.response;
    if (!response) throw new Error(error);

    /** @type {import('axios').AxiosRequestConfig} */
    const request = response.request;

    throw new Error(
      [
        "SSO Error",
        `URL: ${request.url}`,
        `BaseURL: ${request.baseURL}`,
        `Method: ${request.method}`,
        `Status: ${response.status}`,
      ].join("\n")
    );
  }
);

module.exports = instance;