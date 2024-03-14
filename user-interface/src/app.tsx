import { useEffect, useState } from "react";
import { View, StyleSheet, Text } from "react-native";
import { Colours, Padding } from "./styles/theme";
import ServerList from "./constructs/server-list";
import Authenticate from "./constructs/authenticate";
import Server from "./constructs/server";
import UseSso, { SsoProvider, UseSsoControls } from "./data/use-sso";
import { ServerProvider } from "./data/use-server";
import useProfile from "./data/use-profile";

const styles = StyleSheet.create({
  app: {
    flexDirection: "row",
    height: "100%",
  },
  server_list: {
    width: 80,
    backgroundColor: Colours.Highlight.Background,
    padding: Padding,
    borderRightColor: Colours.Highlight.Foreground,
    borderRightWidth: 2,
    height: "100%",
  },
  main_panel: {
    flex: 1,
  },
});

const MainPanel = () => {
  const profile = useProfile();
  const [open, set_open] = useState("");

  return (
    <View style={styles.app}>
      <View style={styles.server_list}>
        <ServerList on_open={set_open} profile={profile} />
      </View>
      <View style={styles.main_panel}>
        {profile.state?.Servers.map((s) => (
          <ServerProvider key={s.Url} url={s.Url}>
            <Server open={open === s.Url} />
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
