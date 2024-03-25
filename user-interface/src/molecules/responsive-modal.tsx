import { PropsWithChildren } from "react";
import { View, Modal, StyleProp, ViewStyle } from "react-native";
import { Class, Classes, Colours, Padding } from "../styles/theme";
import UseOrientation from "../utils/orientation";

type Props = PropsWithChildren<{
  open: boolean;
  colour?: keyof typeof Colours;
  classes?: Array<Class>;
}>;

export default (props: Props) => {
  const orientation = UseOrientation();

  if (orientation === "landscape")
    return (
      <View
        style={{
          ...Classes(...(props.classes ?? [])),
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
          ...Classes("edge_container", "fill"),
          backgroundColor: Colours[props.colour ?? "Highlight"].Background,
        }}
      >
        {props.children}
      </View>
    </Modal>
  );
};
