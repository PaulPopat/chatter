import { PropsWithChildren, createContext, useContext, useState } from "react";
import { z } from "zod";
import Form from "../atoms/form";
import Textbox from "../atoms/textbox";
import Submitter from "../atoms/submitter";

const AuthResponse = z.object({
  AdminToken: z.string(),
  ServerToken: z.string(),
  UserId: z.string(),
});

const Auth = z.object({
  admin_token: z.string(),
  server_token: z.string(),
  user_id: z.string(),
});

export type Sso = z.infer<typeof Auth>;

function GetAuthStorage() {
  return JSON.parse(
    sessionStorage.getItem("auth") ??
      JSON.stringify({
        admin_token: "",
        server_token: "",
        user_id: "",
      })
  );
}

const AuthContext = createContext<Sso>(GetAuthStorage());

export function UseSso() {
  return useContext(AuthContext);
}

export const SsoProvider = (props: PropsWithChildren) => {
  const [auth, set_auth] = useState<Sso>(GetAuthStorage());

  if (!auth) {
    return (
      <Form
        area="sso"
        method="GET"
        url="/api/v1/auth/token"
        on_success={(_, d) => {
          const data = AuthResponse.parse(d);
          const input = {
            admin_token: data.AdminToken,
            server_token: data.ServerToken,
            user_id: data.UserId,
          };

          set_auth(input);
          sessionStorage.setItem("auth", JSON.stringify(input));
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
