export default class Url {
  readonly #url: string;
  readonly #parameters: Record<string, string>;

  constructor(url: string, parameters: Record<string, string>) {
    this.#url = url;
    this.#parameters = parameters;
  }

  href(base: string) {
    const ps = { ...this.#parameters };

    let url =
      (!base.endsWith("/") ? base + "/" : base) +
      (this.#url.startsWith("/") ? this.#url.replace("/", "") : this.#url);

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
}
