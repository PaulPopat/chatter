import { PropsWithChildren } from "react";
import { View, Modal, StyleProp, ViewStyle } from "react-native";
import { Colours, Padding } from "../styles/theme";
import UseOrientation from "../utils/orientation";

type Props = PropsWithChildren<{
  open: boolean;
  colour?: keyof typeof Colours;
  style: ViewStyle;
}>;

export default (props: Props) => {
  const orientation = UseOrientation();

  if (orientation === "landscape")
    return (
      <View
        style={{
          ...props.style,
          opacity: props.open ? 1 : 0,
          backgroundColor: Colours[props.colour ?? "Highlight"].Background,
        }}
      >
        {props.children}
      </View>
    );

  return (
    <Modal animationType="slide" transparent={true} visible={props.open}>
      <View
        style={{
          backgroundColor: Colours[props.colour ?? "Highlight"].Background,
          padding: Padding,
          height: "100%",
        }}
      >
        {props.children}
      </View>
    </Modal>
  );
};
