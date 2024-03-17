import { SSO_BASE } from "@effuse/config";
import IResource from "./i-resource";
import Url from "./url";

export default class SsoAsset extends Url implements IResource {
  get Uri() {
    return super.href(SSO_BASE);
  }
}
