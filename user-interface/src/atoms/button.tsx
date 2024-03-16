import { PropsWithChildren } from "react";
import { Class, Classes, Colours } from "../styles/theme";
import { Pressable, Text } from "react-native";

type Props = {
  on_click: () => void;
  colour?: keyof typeof Colours;
  classes?: Array<Class>;
};

export default (props: PropsWithChildren<Props>) => {
  const colour = Colours[props.colour ?? "Primary"];
  return (
    <Pressable
      style={{
        ...Classes("container", ...(props.classes ?? [])),
        backgroundColor: colour.Background,
      }}
      onPress={props.on_click}
    >
      <Text
        style={{
          ...Classes("body_text"),
          textAlign: "center",
          color: colour.Foreground,
        }}
      >
        {props.children}
      </Text>
    </Pressable>
  );
};
