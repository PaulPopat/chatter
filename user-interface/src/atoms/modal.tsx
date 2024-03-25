import { PropsWithChildren } from "react";
import TopBar from "./top-bar";
import Icon from "./icon";
import { Modal, Pressable, ScrollView, View } from "./native";

type Props = PropsWithChildren<{
  open: boolean;
  set_open: (open: boolean) => void;
  class?: string;
  title: string;
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
        class="modal_background"
        onPress={() => props.set_open(false)}
      />
      <View class="max_fill modal container">
        <View class={["card highlight flush column max_fill", props.class]}>
          <TopBar title={props.title}>
            <Pressable onPress={() => props.set_open(false)}>
              <Icon area="System" icon="close" class="body-text" />
            </Pressable>
          </TopBar>
          <ScrollView class="container flex_fill">{props.children}</ScrollView>
        </View>
      </View>
    </Modal>
  );
};
