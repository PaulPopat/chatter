import { Text, View, StyleSheet } from "react-native";
import UseChannels, { Channel } from "../data/use-channels";
import { PropsWithChildren } from "react";
import Icon from "../atoms/icon";
import { Padding } from "../styles/theme";

const styles = StyleSheet.create({
  channel_container: {
    flexDirection: "row",
    alignItems: "center",
    padding: Padding,
  },
  channel_name: {
    paddingLeft: Padding,
  },
});

const ChannelView = (props: PropsWithChildren<{ channel: Channel }>) => {
  return (
    <View style={styles.channel_container}>
      <Icon area="Communication" icon="chat-3" />
      <Text style={styles.channel_name}>{props.channel.Name}</Text>
    </View>
  );
};

export default (props: { open: boolean }) => {
  const {
    state: channels,
    actions: { create_channel },
  } = UseChannels();

  if (!props.open) return <></>;

  return (
    <View>
      {channels?.map((c) => (
        <ChannelView key={c.ChannelId} channel={c} />
      ))}
    </View>
  );
};
