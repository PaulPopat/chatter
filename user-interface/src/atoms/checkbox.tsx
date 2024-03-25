import { PropsWithChildren, useEffect } from "react";
import { UseForm } from "./form";
import { KeyboardTypeOptions } from "react-native";
import Icon from "./icon";
import { Class } from "../styles/theme";
import { Pressable, View, Text } from "./native";

type Props = {
  name: string;
  keyboard?: KeyboardTypeOptions;
  password?: boolean;
  clear_on_submit?: boolean;

  default_value?: boolean;

  submit_on_change?: boolean;
  class?: Class;
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
      class={["row", props.class]}
      onPress={() => {
        set_value(!value);
        if (props.submit_on_change) setTimeout(() => submit(), 5);
      }}
    >
      <View class="centre bordered" style={{ width: 24, height: 24 }}>
        {value ? <Icon area="System" icon="check" /> : undefined}
      </View>
      <Text class="body_text spacer">{props.children}</Text>
    </Pressable>
  );
};
