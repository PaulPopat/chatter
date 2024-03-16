import { z } from "zod";
import Url from "./url";
import { useCallback } from "react";
import Json from "./json";
import { SSO_BASE } from "@effuse/config";
import UseSso from "../data/use-sso";
import UseServer from "../data/use-server";

const BodyTypes = ["PUT", "POST"];

type Mapper<TBody, TFinalBody> = (
  body: TBody
) => TFinalBody | Promise<TFinalBody>;

type FetchBase<TExpect, TBody = Record<string, unknown>> = {
  expect?: z.ZodType<TExpect>;
  cachable?: boolean;
  headers?: Record<string, string>;
  mapper?: Mapper<TBody, unknown>;
  body_type?: z.ZodType<TBody>;
  method: string;
};

export type FetchConfig<TExpect, TBody = Record<string, unknown>> = FetchBase<
  TExpect,
  TBody
> &
  (
    | {
        area: "sso";
      }
    | {
        area: "server";
        base_url: string;
      }
  );

export type FetchResponse<TExpect> = {
  response: Response;
  data: TExpect;
};

export type Fetcher<TExpect, TBody = Record<string, unknown>> = (
  body: TBody
) => Promise<FetchResponse<TExpect>>;

export type FetcherOf<TBase> = TBase extends FetchBase<
  infer Output,
  infer Input
>
  ? Fetcher<Output, Input>
  : unknown;

export async function Fetch<TExpect = unknown, TBody = Record<string, unknown>>(
  url: string,
  props: FetchConfig<TExpect, TBody>,
  body: TBody,
  token?: string
): Promise<FetchResponse<TExpect>> {
  if (props.body_type) body = props.body_type.parse(body);
  body = props.mapper
    ? await Promise.resolve(props.mapper(body))
    : (body as any);
  const base = props.area === "sso" ? SSO_BASE : props.base_url;

  const is_body_type = BodyTypes.includes(props?.method ?? "GET");
  const uri = new Url(url, body as any, !is_body_type);

  const headers: Record<string, string> = {};

  if (token) headers["Authorization"] = `Bearer ${token}`;
  const response = await fetch(uri.href(base), {
    ...props,
    body: is_body_type ? Json.ToString(body) : undefined,
    headers: {
      ...(props.headers ?? {}),
      ...headers,
    },
  });

  if (!response.ok) throw response;
  const json = Json.Parse(await response.text());

  if (props?.expect)
    return {
      response,
      data: props.expect.parse(json),
    };

  return {
    response,
    data: json as TExpect,
  };
}

export type UseFetcherConfig<
  TExpect,
  TBody = Record<string, unknown>
> = FetchBase<TExpect, TBody> & {
  no_auth?: boolean;
  area: "sso" | "server";

  on_success?: (response: Response, data: TExpect) => void;
  on_fail?: (response: Response) => void;
};

export default function UseFetcher<
  TExpect = unknown,
  TBody = Record<string, unknown>
>(
  url: string,
  props: UseFetcherConfig<TExpect, TBody>
): Fetcher<TExpect, TBody> {
  const server = UseServer();
  const token = props.area === "sso" ? UseSso().AdminToken : server.LocalToken;

  return useCallback(
    (body: TBody) =>
      Fetch(
        url,
        props.area === "sso"
          ? { ...props, area: "sso" }
          : { ...props, area: "server", base_url: server.BaseUrl },
        body,
        !props?.no_auth ? token : undefined
      )
        .then((r) => {
          if (props.on_success) props.on_success(r.response, r.data);
          return r;
        })
        .catch((response) => {
          if (props.on_fail) props.on_fail(response);
          throw response;
        }),
    [url, props, token]
  );
}
