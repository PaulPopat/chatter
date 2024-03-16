import { PropsWithChildren, useEffect } from "react";
import { UseForm } from "./form";

type Props = {
  name: string;
  value: string;
};

export default (props: PropsWithChildren<Props>) => {
  const { value, set_value } = UseForm(props.name);

  useEffect(() => {
    set_value(props.value);
  }, [props.value]);

  return <></>;
};
