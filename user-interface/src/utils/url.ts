import JoinUrl from "./url-join";

export default class Url {
  readonly #url: string;
  readonly #parameters: Record<string, unknown>;
  readonly #include_query: boolean;

  constructor(
    url: string | Array<string>,
    parameters: Record<string, unknown>,
    include_query: boolean
  ) {
    this.#url = Array.isArray(url) ? JoinUrl(...url) : url;
    this.#parameters = parameters;
    this.#include_query = include_query;
  }

  href(base: string) {
    const ps = { ...this.#parameters };

    let url =
      (!base.endsWith("/") ? base + "/" : base) +
      (this.#url.startsWith("/") ? this.#url.replace("/", "") : this.#url);

    for (const key in ps)
      if (url.includes(":" + key)) {
        const value = ps[key];
        if (typeof value !== "string")
          throw new Error("May only use strings for URL parameters");
        url = url.replace(":" + key, encodeURIComponent(value));
        delete ps[key];
      }

    const params = new URLSearchParams();

    if (this.#include_query)
      for (const key in ps) {
        const value = ps[key];
        if (typeof value !== "string")
          throw new Error("May only use strings for URL parameters");
        params.set(key, encodeURIComponent(value));
      }

    const searchString = params.toString();
    if (searchString) return url + "?" + searchString;

    return url;
  }
}
