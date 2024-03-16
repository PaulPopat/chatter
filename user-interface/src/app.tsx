import { useEffect, useState } from "react";
import { View, StyleSheet, Text } from "react-native";
import { Classes, Colours, Padding } from "./styles/theme";
import ServerList from "./constructs/server-list";
import Authenticate from "./constructs/authenticate";
import Server from "./constructs/server";
import UseSso, { SsoProvider, UseSsoControls } from "./data/use-sso";
import { ServerProvider } from "./data/use-server";
import useProfile from "./data/use-profile";
import UseOrientation from "./utils/orientation";

const MainPanel = () => {
  const profile = useProfile();
  const [open, set_open] = useState("");
  const orientation = UseOrientation();

  return (
    <View style={Classes("row", "fill", "no_overflow", "no_gap")}>
      <View
        style={{
          ...Classes("highlight", "border_right", "fill"),
          ...(orientation === "landscape"
            ? {
                width: 90,
              }
            : {
                width: "100%",
                borderRightWidth: 0,
                overflow: "hidden",
              }),
        }}
      >
        <View style={Classes("fill")}>
          <ServerList on_open={set_open} profile={profile} />
        </View>
      </View>
      <View style={Classes("flex_fill", "fill")}>
        {profile.state?.Servers.map((s) => (
          <ServerProvider key={s.Url} url={s.Url}>
            <Server open={open === s.Url} blur={() => set_open("")} />
          </ServerProvider>
        ))}
      </View>
    </View>
  );
};

const Main = () => {
  const auth = UseSso();
  const { refresh } = UseSsoControls();

  useEffect(() => {
    const interval = setInterval(() => {
      if (auth.AdminToken && auth.IsExpired)
        refresh({ token: auth.RefreshToken });
    }, 10000);

    return () => {
      clearInterval(interval);
    };
  }, [auth]);

  if (auth.AdminToken && auth.IsExpired) return <Text>Loading</Text>;
  if (!auth.AdminToken) return <Authenticate />;

  return <MainPanel />;
};

export default () => {
  return (
    <SsoProvider>
      <Main />
    </SsoProvider>
  );
};
