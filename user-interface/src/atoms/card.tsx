import { PropsWithChildren } from "react";
import { Class, Classes, Colours } from "../styles/theme";
import { Pressable, Text, View } from "react-native";

type Props = {
  colour?: keyof typeof Colours;
  classes?: Array<Class>;
};

export default (props: PropsWithChildren<Props>) => {
  const colour = props.colour ?? "Primary";
  return (
    <View style={Classes("card", `colour_${colour}`, ...(props.classes ?? []))}>
      {props.children}
    </View>
  );
};
