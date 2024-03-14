import { Text, View, StyleSheet } from "react-native";
import { Channel } from "../data/use-channels";
import ChatChannel from "./chat-channel";
import TopBar from "../atoms/top-bar";

const styles = StyleSheet.create({
  container: {
    height: "100%",
    flexDirection: "column",
  },
  content: {
    flex: 1,
  },
});

const SubChannel = (props: { channel: Channel }) => {
  switch (props.channel.Type) {
    case "Messages":
      return <ChatChannel channel_id={props.channel.ChannelId} />;
    default:
      return <Text>Unknown Channel Type</Text>;
  }
};

export default (props: { channel: Channel; blur: () => void }) => {
  return (
    <View style={styles.container}>
      <TopBar click={props.blur} title={props.channel.Name}></TopBar>
      <View style={styles.content}>
        <SubChannel {...props} />
      </View>
    </View>
  );
};
