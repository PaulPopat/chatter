import { PropsWithChildren } from "react";
import { BorderRadius, Colours, Margins, Padding } from "../styles/theme";
import { Pressable, StyleSheet, Text } from "react-native";

type Props = {
  on_click: () => void;
  colour?: keyof typeof Colours;
};

const styles = StyleSheet.create({
  container: {
    marginTop: Margins,
    marginBottom: Margins,
    padding: Padding,
    borderRadius: BorderRadius,
  },
  text: {
    textAlign: "center",
  },
});

export default (props: PropsWithChildren<Props>) => {
  const colour = Colours[props.colour ?? "Primary"];
  return (
    <Pressable
      style={{
        ...styles.container,
        backgroundColor: colour.Background,
      }}
      onPress={props.on_click}
    >
      <Text
        style={{
          ...styles.text,
          color: colour.Foreground,
        }}
      >
        {props.children}
      </Text>
    </Pressable>
  );
};
