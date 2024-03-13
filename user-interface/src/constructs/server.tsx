import { Text, View, StyleSheet, Pressable } from "react-native";
import UseChannels, { Channel } from "../data/use-channels";
import { PropsWithChildren, useState } from "react";
import Icon from "../atoms/icon";
import { BorderRadius, Colours, Padding } from "../styles/theme";
import ChannelView from "./channel-view";

const styles = StyleSheet.create({
  channel_container: {
    flexDirection: "row",
    alignItems: "center",
    padding: Padding,
    margin: Padding,
    backgroundColor: Colours.Body.Background,
    borderRadius: BorderRadius,
  },
  channel_name: {
    paddingLeft: Padding,
    color: Colours.Highlight.Foreground,
  },
  channel_list: {
    flexDirection: "column",
    backgroundColor: Colours.Highlight.Background,
    height: "100%",
    borderRightColor: Colours.Highlight.Foreground,
    borderRightWidth: 2,
  },
  server_container: {
    flexDirection: "row",
    height: "100%",
  },
  server_view: {
    flex: 1,
    height: "100%",
  },
});

const ChannelListItem = (
  props: PropsWithChildren<{ channel: Channel; on_open: () => void }>
) => {
  return (
    <Pressable onPress={props.on_open}>
      <View style={styles.channel_container}>
        <Icon area="Communication" icon="chat-3" />
        <Text style={styles.channel_name}>{props.channel.Name}</Text>
      </View>
    </Pressable>
  );
};

export default (props: { open: boolean }) => {
  const { state: channels } = UseChannels();
  const [open_channel, set_open_channel] = useState("");

  if (!props.open) return <></>;

  return (
    <View style={styles.server_container}>
      <View style={styles.channel_list}>
        {channels?.map((c) => (
          <ChannelListItem
            key={c.ChannelId}
            channel={c}
            on_open={() => set_open_channel(c.ChannelId)}
          />
        ))}
      </View>

      <View style={styles.server_view}>
        {open_channel && <ChannelView channel_id={open_channel} />}
      </View>
    </View>
  );
};
