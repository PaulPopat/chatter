import { PropsWithChildren, useCallback } from "react";
import { z } from "zod";
import { Fetcher } from "../../utils/fetch";
import { StyleProp, View, ViewStyle } from "react-native";
import RawForm from "./raw-form";

type FormProps<TExpect> = {
  fetcher: Fetcher<TExpect>;
  style?: StyleProp<ViewStyle>;
};

export default function Form<TExpect>(
  props: PropsWithChildren<FormProps<TExpect>>
) {
  const on_submit = useCallback(
    async (data: any) => {
      try {
        await props.fetcher(data);
      } catch (response: unknown) {
        if (response instanceof Response) console.error(response);
        else throw response;
      }
    },
    [props]
  );

  return (
    <RawForm on_submit={on_submit} form_type={z.any()} style={props.style}>
      <View style={props.style}>{props.children}</View>
    </RawForm>
  );
}
