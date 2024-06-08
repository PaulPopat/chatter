import React, {
  PropsWithChildren,
  createContext,
  useContext,
  useState,
} from "react";
import { Auth, Sso } from "../auth/sso";
import UseFetcher, { Fetcher } from "../utils/fetch";
import { Session } from "../utils/system/storage";

const SsoContext = createContext(Sso.Stored);

const SsoControlContext = createContext<{
  refresh: Fetcher<unknown>;
  login: Fetcher<unknown>;
  register: Fetcher<unknown>;
  clear: () => void;
}>(null as any);

export default function UseSso() {
  return useContext(SsoContext);
}

export function UseSsoControls() {
  return useContext(SsoControlContext);
}

export const SsoProvider = (props: PropsWithChildren) => {
  const [auth, set_auth] = useState(Sso.Stored);

  const refresh = UseFetcher("/api/v1/auth/refresh-token", {
    method: "GET",
    expect: Auth,
    area: "sso",
    on_success(_, d) {
      set_auth(new Sso(d));
      Session.auth = d;
    },
    on_fail(response) {
      set_auth(Sso.Empty);
      delete Session.auth;
    },
  });

  const login = UseFetcher("/api/v1/auth/token", {
    area: "sso",
    method: "GET",
    expect: Auth,
    on_success(_, d) {
      set_auth(new Sso(d));
      Session.auth = d;
    },
  });

  const register = UseFetcher("/api/v1/users", {
    method: "POST",
    area: "sso",
    expect: Auth,
    on_success(_, d) {
      set_auth(new Sso(d));
      Session.auth = d;
    },
  });

  const value = React.useMemo(
    () => ({
      refresh,
      login,
      register,
      clear() {
        set_auth(Sso.Empty);
      },
    }),
    [refresh, login, register]
  );

  return (
    <SsoControlContext.Provider value={value}>
      <SsoContext.Provider value={auth}>{props.children}</SsoContext.Provider>
    </SsoControlContext.Provider>
  );
};
