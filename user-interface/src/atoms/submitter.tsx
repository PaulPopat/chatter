import { PropsWithChildren } from "react";
import { UseSubmitter } from "./form";
import { Button, Text } from "react-native";

type Props = {
  children: string;
};

export default (props: Props) => {
  const submit = UseSubmitter();

  return <Button title={props.children} onPress={submit} />;
};
