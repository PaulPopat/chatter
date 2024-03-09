import { z } from "zod";
import { Fetch } from "../utils/fetch";
import {
  PropsWithChildren,
  createContext,
  useContext,
  useEffect,
  useState,
} from "react";
import { addHours } from "date-fns";
import { Sso } from "./sso";
import UseSso from "../data/use-sso";

export const Auth = z.object({
  LocalToken: z.string(),
});

export class Server {
  readonly #server_url: string;
  readonly #local_token: string;
  readonly #expires: Date;

  constructor(dto: z.infer<typeof Auth>, server_url: string) {
    this.#server_url = server_url;
    this.#local_token = dto.LocalToken;
    this.#expires = addHours(new Date(), 12);
  }

  get #expires_in_seconds() {
    return (this.#expires.getTime() - new Date().getTime()) / 1000;
  }

  get ShouldRefresh() {
    return this.#expires_in_seconds <= 60;
  }

  get IsExpired() {
    return this.#expires_in_seconds <= 0;
  }

  async AsRefreshed(sso: Sso) {
    return Server.ForServer(this.#server_url, sso);
  }

  get LocalToken() {
    return this.#local_token;
  }

  get BaseUrl() {
    return this.#server_url;
  }

  static async ForServer(url: string, sso: Sso) {
    const { data } = await Fetch(
      "/api/v1/auth/token",
      {
        method: "GET",
        expect: Auth,
        area: "server",
        base_url: url,
      },
      {
        token: sso.ServerToken,
      }
    );

    return new Server(data, url);
  }
}
