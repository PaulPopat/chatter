import { Image } from "react-native";
import { BorderRadiusLarge } from "../styles/theme";
import { PropsWithChildren } from "react";
import IResource from "../utils/i-resource";

type Props = {
  src: IResource;
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
