import { PropsWithChildren, useCallback, useState } from "react";
import { z } from "zod";
import { Fetcher } from "../../utils/fetch";
import RawForm from "./raw-form";
import { Class } from "../../styles/theme";
import { Text } from "../native";

type FormProps<TExpect, TBody> = {
  fetcher: Fetcher<TExpect, TBody>;
  class?: Class;
  on_submit?: () => void;
  hide_notification?: boolean;
};

export default function Form<TExpect, TBody>(
  props: PropsWithChildren<FormProps<TExpect, TBody>>
) {
  const [res, set_res] = useState("unknown" as "unknown" | "success" | "error");

  const on_submit = useCallback(
    async (data: any) => {
      try {
        const { response } = await props.fetcher(data);
        if (response.status < 399) set_res("success");
        else set_res("error");
      } catch (response: unknown) {
        if (response instanceof Response) console.error(response);
        else throw response;

        set_res("error");
      }

      props.on_submit && props.on_submit();

      setTimeout(() => set_res("unknown"), 10000);
    },
    [props]
  );

  return (
    <RawForm on_submit={on_submit} form_type={z.any()} class={props.class}>
      {res === "success" && !props.hide_notification ? (
        <Text class="colour_secondary container">Success</Text>
      ) : res === "error" && !props.hide_notification ? (
        <Text class="colour_danger container">Error</Text>
      ) : (
        <></>
      )}
      {props.children}
    </RawForm>
  );
}
