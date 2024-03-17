import { Platform } from "react-native";
import IAsset from "./asset";
import { ToBase64 } from "./file";
import DataAsset from "./data-asset";

export default function FilePicker(accept?: string): Promise<IAsset> {
  switch (Platform.OS) {
    case "web":
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
    default:
      throw new Error("Unsupported operating system");
  }
}
