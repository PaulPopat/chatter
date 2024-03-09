import { useEffect, useState } from "react";
import { View } from "react-native";
import { AuthContext, Sso } from "./auth/sso";
import { Colours, Padding } from "./styles/theme";
import ServerList from "./constructs/server-list";
import Authenticate from "./constructs/authenticate";
import Server from "./constructs/server";
import { ServerAuthProvider } from "./auth/server";

export default () => {
  const [auth, set_auth] = useState(Sso.Stored);
  const [open, set_open] = useState("");

  useEffect(() => {
    const interval = setInterval(() => {
      if (auth.IsExpired) auth.AsRefreshed().then(set_auth);
    }, 10000);

    return () => {
      clearInterval(interval);
    };
  }, [auth]);

  if (!auth.AdminToken) {
    return <Authenticate set_auth={set_auth} />;
  }

  return (
    <AuthContext.Provider value={auth}>
      <View
        style={{
          flexDirection: "row",
          minHeight: "100%",
        }}
      >
        <View
          style={{
            maxWidth: 200,
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
            <ServerAuthProvider url={open}>
              <Server />
            </ServerAuthProvider>
          )}
        </View>
      </View>
    </AuthContext.Provider>
  );
};
