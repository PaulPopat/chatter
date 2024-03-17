import IAsset, { AssetDto } from "./asset";
import DefaultAsset from "remixicon/icons/Communication/chat-3-line.svg";

export type Encoding = "utf8" | "base64";

export default class DataAsset extends IAsset {
  readonly #encoding: Encoding;
  readonly #mime: string;
  readonly #data: string;

  constructor(data: string, mime: string, encoding: Encoding = "base64") {
    super();
    this.#encoding = encoding;
    this.#mime = mime;
    this.#data = data;
  }
  async DataTransferObject(): Promise<AssetDto> {
    return {
      Data: this.#data,
      Mime: this.#mime,
      Encoding: this.#encoding,
    };
  }

  get Uri() {
    if (!this.#data) return `data:image/svg+xml;utf8,${DefaultAsset}`;
    return `data:${this.#mime};${this.#encoding},${this.#data}`;
  }

  static get Default() {
    return new DataAsset("", "");
  }
}
