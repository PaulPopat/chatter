import { PropsWithChildren } from "react";
import { Pressable } from "react-native";
import Icon from "./icon";
import UseOrientation from "../utils/orientation";
import { View, Text } from "./native";
import { Class } from "../styles/theme";

type Props = {
  click?: () => void;
  title: string | undefined;
  class?: Class;
};

export default (props: PropsWithChildren<Props>) => {
  const orientation = UseOrientation();
  return (
    <View class={["edge_container row border_bottom", props.class]}>
      {orientation === "portrait" && props.click && (
        <Pressable onPress={props.click}>
          <Icon area="Arrows" icon="arrow-left" />
        </Pressable>
      )}
      <Text class="flex_fill edge_container body_text">{props.title}</Text>
      {props.children}
    </View>
  );
};
