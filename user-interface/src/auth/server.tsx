import { z } from "zod";
import { Fetch } from "../utils/fetch";
import { Session } from "../utils/storage";
import {
  PropsWithChildren,
  createContext,
  useContext,
  useEffect,
  useState,
} from "react";
import { addHours } from "date-fns";
import { Sso, UseSso } from "./sso";

export const Auth = z.object({
  LocalToken: z.string(),
});

const EmptyAuth = {
  AdminToken: "",
  ServerToken: "",
  UserId: "",
  RefreshToken: "",
  Expires: new Date(),
};

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

const ServerAuthContext = createContext<Server>(null as any);

export function UseServerAuth() {
  return useContext(ServerAuthContext);
}

export const ServerAuthProvider = (
  props: PropsWithChildren<{ url: string }>
) => {
  const [server, set_server] = useState<Server | undefined>();
  const sso = UseSso();

  useEffect(() => {
    if (!server) return;

    const interval = setInterval(() => {
      if (server.IsExpired) server.AsRefreshed(sso).then(set_server);
    }, 10000);

    return () => {
      clearInterval(interval);
    };
  }, [server, sso]);

  if (!server) throw Server.ForServer(props.url, sso).then(set_server);

  return (
    <ServerAuthContext.Provider value={server}>
      {props.children}
    </ServerAuthContext.Provider>
  );
};
