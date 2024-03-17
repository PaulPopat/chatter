import { Image } from "react-native";
import { BorderRadiusLarge, Class, Classes } from "../styles/theme";
import { PropsWithChildren, useEffect, useState } from "react";
import IAsset from "../utils/asset";
import DataAsset from "../utils/data-asset";

type Props = {
  src: IAsset;
  size: number;
  classes?: Array<Class>;
};

export default (props: PropsWithChildren<Props>) => {
  const [fallback, set_fallback] = useState(false);

  useEffect(() => {
    set_fallback(false);
  }, [props.src.Uri]);

  return (
    <Image
      style={{
        ...Classes(...(props.classes ?? [])),
        borderRadius: BorderRadiusLarge,
      }}
      source={{
        uri: fallback ? DataAsset.Default.Uri : props.src.Uri,
        width: props.size,
        height: props.size,
      }}
      onError={() => set_fallback(true)}
    />
  );
};
