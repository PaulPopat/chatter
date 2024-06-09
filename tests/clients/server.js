require("dotenv").config({ path: "../.env" });

const { default: axios } = require("axios");

const instance = axios.create({
  baseURL: process.env.SERVER_BASE_URL,
});


instance.interceptors.response.use(
  (r) => r,
  (error) => {
    /** @type {import('axios').AxiosResponse} */
    const response = error.response;
    if (!response) throw new Error(error);

    const request = error.request;

    throw new Error(
      [
        "Server Error",
        `URL: ${request.path}`,
        `Method: ${request.method}`,
        `Status: ${response.status}`,
      ].join("\n")
    );
  }
);

module.exports = instance;