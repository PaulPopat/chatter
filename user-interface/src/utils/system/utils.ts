import { Platform } from "react-native";

type SupportedPlatform = "web";

export function SystemMatch<T>(options: Record<SupportedPlatform, () => T>) {
  const match = options[Platform.OS as SupportedPlatform];
  if (!match) throw new Error("Unsupported operating system");

  return match();
}
