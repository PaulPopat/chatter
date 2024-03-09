import { useCallback, useEffect, useState } from "react";
import UseFetcher, { FetchConfig, UseFetcherConfig } from "./fetch";
import { Session } from "./storage";

export default function UseRemoteState<TExpect>(
  url: string,
  props: UseFetcherConfig<TExpect>
) {
  const fetcher = UseFetcher(url, props);

  const [data, set_data] = useState<TExpect>();

  const update = useCallback(
    () =>
      fetcher({})
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
    [fetcher]
  );

  useEffect(() => {
    update();
  }, []);

  return [data, update] as const;
}
