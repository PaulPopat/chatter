require("dotenv").config({ path: "../.env" });
const { WebSocket } = require("ws");
const promiseWithTimeout = require("./promise-with-timeout");
const sleep = require("./sleep");

let base = process.env.SERVER_BASE_URL;
if (!base.endsWith("/")) base = base + "/";

let connections = 0;

async function connection(url) {
  const messages = [];
  const con = new WebSocket(url);

  con.addEventListener("message", (ev) => {
    const data = ev.data;
    if (typeof data !== "string") return;
    messages.push(JSON.parse(data));
  });

  await promiseWithTimeout((res, rej) => {
    con.onopen = res;
    con.onclose = rej;
    con.onerror = rej;
  });

  return {
    get messages() {
      return messages;
    },

    waitForMessage() {
      return promiseWithTimeout((res) => {
        con.addEventListener("message", function listener() {
          con.removeEventListener("message", listener);
          res();
        });
      });
    },

    sendMessage(text) {
      return Promise.all([
        promiseWithTimeout((res) => {
          con.addEventListener("message", function listener() {
            con.removeEventListener("message", listener);
            res();
          });
        }),
        new Promise(async (res) => {
          await sleep(10);
          con.send(`send:${text}`);
          res();
        }),
      ]);
    },

    close() {
      con.close();
    },
  };
}

/**
 * @param {string} url
 * @param {Record<string, string>} parameters
 */
module.exports = async function (url, parameters = {}) {
  if (url.startsWith("/")) url = url.replace("/", "");
  const ps = { ...parameters };

  for (const key in ps)
    if (url.includes(":" + key)) {
      url = url.replace(":" + key, encodeURIComponent(ps[key]));
      delete ps[key];
    }

  const params = new URLSearchParams();

  for (const key in ps) params.set(key, ps[key]);

  const searchString = params.toString();
  if (searchString) return await connection(base + url + "?" + searchString);

  return await connection(base + url);
};
