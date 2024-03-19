import { Platform } from "react-native";

export function GetArgs(): Record<string, string> {
  switch (Platform.OS) {
    case "web":
      const query = new URLSearchParams(window.location.search);

      const result: Record<string, string> = {};
      for (const [key, value] of query.entries()) result[key] = value;

      return result;
    default:
      throw new Error("Unsupported operating system");
  }
}

export function ClearArgs() {
  switch (Platform.OS) {
    case "web":
      history.pushState(undefined, "", window.location.pathname);
    default:
      throw new Error("Unsupported operating system");
  }
}
