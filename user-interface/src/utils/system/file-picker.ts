import IAsset from "../asset";
import { ToBase64 } from "../file";
import DataAsset from "../data-asset";
import { SystemMatch } from "./utils";

export default SystemMatch({
  web: () => (accept?: string) => {
    const input = document.createElement("input");
    input.type = "file";
    if (accept) input.accept = accept;

    return new Promise<IAsset>((res, rej) => {
      setTimeout(() => {
        input.addEventListener("change", (e) => {
          const target = e.target;
          if (!(target instanceof HTMLInputElement) || !target.files) return;

          const file = target.files[0];
          if (!(file instanceof File)) rej("Not a valid file");
          else
            ToBase64(file).then((f) => {
              res(new DataAsset(f.base64, f.mime, "base64"));
              input.remove();
            });
        });

        setTimeout(() => {
          input.click();
        }, 10);
      }, 10);
    });
  },
});
