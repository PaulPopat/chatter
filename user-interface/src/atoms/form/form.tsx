import { PropsWithChildren, useCallback } from "react";
import { z } from "zod";
import { Fetcher } from "../../utils/fetch";
import RawForm from "./raw-form";
import { Class } from "../../styles/theme";

type FormProps<TExpect, TBody> = {
  fetcher: Fetcher<TExpect, TBody>;
  classes?: Array<Class>;
  on_submit?: () => void;
};

export default function Form<TExpect, TBody>(
  props: PropsWithChildren<FormProps<TExpect, TBody>>
) {
  const on_submit = useCallback(
    async (data: any) => {
      try {
        await props.fetcher(data);
      } catch (response: unknown) {
        if (response instanceof Response) console.error(response);
        else throw response;
      }

      props.on_submit && props.on_submit();
    },
    [props]
  );

  return (
    <RawForm on_submit={on_submit} form_type={z.any()} classes={props.classes}>
      {props.children}
    </RawForm>
  );
}
