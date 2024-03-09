import { z } from "zod";
import Url from "./url";
import { useCallback } from "react";
import Json from "./json";
import { SSO_BASE } from "@effuse/config";
import UseSso from "../data/use-sso";
import UseServer from "../data/use-server";

const BodyTypes = ["PUT", "POST"];

type Mapper = (
  body: Record<string, unknown>
) => Record<string, unknown> | Promise<Record<string, unknown>>;

export type FetchConfig<TExpect> = {
  expect?: z.ZodType<TExpect>;
  cachable?: boolean;
  method: string;
  headers?: Record<string, string>;
  mapper?: Mapper;
} & (
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

export type Fetcher<TExpect> = (
  body: Record<string, unknown>
) => Promise<FetchResponse<TExpect>>;

export async function Fetch<TExpect = unknown>(
  url: string,
  props: FetchConfig<TExpect>,
  body: Record<string, unknown>,
  token?: string
): Promise<FetchResponse<TExpect>> {
  body = props.mapper ? await Promise.resolve(props.mapper(body)) : body;
  const base = props.area === "sso" ? SSO_BASE : props.base_url;

  const is_body_type = BodyTypes.includes(props?.method ?? "GET");
  const uri = new Url(url, body, !is_body_type);

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

export type UseFetcherConfig<TExpect = unknown> = {
  no_auth?: boolean;
  expect?: z.ZodType<TExpect>;
  cachable?: boolean;
  method: string;
  headers?: Record<string, string>;
  area: "sso" | "server";
  mapper?: Mapper;

  on_success?: (response: Response, data: TExpect) => void;
  on_fail?: (response: Response) => void;
};

export default function UseFetcher<TExpect = unknown>(
  url: string,
  props: UseFetcherConfig<TExpect>
): Fetcher<TExpect> {
  const server = UseServer();
  const token = props.area === "sso" ? UseSso().AdminToken : server.LocalToken;

  return useCallback(
    (body: Record<string, unknown>) =>
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
