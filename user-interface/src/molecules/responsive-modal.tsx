import { PropsWithChildren } from "react";
import { Class, Colours } from "../styles/theme";
import UseOrientation from "../utils/orientation";
import { Modal, View } from "../atoms/native";

type Props = PropsWithChildren<{
  open: boolean;
  colour?: keyof typeof Colours;
  class?: Class;
}>;

export default (props: Props) => {
  const orientation = UseOrientation();
  const colour = Colours[props.colour ?? "Highlight"];

  if (orientation === "landscape")
    return (
      <View
        class={props.class}
        style={{
          opacity: props.open ? 1 : 0,
          backgroundColor: colour.Background,
        }}
      >
        {props.children}
      </View>
    );

  return (
    <Modal animationType="slide" transparent={true} visible={props.open}>
      <View
        class="edge_container fill"
        style={{
          backgroundColor: colour.Background,
        }}
      >
        {props.children}
      </View>
    </Modal>
  );
};
