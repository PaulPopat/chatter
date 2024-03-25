import Json from "../json";
import { SystemMatch } from "./utils";

export const Session = SystemMatch({
  web: () =>
    new Proxy<Record<string, unknown>>(
      {},
      {
        get(_, p) {
          if (typeof p !== "string")
            throw new Error("May only index session storage by string");

          const value = window.sessionStorage.getItem(p);

          if (!value) return undefined;
          return Json.Parse(value);
        },
        set(_, p, input) {
          if (typeof p !== "string")
            throw new Error("May only index session storage by string");

          window.sessionStorage.setItem(p, Json.ToString(input));
          return true;
        },
      }
    ),
});
