import { createContext, useContext, useEffect } from "react";

export type InputValidation =
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

export const FormContext = createContext<FormContext>({
  set_value(name: string, value: unknown) {
    return { type: "ok" };
  },
  get_value(name) {},
  on_submit(handler) {},
  off_submit(handler) {},
  submit() {},
});

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
