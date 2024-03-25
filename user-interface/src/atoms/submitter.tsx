import { Class, Colours } from "../styles/theme";
import { UseSubmitter } from "./form";
import Button from "./button";
import { PropsWithChildren } from "react";

type Props = {
  colour?: keyof typeof Colours;
  class?: Class;
};

export default (props: PropsWithChildren<Props>) => {
  const submit = UseSubmitter();

  return (
    <Button colour={props.colour} on_click={submit} class={props.class}>
      {props.children}
    </Button>
  );
};
