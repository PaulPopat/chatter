import { PropsWithChildren, useCallback, useState } from "react";
import { z } from "zod";
import { StyleProp, View, ViewStyle } from "react-native";
import { FormContext } from "./common";

type FormProps<TFormType> = {
  on_submit: (data: TFormType) => void;
  form_type: z.ZodType<TFormType>;

  style?: StyleProp<ViewStyle>;
};

export default function Form<TExpect>(
  props: PropsWithChildren<FormProps<TExpect>>
) {
  const [data, set_data] = useState<Record<string, unknown>>({});
  const [callbacks, set_callbacks] = useState<Array<() => void>>([]);

  const on_submit = useCallback(async () => {
    for (const callback of callbacks) callback();
    props.on_submit(props.form_type.parse(data));
  }, [data, callbacks, props.on_submit]);

  return (
    <FormContext.Provider
      value={{
        set_value(name, value) {
          set_data((d) => ({ ...d, [name]: value }));
          return { type: "ok" };
        },
        get_value(name) {
          return data[name];
        },
        on_submit(handler) {
          set_callbacks((c) => [...c, handler]);
        },
        off_submit(handler) {
          set_callbacks((c) => c.filter((h) => h !== handler));
        },
        submit: on_submit,
      }}
    >
      <View style={props.style}>{props.children}</View>
    </FormContext.Provider>
  );
}
