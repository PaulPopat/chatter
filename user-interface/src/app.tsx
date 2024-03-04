import { PropsWithChildren } from "react";
import { Text, View } from "react-native";
import { SsoProvider } from "./auth/sso";
import ServerList from "./constructs/server-list";
import UseOrientation from "./utils/orientation";
import { Colours, Padding } from "./styles/theme";

export default (props: PropsWithChildren) => {
  const orientation = UseOrientation();

  if (orientation === "portrait")
    return <Text>Portrait is currently not supported</Text>;

  return (
    <SsoProvider>
      <View
        style={{
          maxWidth: 200,
          minHeight: "100%",
          backgroundColor: Colours.Highlight.Background,
          padding: Padding,
        }}
      >
        <ServerList />
      </View>
    </SsoProvider>
  );
};
