import { PropsWithChildren } from "react";
import { View, Modal, Pressable, ScrollView } from "react-native";
import { Classes } from "../styles/theme";
import TopBar from "./top-bar";
import Icon from "./icon";

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
        style={Classes("modal_background")}
        onPress={() => props.set_open(false)}
      />
      <View style={Classes("max_fill modal container")}>
        <View style={Classes("card highlight flush column max_fill", props.class ?? "")}>
          <TopBar title={props.title}>
            <Pressable onPress={() => props.set_open(false)}>
              <Icon area="System" icon="close" />
            </Pressable>
          </TopBar>
          <ScrollView style={Classes("container flex_fill")}>
            {props.children}
          </ScrollView>
        </View>
      </View>
    </Modal>
  );
};
