import { PropsWithChildren } from "react";
import { UseForm } from "./form";
import { KeyboardTypeOptions, Text, Pressable, View } from "react-native";
import Icon from "./icon";
import { Classes } from "../styles/theme";
import FilePicker from "../utils/file-picker";
import IAsset from "../utils/asset";
import Image from "./image";

type Props = {
  name: string;
  keyboard?: KeyboardTypeOptions;
  default: IAsset;
  password?: boolean;
  clear_on_submit?: boolean;
};

export default (props: PropsWithChildren<Props>) => {
  const { value, set_value, use_submit } = UseForm(props.name);

  use_submit(() => {
    if (props.clear_on_submit) set_value(null);
  }, [props.clear_on_submit]);

  return (
    <Pressable
      style={Classes("container", "column", "card", "centre")}
      onPress={() => FilePicker("image/*").then(set_value)}
    >
      <View style={Classes("row")}>
        <Icon area="System" icon="upload-cloud-2" />
        <Text style={Classes("body_text")}>{props.children}</Text>
      </View>

      <Image src={(value as IAsset) ?? props.default} size={64} />
    </Pressable>
  );
};
