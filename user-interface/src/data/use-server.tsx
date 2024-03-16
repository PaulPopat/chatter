import {
  PropsWithChildren,
  createContext,
  useContext,
  useEffect,
  useState,
} from "react";
import { Server } from "../auth/server";
import UseSso from "./use-sso";
import { Text } from "react-native";
import Loading from "../atoms/loading";

const ServerContext = createContext<Server>(null as any);

export const ServerProvider = (props: PropsWithChildren<{ url: string }>) => {
  const [server, set_server] = useState<Server | undefined>();
  const sso = UseSso();

  useEffect(() => {
    if (!server) {
      Server.ForServer(props.url, sso).then(set_server);
      return;
    }

    const interval = setInterval(() => {
      if (server.IsExpired) server.AsRefreshed(sso).then(set_server);
    }, 10000);

    return () => {
      clearInterval(interval);
    };
  }, [server, sso]);

  if (!server) {
    return <Loading />;
  }

  return (
    <ServerContext.Provider value={server}>
      {props.children}
    </ServerContext.Provider>
  );
};

export default function UseServer() {
  return useContext(ServerContext);
}
