import { PropsWithChildren, useEffect, useRef, useState } from "react";
import { UseForm } from "./form";
import {
  KeyboardTypeOptions,
  TouchableOpacity,
  TextInput as TextInputType,
} from "react-native";
import { z } from "zod";
import { Class, FontSizes } from "../styles/theme";
import { View, Text, TextInput } from "./native";

type Props = {
  name: string;
  keyboard?: KeyboardTypeOptions;
  password?: boolean;
  clear_on_submit?: boolean;

  default_value?: string;
  multiline?: boolean;
  class?: Class;
};

const DefaultHeight = 35;

export default (props: PropsWithChildren<Props>) => {
  const { value, set_value, submit, use_submit } = UseForm(props.name);
  const [focused, set_focused] = useState(false);
  const input = useRef<TextInputType>(null);
  const [height, set_height] = useState(DefaultHeight);

  useEffect(() => {
    set_value(props.default_value ?? "");
  }, [props.default_value]);

  use_submit(() => {
    if (props.clear_on_submit) {
      set_height(DefaultHeight);
      set_value("");
    }
  }, [props.clear_on_submit]);

  return (
    <TouchableOpacity
      onFocus={() => {
        set_focused(true);
        input.current?.focus();
      }}
    >
      <View class={["card", "colour_body", props.class]}>
        <Text
          class="body_text container colour_body"
          style={{
            ...(!!(value || focused)
              ? {
                  position: "absolute",
                  top: -10,
                  left: 4,
                  padding: 2,
                  fontSize: FontSizes.Small,
                }
              : {
                  position: "absolute",
                  top: 0,
                  left: 3,
                  fontSize: FontSizes.Label,
                }),
          }}
        >
          {props.children}
        </Text>
        <TextInput
          ref={input}
          class="body_text edge_container"
          style={
            {
              outlineStyle: "none",
              height: height,
            } as any
          }
          value={value ? z.string().parse(value) : ""}
          onChangeText={set_value}
          keyboardType={props.keyboard}
          secureTextEntry={props.password}
          onKeyPress={(e: any) => {
            if (props.multiline && e.key === "Enter" && e.shiftKey) {
              e.preventDefault();
              submit();
            } else if (e.key === "Enter" && !props.multiline) {
              submit();
            }
          }}
          onFocus={() => set_focused(true)}
          onBlur={() => set_focused(false)}
          multiline={props.multiline}
          onContentSizeChange={(e) =>
            set_height(e.nativeEvent.contentSize.height)
          }
        />
      </View>
    </TouchableOpacity>
  );
};
