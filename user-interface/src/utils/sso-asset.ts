import { SSO_BASE } from "@effuse/config";
import Asset, { AssetDto } from "./asset";
import Url from "./url";

export default class SsoAsset extends Asset {
  readonly #url: Url;

  constructor(...props: ConstructorParameters<typeof Url>) {
    super();
    this.#url = new Url(...props);
  }

  get Uri() {
    return this.#url.href(SSO_BASE);
  }

  async DataTransferObject(): Promise<AssetDto> {
    const response = await fetch(this.#url.href(SSO_BASE));

    return {
      Data: btoa(
        String.fromCharCode.apply(null, [
          ...new Uint8Array(await response.arrayBuffer()),
        ])
      ),
      Mime: response.headers.get("Content-Type") ?? "",
      Encoding: "base64",
    };
  }
}
