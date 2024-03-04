import { z } from "zod";
import Url from "./url";
import { UseSso } from "../auth/sso";
import { useCallback } from "react";
import Json from "./json";
import { SSO_BASE } from "@effuse/config";

const BodyTypes = ["PUT", "POST"];

export type Area = "server" | "sso";

export type FetchConfig<TExpect> = Omit<RequestInit, "body"> & {
  expect?: z.ZodType<TExpect>;
  area?: Area;
  no_auth?: boolean;
};

export type FetchResponse<TExpect> = {
  response: Response;
  data: TExpect;
};

export type Fetcher<TExpect> = (
  body: Record<string, unknown>
) => Promise<FetchResponse<TExpect>>;

export async function Fetch<TExpect = unknown>(
  url: string,
  props: Omit<FetchConfig<TExpect>, "no_auth">,
  body: Record<string, unknown>,
  token?: string
): Promise<FetchResponse<TExpect>> {
  const base = props?.area === "sso" ? SSO_BASE : "";

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

export default function UseFetcher<TExpect = unknown>(
  url: string,
  props?: FetchConfig<TExpect>
): Fetcher<TExpect> {
  const token = props?.area === "sso" ? UseSso().AdminToken : "";

  return useCallback(
    (body: Record<string, unknown>) =>
      Fetch(url, props ?? {}, body, !props?.no_auth ? token : undefined),
    [url, props, token]
  );
}
