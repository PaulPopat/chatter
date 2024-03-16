import { View } from "react-native";
import Icon from "./icon";
import { Classes } from "../styles/theme";

export default () => (
  <View style={Classes("centre", "fill_all")}>
    <Icon area="System" icon="loader-2" size={64} />
  </View>
);
