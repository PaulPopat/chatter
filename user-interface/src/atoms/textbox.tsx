import { PropsWithChildren } from "react";
import { UseForm } from "./form";
import {
  TextInput,
  StyleSheet,
  KeyboardTypeOptions,
  View,
  Text,
} from "react-native";
import { z } from "zod";

type Props = {
  name: string;
  keyboard?: KeyboardTypeOptions;
  password?: boolean;
};

const styles = StyleSheet.create({
  view: {
    height: 40,
    margin: 12,
  },
  input: {
    height: 40,
    borderBottomWidth: 1,
    padding: 10,
  },
});

export default (props: PropsWithChildren<Props>) => {
  const { value, set_value, submit } = UseForm(props.name);

  return (
    <View style={styles.view}>
      <Text>{props.children}</Text>
      <TextInput
        style={styles.input}
        value={value ? z.string().parse(value) : ""}
        onChangeText={set_value}
        keyboardType={props.keyboard}
        secureTextEntry={props.password}
        onKeyPress={(e: any) => {
          if (e.key !== "Enter") return;
          submit();
        }}
      />
    </View>
  );
};
