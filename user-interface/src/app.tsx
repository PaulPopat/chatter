import { useEffect, useState } from "react";
import { View } from "react-native";
import { Colours, Padding } from "./styles/theme";
import ServerList from "./constructs/server-list";
import Authenticate from "./constructs/authenticate";
import Server from "./constructs/server";
import UseSso, { SsoProvider, UseSsoControls } from "./data/use-sso";
import { ServerProvider } from "./data/use-server";

const Main = () => {
  const auth = UseSso();
  const { refresh } = UseSsoControls();
  const [open, set_open] = useState("");

  useEffect(() => {
    const interval = setInterval(() => {
      if (auth.AdminToken && auth.IsExpired)
        refresh({ token: auth.RefreshToken });
    }, 10000);

    return () => {
      clearInterval(interval);
    };
  }, [auth]);

  if (!auth.AdminToken) {
    return <Authenticate />;
  }

  return (
    <View
      style={{
        flexDirection: "row",
        minHeight: "100%",
      }}
    >
      <View
        style={{
          width: 80,
          backgroundColor: Colours.Highlight.Background,
          padding: Padding,
        }}
      >
        <ServerList on_open={set_open} />
      </View>
      <View
        style={{
          flex: 1,
        }}
      >
        {open && (
          <ServerProvider url={open}>
            <Server />
          </ServerProvider>
        )}
      </View>
    </View>
  );
};

export default () => {
  return (
    <SsoProvider>
      <Main />
    </SsoProvider>
  );
};
