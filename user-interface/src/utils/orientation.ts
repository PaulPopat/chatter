import { useEffect, useState } from "react";
import { Dimensions } from "react-native";

type Orientation = "landscape" | "portrait";

function GetOrientation(): Orientation {
  return Dimensions.get("window").width < Dimensions.get("window").height
    ? "portrait"
    : "landscape";
}

export default function UseOrientation() {
  const [orientation, set_orientation] = useState<Orientation>(
    GetOrientation()
  );

  useEffect(() => {
    const listener = () => set_orientation(GetOrientation());
    Dimensions.addEventListener("change", listener);
  });

  return orientation;
}
