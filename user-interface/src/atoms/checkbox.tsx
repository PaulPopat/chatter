import { PropsWithChildren, useEffect } from "react";
import { UseForm } from "./form";
import { KeyboardTypeOptions, View, Text, Pressable } from "react-native";
import Icon from "./icon";
import { Class, Classes } from "../styles/theme";

type Props = {
  name: string;
  keyboard?: KeyboardTypeOptions;
  password?: boolean;
  clear_on_submit?: boolean;

  default_value?: boolean;

  submit_on_change?: boolean;
  classes?: Array<Class>;
};

export default (props: PropsWithChildren<Props>) => {
  const { value, set_value, use_submit, submit } = UseForm(props.name);

  use_submit(() => {
    if (props.clear_on_submit) set_value(false);
  }, [props.clear_on_submit]);

  useEffect(() => {
    set_value(props.default_value ?? false);
  }, [props.default_value]);

  return (
    <Pressable
      style={Classes("row", ...(props.classes ?? []))}
      onPress={() => {
        set_value(!value);
        if (props.submit_on_change) setTimeout(() => submit(), 5);
      }}
    >
      <View
        style={{
          ...Classes("centre", "bordered"),
          width: 24,
          height: 24,
        }}
      >
        {value ? <Icon area="System" icon="check" /> : undefined}
      </View>
      <Text style={Classes("body_text", "spacer")}>{props.children}</Text>
    </Pressable>
  );
};
