/**
 * @param {string} base
 * @param {string} url
 * @param {Record<string, string>} parameters
 */
export default function create_url(base, url, parameters = {}) {
  const ps = { ...parameters };

  if (!base.endsWith("/")) base = base + "/";
  if (url.startsWith("/")) url = url.replace("/", "");

  url = base + url;
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
}
