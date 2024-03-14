import { PropsWithChildren } from "react";
import { UseForm } from "./form";
import {
  StyleSheet,
  KeyboardTypeOptions,
  View,
  Text,
  Pressable,
} from "react-native";
import Icon from "./icon";
import { Colours, Padding } from "../styles/theme";
import FilePicker from "../utils/file-picker";

type Props = {
  name: string;
  keyboard?: KeyboardTypeOptions;
  password?: boolean;
  clear_on_submit?: boolean;
};

const styles = StyleSheet.create({
  container: {
    padding: Padding,
    flexDirection: "row",
    alignItems: "center",
  },
  label: {
    margin: Padding,
  },
});

export default (props: PropsWithChildren<Props>) => {
  const { value, set_value, use_submit } = UseForm(props.name);

  use_submit(() => {
    if (props.clear_on_submit) set_value(null);
  }, [props.clear_on_submit]);

  return (
    <Pressable
      style={styles.container}
      onPress={() => FilePicker().then(set_value)}
    >
      <Icon area="System" icon="upload-cloud-2" />
      {value && value instanceof File ? (
        <Text style={styles.label}>{value.name}</Text>
      ) : (
        <Text style={styles.label}>{props.children}</Text>
      )}
    </Pressable>
  );
};
