import { PropsWithChildren, useCallback } from "react";
import { z } from "zod";
import UseFetcher from "../../utils/fetch";
import { StyleProp, View, ViewStyle } from "react-native";
import RawForm from "./raw-form";

type FormProps<TExpect> = {
  url: string;
  method: "GET" | "PUT" | "POST" | "DELETE";
  area: "server" | "sso";
  no_auth?: boolean;
  expect?: z.ZodType<TExpect>;

  on_success?: (response: Response, data: TExpect) => void;
  on_fail?: (response: Response) => void;

  style?: StyleProp<ViewStyle>;
};

export default function Form<TExpect>(
  props: PropsWithChildren<FormProps<TExpect>>
) {
  const fetcher = UseFetcher(props.url, {
    method: props.method,
    area: props.area,
    no_auth: props.no_auth,
    expect: props.expect,
  });

  const on_submit = useCallback(
    async (data: any) => {
      try {
        const { response, data: json } = await fetcher(data);
        if (props.on_success) props.on_success(response, json);
      } catch (response: unknown) {
        if (response instanceof Response && props.on_fail)
          props.on_fail(response);
        else throw response;
      }
    },
    [props, fetcher]
  );

  return (
    <RawForm on_submit={on_submit} form_type={z.any()} style={props.style}>
      <View style={props.style}>{props.children}</View>
    </RawForm>
  );
}
