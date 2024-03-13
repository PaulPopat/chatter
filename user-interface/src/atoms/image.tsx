import { View, Image, StyleSheet, Pressable } from "react-native";
import { BorderRadiusLarge } from "../styles/theme";
import DataUrl from "../utils/data-url";
import { PropsWithChildren } from "react";

const styles = StyleSheet.create({
  image: {
    borderRadius: BorderRadiusLarge,
  },
});

type Props = {
  src: DataUrl;
  size: number;
};

export default (props: PropsWithChildren<Props>) => (
  <Image
    style={styles.image}
    source={{
      uri: props.src.Uri,
      width: props.size,
      height: props.size,
    }}
  />
);
