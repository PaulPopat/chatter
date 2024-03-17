import { PropsWithChildren } from "react";
import { View, Modal, Pressable } from "react-native";
import { Classes } from "../styles/theme";

type Props = PropsWithChildren<{
  open: boolean;
  set_open: (open: boolean) => void;
}>;

export default (props: Props) => {
  return (
    <Modal
      animationType="fade"
      transparent={true}
      visible={props.open}
      onRequestClose={() => {
        props.set_open(false);
      }}
      onDismiss={() => {
        props.set_open(false);
      }}
      style={{ maxHeight: "100%" }}
    >
      <Pressable
        style={Classes("modal_background")}
        onPress={() => props.set_open(false)}
      />
      <View style={Classes("card", "highlight", "max_fill", "spacer", "modal")}>
        {props.children}
      </View>
    </Modal>
  );
};
