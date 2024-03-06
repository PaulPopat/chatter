import { Text, View } from "react-native";

export default (props: { url: string }) => {
  return (
    <View>
      <Text>{props.url}</Text>
    </View>
  );
};
