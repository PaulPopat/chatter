import { PropsWithChildren } from "react";
import { Text } from "./native";
import { Class } from "../styles/theme";

const icons = {
  user: "\uF264",
  check: "\uEB7B",
  "upload-cloud-2": "\uF24C",
  "loader-2": "\uEEC2",
  close: "\uEB99",
  "arrow-left": "\uEA60",
  "settings-2": "\uF0E4",
  "send-plane-2": "\uF0D8",
  "lock-2": "\uEECC",
  "chat-3": "\uEB51",
};

type Props = {
  icon: keyof typeof icons;
  area?: string;
  type?: "line" | "fill";
  size?: number;
  class?: Class;
};

export default (props: PropsWithChildren<Props>) => (
  <Text
    class={props.class}
    style={{
      fontSize: props.size,
      fontFamily: "RemixIcon",
    }}
  >
    {icons[props.icon]}
  </Text>
);
