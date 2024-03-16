import { Text, View } from "react-native";
import { Channel } from "../data/use-channels";
import ChatChannel from "./chat-channel";
import TopBar from "../atoms/top-bar";
import { Classes } from "../styles/theme";

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
    <View style={Classes("fill")}>
      <TopBar click={props.blur} title={props.channel.Name}></TopBar>
      <View style={Classes("flex_fill")}>
        <SubChannel {...props} />
      </View>
    </View>
  );
};
