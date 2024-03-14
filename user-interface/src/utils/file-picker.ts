import { Platform } from "react-native";

export default function FilePicker(): Promise<File> {
  switch (Platform.OS) {
    case "web":
      return new Promise<File>((res, rej) => {
        var input = document.createElement("input");
        input.type = "file";

        input.onchange = (e: Event) => {
          const target = e.target;
          if (!(target instanceof HTMLInputElement) || !target.files) return;

          const file = target.files[0];
          if (!(file instanceof File)) rej("Not a valid file");
          else res(file);
        };

        input.click();
      });
    default:
      throw new Error("Unsupported operating system");
  }
}
