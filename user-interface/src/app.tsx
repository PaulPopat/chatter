import { PropsWithChildren } from "react";
import { Text, View } from "react-native";
import { SsoProvider } from "./auth/sso";

export default (props: PropsWithChildren) => {
  return (
    <View>
      <SsoProvider>
        <Text>Hello world</Text>
      </SsoProvider>
    </View>
  );
};
