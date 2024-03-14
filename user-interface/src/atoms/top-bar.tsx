import { PropsWithChildren } from "react";
import { Pressable, View, Text, StyleSheet } from "react-native";
import Icon from "./icon";
import { Padding, Colours, BorderRadius } from "../styles/theme";
import UseOrientation from "../utils/orientation";

const styles = StyleSheet.create({
  server_manage: {
    padding: Padding,
    marginVertical: Padding,
    flexDirection: "row",
    alignItems: "center",
    borderBottomWidth: 2,
    borderBottomColor: Colours.Body.Foreground,
  },
  server_manage_text: {
    padding: Padding,
    flex: 1,
  },
});

type Props = {
  click: () => void;
  title: string | undefined;
};

export default (props: PropsWithChildren<Props>) => {
  const orientation = UseOrientation();
  return (
    <View style={styles.server_manage}>
      {orientation === "portrait" && (
        <Pressable onPress={props.click}>
          <Icon area="Arrows" icon="arrow-left" />
        </Pressable>
      )}
      <Text style={styles.server_manage_text}>{props.title}</Text>
      {props.children}
    </View>
  );
};
