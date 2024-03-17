import IResource from "./i-resource";
import DefaultAsset from "remixicon/icons/Communication/chat-3-line.svg";

export type Encoding = "utf8" | "base64";

export default class DataUrl implements IResource {
  readonly #encoding: Encoding;
  readonly #mime: string;
  readonly #data: string;

  constructor(data: string, mime: string, encoding: Encoding = "base64") {
    this.#encoding = encoding;
    this.#mime = mime;
    this.#data = data;
  }

  get Uri() {
    if (!this.#data) return `data:image/svg+xml;utf8,${DefaultAsset}`;
    return `data:${this.#mime};${this.#encoding},${this.#data}`;
  }
}
