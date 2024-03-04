import { useCallback, useEffect, useState } from "react";
import UseFetcher, { FetchConfig } from "./fetch";
import { Session } from "./storage";

export default function UseRemoteState<TExpect>(
  url: string,
  props: FetchConfig<TExpect>
) {
  const fetcher = UseFetcher(url, props);

  const [data, set_data] = useState<TExpect>();

  const update = useCallback(
    () =>
      fetcher({})
        .then(({ data }) => {
          set_data(data);
          Session[url] = data;
        })
        .catch((err) => {
          console.error(err);
          const d = Session[url];
          set_data(props.expect?.parse(d) ?? (d as any));
        }),
    [fetcher]
  );

  useEffect(() => {
    update();
  }, []);

  return [data, update] as const;
}
