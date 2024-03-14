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
  checkbox: {
    width: 24,
    height: 24,
    borderWidth: 2,
    borderColor: Colours.Body.Foreground,
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    margin: Padding,
  },
  label: {},
});

export default (props: PropsWithChildren<Props>) => {
  const { value, set_value, use_submit } = UseForm(props.name);

  use_submit(() => {
    if (props.clear_on_submit) set_value(false);
  }, [props.clear_on_submit]);

  return (
    <Pressable style={styles.container} onPress={() => set_value(!value)}>
      <View style={styles.checkbox}>
        {value ? <Icon area="System" icon="check" /> : undefined}
      </View>
      <Text style={styles.label}>{props.children}</Text>
    </Pressable>
  );
};
