import { PropsWithChildren, useCallback, useRef, useState } from "react";
import { z } from "zod";
import { FormContext } from "./common";
import { Class } from "../../styles/theme";
import { View } from "../native";

type FormProps<TFormType> = {
  on_submit: (data: TFormType) => void;
  form_type: z.ZodType<TFormType>;

  class?: Class;
};

export default function Form<TExpect>(
  props: PropsWithChildren<FormProps<TExpect>>
) {
  const [data, set_data] = useState<Record<string, unknown>>({});
  const [callbacks, set_callbacks] = useState<Array<() => void>>([]);
  const dataRef = useRef<Record<string, unknown>>();

  dataRef.current = data;

  const on_submit = useCallback(async () => {
    for (const callback of callbacks) callback();
    const data = dataRef.current;
    props.on_submit(props.form_type.parse(data));
  }, [dataRef, callbacks, props.on_submit]);

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
      <View class={["column", props.class]}>{props.children}</View>
    </FormContext.Provider>
  );
}
