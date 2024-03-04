import {
  PropsWithChildren,
  createContext,
  useContext,
  useEffect,
  useState,
} from "react";
import { z } from "zod";
import Form from "../atoms/form";
import Textbox from "../atoms/textbox";
import Submitter from "../atoms/submitter";
import { Session } from "../utils/storage";
import { Fetch } from "../utils/fetch";

const Auth = z.object({
  AdminToken: z.string(),
  ServerToken: z.string(),
  UserId: z.string(),
  RefreshToken: z.string(),
  Expires: z.date(),
});

const EmptyAuth = {
  AdminToken: "",
  ServerToken: "",
  UserId: "",
  RefreshToken: "",
  Expires: new Date(),
};

export class Sso {
  readonly #admin_token: string;
  readonly #server_token: string;
  readonly #user_id: string;
  readonly #refresh_token: string;
  readonly #expires: Date;

  constructor(dto: z.infer<typeof Auth>) {
    this.#admin_token = dto.AdminToken;
    this.#server_token = dto.ServerToken;
    this.#user_id = dto.UserId;
    this.#refresh_token = dto.RefreshToken;
    this.#expires = new Date(dto.Expires);
  }

  get #expires_in_seconds() {
    return (new Date().getTime() - this.#expires.getTime()) / 1000;
  }

  get ShouldRefresh() {
    return this.#expires_in_seconds <= 60;
  }

  get IsExpired() {
    return this.#expires_in_seconds <= 0;
  }

  async AsRefreshed() {
    const { data } = await Fetch(
      "/api/v1/auth/refresh-token",
      {
        method: "GET",
        expect: Auth,
      },
      {
        token: this.#refresh_token,
      }
    );
    return new Sso(data);
  }

  get AdminToken() {
    return this.#admin_token;
  }

  get ServerToken() {
    return this.#server_token;
  }
}

function GetAuthStorage() {
  return new Sso(Auth.parse(Session.auth ?? EmptyAuth));
}

const AuthContext = createContext<Sso>(GetAuthStorage());

export function UseSso() {
  return useContext(AuthContext);
}

export const SsoProvider = (props: PropsWithChildren) => {
  const [auth, set_auth] = useState<Sso>(GetAuthStorage());

  useEffect(() => {
    const interval = setInterval(() => {
      if (auth.IsExpired) auth.AsRefreshed().then(set_auth);
    }, 10000);

    return () => {
      clearInterval(interval);
    };
  }, [auth]);

  if (!auth.AdminToken) {
    return (
      <Form
        area="sso"
        method="GET"
        url="/api/v1/auth/token"
        on_success={(_, d) => {
          const input = Auth.parse(d);
          set_auth(new Sso(input));
          Session.auth = input;
        }}
      >
        <Textbox name="email" keyboard="email-address">
          Email
        </Textbox>
        <Textbox name="password" password>
          Password
        </Textbox>
        <Submitter>Log In</Submitter>
      </Form>
    );
  }

  return (
    <AuthContext.Provider value={auth}>{props.children}</AuthContext.Provider>
  );
};
