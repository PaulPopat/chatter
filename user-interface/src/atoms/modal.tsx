import { PropsWithChildren } from "react";
import { View, StyleSheet, Modal } from "react-native";
import {
  BorderRadius,
  BorderWidth,
  Colours,
  Margins,
  Padding,
} from "../styles/theme";

type Props = PropsWithChildren<{
  open: boolean;
  set_open: (open: boolean) => void;
}>;

const styles = StyleSheet.create({
  modal_body: {
    backgroundColor: Colours.Highlight.Background,
    color: Colours.Highlight.Foreground,
    margin: Margins,
    padding: Padding,
    borderWidth: BorderWidth,
    borderRadius: BorderRadius,
  },
});

export default (props: Props) => {
  return (
    <Modal
      animationType="slide"
      transparent={true}
      visible={props.open}
      onRequestClose={() => {
        props.set_open(false);
      }}
      onDismiss={() => {
        props.set_open(false);
      }}
    >
      <View style={styles.modal_body}>{props.children}</View>
    </Modal>
  );
};
