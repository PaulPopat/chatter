import { PropsWithChildren } from "react";
import { UseForm } from "./form";
import { KeyboardTypeOptions, Text, Pressable } from "react-native";
import Icon from "./icon";
import { Classes } from "../styles/theme";
import FilePicker from "../utils/file-picker";

type Props = {
  name: string;
  keyboard?: KeyboardTypeOptions;
  password?: boolean;
  clear_on_submit?: boolean;
};

export default (props: PropsWithChildren<Props>) => {
  const { value, set_value, use_submit } = UseForm(props.name);

  use_submit(() => {
    if (props.clear_on_submit) set_value(null);
  }, [props.clear_on_submit]);

  return (
    <Pressable
      style={Classes("container", "row")}
      onPress={() => FilePicker().then(set_value)}
    >
      <Icon area="System" icon="upload-cloud-2" />
      {value && value instanceof File ? (
        <Text style={Classes("body_text", "spacer")}>{value.name}</Text>
      ) : (
        <Text style={Classes("body_text", "spacer")}>{props.children}</Text>
      )}
    </Pressable>
  );
};
