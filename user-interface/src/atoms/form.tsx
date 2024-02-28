import {
  PropsWithChildren,
  createContext,
  useCallback,
  useContext,
  useEffect,
  useState,
} from "react";
import Url from "../utils/url";
import { z } from "zod";
import { UseSso } from "../auth/sso";
import { SSO_BASE } from "../constants";

type FormProps = {
  url: string;
  method: "GET" | "PUT" | "POST" | "DELETE";
  area: "server" | "sso";
  no_auth?: boolean;

  on_success?: (response: Response, data: unknown) => void;
  on_fail?: (response: Response) => void;
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

export default (props: PropsWithChildren<FormProps>) => {
  const [data, set_data] = useState<Record<string, unknown>>({});
  const [callbacks, set_callbacks] = useState<Array<() => void>>([]);

  const token = props.area === "sso" ? UseSso().admin_token : "";
  const base = props.area === "sso" ? SSO_BASE : "";

  const on_submit = useCallback(async () => {
    const is_body_type = BodyTypes.includes(props.method);
    const uri = new Url(props.url, !is_body_type ? UrlInput.parse(data) : {});

    const headers: Record<string, string> = {};

    if (!props.no_auth) headers["Authorization"] = `Bearer ${token}`;
    if (is_body_type) headers["Content-Type"] = "application/json";

    const response = await fetch(uri.href(base), {
      body: is_body_type ? JSON.stringify(data) : undefined,
      headers: headers,
    });

    if (!response.ok)
      if (props.on_fail) props.on_fail(response);
      else throw response;

    const json = await response.json();

    if (props.on_success) props.on_success(response, json);
  }, [data, callbacks, props]);

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
      {props.children}
    </FormContext.Provider>
  );
};
