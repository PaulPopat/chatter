import { BorderRadiusLarge, Class } from "../styles/theme";
import { PropsWithChildren, useEffect, useState } from "react";
import IAsset from "../utils/asset";
import DataAsset from "../utils/data-asset";
import { Image } from "./native";

type Props = {
  src: IAsset;
  size: number;
  class?: Class;
};

export default (props: PropsWithChildren<Props>) => {
  const [fallback, set_fallback] = useState(false);

  useEffect(() => {
    set_fallback(false);
  }, [props.src.Uri]);

  return (
    <Image
      class={props.class}
      style={{ borderRadius: BorderRadiusLarge }}
      source={{
        uri: fallback ? DataAsset.Default.Uri : props.src.Uri,
        width: props.size,
        height: props.size,
      }}
      onError={() => set_fallback(true)}
    />
  );
};
