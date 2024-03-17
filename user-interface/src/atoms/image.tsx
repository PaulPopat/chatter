import { Image } from "react-native";
import { BorderRadiusLarge, Class, Classes } from "../styles/theme";
import { PropsWithChildren, useEffect, useState } from "react";
import IResource from "../utils/i-resource";
import DataUrl from "../utils/data-url";

type Props = {
  src: IResource;
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
        uri: fallback ? DataUrl.Default.Uri : props.src.Uri,
        width: props.size,
        height: props.size,
      }}
      onError={() => set_fallback(true)}
    />
  );
};
