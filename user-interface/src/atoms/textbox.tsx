import { PropsWithChildren, useRef, useState } from "react";
import { UseForm } from "./form";
import {
  TextInput,
  StyleSheet,
  KeyboardTypeOptions,
  View,
  Text,
  TouchableOpacity,
} from "react-native";
import { z } from "zod";
import {
  BorderRadius,
  BorderWidth,
  Colours,
  FontSizes,
  Margins,
  Padding,
} from "../styles/theme";

type Props = {
  name: string;
  keyboard?: KeyboardTypeOptions;
  password?: boolean;
  clear_on_submit?: boolean;
};

const styles = StyleSheet.create({
  view: {
    marginTop: Margins,
    marginBottom: Margins,
    borderColor: Colours.Secondary.Background,
    borderWidth: BorderWidth,
    borderRadius: BorderRadius,
  },
  viewFocus: {
    borderColor: Colours.Primary.Background,
  },
  input: {
    padding: Padding,
    fontSize: FontSizes.Label,
    paddingTop: 0,
  },
  label: {
    fontSize: FontSizes.Label,
    padding: Padding,
  },
});

export default (props: PropsWithChildren<Props>) => {
  const { value, set_value, submit, use_submit } = UseForm(props.name);
  const [focused, set_focused] = useState(false);
  const input = useRef<TextInput>(null);

  use_submit(() => {
    if (props.clear_on_submit) set_value("");
  }, [props.clear_on_submit]);

  return (
    <TouchableOpacity
      onFocus={() => {
        set_focused(true);
        input.current?.focus();
      }}
    >
      <View
        style={{
          ...styles.view,
          ...(focused ? styles.viewFocus : {}),
        }}
      >
        <Text style={styles.label}>{props.children}</Text>
        <TextInput
          ref={input}
          style={
            {
              ...styles.input,
              outlineStyle: "none",
            } as any
          }
          value={value ? z.string().parse(value) : ""}
          onChangeText={set_value}
          keyboardType={props.keyboard}
          secureTextEntry={props.password}
          onKeyPress={(e: any) => {
            if (e.key !== "Enter") return;
            submit();
          }}
          onFocus={() => set_focused(true)}
          onBlur={() => set_focused(false)}
        />
      </View>
    </TouchableOpacity>
  );
};
