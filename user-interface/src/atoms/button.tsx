import { PropsWithChildren } from "react";
import { Class, Colours } from "../styles/theme";
import { Pressable, Text } from "./native";

type Props = {
  on_click: () => void;
  colour?: keyof typeof Colours;
  class?: Class;
};

export default (props: PropsWithChildren<Props>) => {
  const colour = Colours[props.colour ?? "Primary"];
  return (
    <Pressable
      class={["container", props.class]}
      style={{ backgroundColor: colour.Background }}
      onPress={props.on_click}
    >
      <Text
        class="body_text"
        style={{ textAlign: "center", color: colour.Foreground }}
      >
        {props.children}
      </Text>
    </Pressable>
  );
};
