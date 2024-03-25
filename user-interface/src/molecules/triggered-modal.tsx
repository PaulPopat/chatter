import { PropsWithChildren, useState } from "react";
import { Pressable, View } from "react-native";
import Modal from "../atoms/modal";

type Props = {
  title: string;
  button: JSX.Element;
};

export default (props: PropsWithChildren<Props>) => {
  const [open, set_open] = useState(false);

  return (
    <>
      <Pressable onPress={() => set_open(true)}>{props.button}</Pressable>
      <Modal {...{ open, set_open }} title={props.title}>
        {props.children}
      </Modal>
    </>
  );
};
