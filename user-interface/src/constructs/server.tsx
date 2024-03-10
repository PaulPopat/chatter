import { Text, View } from "react-native";
import UseChannels from "../data/use-channels";

export default () => {
  const {
    state: channels,
    actions: { create_channel },
  } = UseChannels();

  return (
    <View>
      <Text>{JSON.stringify(channels)}</Text>
    </View>
  );
};
