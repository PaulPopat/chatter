import { useCallback, useEffect, useMemo, useState } from "react";
import UseFetcher, { FetcherOf, UseFetcherConfig } from "./fetch";
import { Session } from "./storage";

type RemoteStateFetchers = Record<string, [string, UseFetcherConfig<any, any>]>;

type Actions<TActions extends RemoteStateFetchers> = {
  [TKey in keyof TActions]: FetcherOf<TActions[TKey][1]>;
};

export default function UseRemoteState<
  TExpect,
  TBody,
  TActions extends RemoteStateFetchers
>(url: string, props: UseFetcherConfig<TExpect, TBody>, actions: TActions) {
  return (body: TBody = {} as any) => {
    const fetcher = UseFetcher(url, props);

    const [state, set_data] = useState<TExpect>();

    const update = useCallback(
      () =>
        fetcher(body)
          .then(({ data, response }) => {
            set_data(data);
            Session[response.url] = data;
          })
          .catch((err) => {
            if (err instanceof Response) {
              const d = Session[err.url];
              set_data(props.expect?.parse(d) ?? (d as any));
            } else {
              throw err;
            }
          }),
      [fetcher, body]
    );

    const compiled_actions = Object.keys(actions)
      .map((k) => [k, actions[k]] as const)
      .reduce(
        (c, [k, [url, config]]) => ({
          ...c,
          [k as keyof TActions]: UseFetcher(url, {
            ...config,
            on_success(response, data) {
              config.on_success && config.on_success(response, data);
              update();
            },
          }),
        }),
        {} as Actions<TActions>
      );

    useEffect(
      () => {
        update();
      },
      Object.keys(body as any).map((k) => (body as any)[k])
    );

    return {
      state,
      actions: compiled_actions,
    };
  };
}
