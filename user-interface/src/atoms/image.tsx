import { Image } from "react-native";
import { BorderRadiusLarge } from "../styles/theme";
import DataUrl from "../utils/data-url";
import { PropsWithChildren } from "react";

type Props = {
  src: DataUrl;
  size: number;
};

export default (props: PropsWithChildren<Props>) => (
  <Image
    style={{
      borderRadius: BorderRadiusLarge,
    }}
    source={{
      uri: props.src.Uri,
      width: props.size,
      height: props.size,
    }}
  />
);
