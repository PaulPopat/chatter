import { useEffect, useState } from "react";
import { ClearArgs, GetArgs } from "../utils/system/args";

let args_listeners: Array<() => void> = [];

export default function UseArgs() {
  const [args, set_args] = useState(GetArgs());

  useEffect(() => {
    const handler = () => set_args(GetArgs());
    args_listeners = [...args_listeners, handler];

    return () => {
      args_listeners = args_listeners.filter((a) => a !== handler);
    };
  }, []);

  return {
    state: args,
    actions: {
      clear_args: () => {
        ClearArgs();
        for (const listener of args_listeners) listener();
      },
    },
  };
}
