import { PropsWithChildren } from "react";
import { Pressable, View, Text } from "react-native";
import Icon from "./icon";
import { Classes } from "../styles/theme";
import UseOrientation from "../utils/orientation";

type Props = {
  click: () => void;
  title: string | undefined;
};

export default (props: PropsWithChildren<Props>) => {
  const orientation = UseOrientation();
  return (
    <View style={Classes("edge_container", "row", "border_bottom")}>
      {orientation === "portrait" && (
        <Pressable onPress={props.click}>
          <Icon area="Arrows" icon="arrow-left" />
        </Pressable>
      )}
      <Text style={Classes("flex_fill", "edge_container", "body_text")}>
        {props.title}
      </Text>
      {props.children}
    </View>
  );
};
