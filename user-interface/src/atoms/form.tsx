import {
  PropsWithChildren,
  createContext,
  useCallback,
  useContext,
  useEffect,
  useState,
} from "react";
import { z } from "zod";
import UseFetcher from "../utils/fetch";
import { StyleProp, View, ViewStyle } from "react-native";

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

type InputValidation =
  | {
      type: "ok";
    }
  | {
      type: "invalid";
      message: string;
    };

type FormContext = {
  set_value(name: string, value: unknown): InputValidation;
  get_value(name: string): unknown;
  on_submit(handler: () => void): void;
  off_submit(handler: () => void): void;
  submit(): void;
};

const FormContext = createContext<FormContext>({
  set_value(name: string, value: unknown) {
    return { type: "ok" };
  },
  get_value(name) {},
  on_submit(handler) {},
  off_submit(handler) {},
  submit() {},
});

const BodyTypes = ["PUT", "POST"];

const UrlInput = z.record(z.string());

export function UseForm(name: string) {
  const data = useContext(FormContext);

  return {
    value: data.get_value(name),
    set_value: (value: unknown) => data.set_value(name, value),
    use_submit: (effect: () => void, effectors: Array<any>) => {
      useEffect(() => {
        data.on_submit(effect);
        return () => {
          data.off_submit(effect);
        };
      }, effectors);
    },
    submit: data.submit,
  };
}

export function UseSubmitter() {
  const data = useContext(FormContext);
  return data.submit;
}

export default function Form<TExpect>(
  props: PropsWithChildren<FormProps<TExpect>>
) {
  const [data, set_data] = useState<Record<string, unknown>>({});
  const [callbacks, set_callbacks] = useState<Array<() => void>>([]);
  const fetcher = UseFetcher(props.url, {
    method: props.method,
    area: props.area,
    no_auth: props.no_auth,
    expect: props.expect,
  });

  const on_submit = useCallback(async () => {
    try {
      const { response, data: json } = await fetcher(data);
      if (props.on_success) props.on_success(response, json);
    } catch (response: unknown) {
      if (response instanceof Response && props.on_fail)
        props.on_fail(response);
      else throw response;
    }
  }, [data, callbacks, props, fetcher]);

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
