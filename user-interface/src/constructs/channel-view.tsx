import { Text } from "react-native";
import { Channel } from "../data/use-channels";
import ChatChannel from "./chat-channel";

export default (props: { channel: Channel }) => {
  switch (props.channel.Type) {
    case "Messages":
      return <ChatChannel channel_id={props.channel.ChannelId} />;
    default:
      return <Text>Unknown Channel Type</Text>;
  }
};
