export type Encoding = "utf8" | "base64";

export default class DataUrl {
  readonly #encoding: Encoding;
  readonly #mime: string;
  readonly #data: string;

  constructor(data: string, mime: string, encoding: Encoding = "base64") {
    this.#encoding = encoding;
    this.#mime = mime;
    this.#data = data;
  }

  get Uri() {
    return `data:${this.#mime};${this.#encoding},${this.#data}`;
  }
}
