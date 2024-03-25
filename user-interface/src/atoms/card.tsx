import { PropsWithChildren } from "react";
import { Class, Colours } from "../styles/theme";
import { View } from "./native";

type Props = {
  colour?: keyof typeof Colours;
  class?: Class;
};

export default (props: PropsWithChildren<Props>) => {
  const colour = props.colour ?? "primary";
  return (
    <View class={[`card colour_${colour}`, props.class]}>{props.children}</View>
  );
};
