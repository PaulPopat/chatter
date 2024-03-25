import { SystemMatch } from "./utils";

export const GetArgs = SystemMatch({
  web: () => () => {
    const query = new URLSearchParams(window.location.search);

    const result: Record<string, string> = {};
    for (const [key, value] of query.entries()) result[key] = value;

    return result;
  },
});

export const ClearArgs = SystemMatch({
  web: () => () => {
    history.pushState(undefined, "", window.location.pathname);
  },
});
