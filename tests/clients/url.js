/**
 * @param {string} url
 * @param {Record<string, string>} parameters
 */
module.exports = function (url, parameters = {}) {
  const ps = { ...parameters };

  for (const key in ps)
    if (url.includes(":" + key)) {
      url = url.replace(":" + key, encodeURIComponent(ps[key]));
      delete ps[key];
    }

  const params = new URLSearchParams();

  for (const key in ps) params.set(key, ps[key]);

  const searchString = params.toString();
  if (searchString) return url + "?" + searchString;

  return url;
};
