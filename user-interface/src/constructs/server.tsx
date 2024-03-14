import { Text, View, StyleSheet, Pressable } from "react-native";
import UseChannels, { Channel } from "../data/use-channels";
import { PropsWithChildren, useState } from "react";
import Icon from "../atoms/icon";
import { BorderRadius, Colours, Padding } from "../styles/theme";
import ChannelView from "./channel-view";
import UseServer from "../data/use-server";
import Button from "../atoms/button";
import Modal from "../atoms/modal";
import { Form } from "../atoms/form";
import Submitter from "../atoms/submitter";
import Textbox from "../atoms/textbox";
import Checkbox from "../atoms/checkbox";

const styles = StyleSheet.create({
  channel_container: {
    flexDirection: "row",
    alignItems: "center",
    padding: Padding,
    backgroundColor: Colours.Body.Background,
    borderRadius: BorderRadius,
    marginVertical: Padding,
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
    overflow: "scroll",
    padding: Padding,
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
  const server = UseServer();
  const {
    state: channels,
    actions: { create_channel },
  } = UseChannels();
  const [open_channel, set_open_channel] = useState<Channel | null>(null);
  const [creating, set_creating] = useState(false);

  if (!props.open) return <></>;

  return (
    <View style={styles.server_container}>
      <Modal open={creating} set_open={set_creating}>
        <Form fetcher={create_channel} on_submit={() => set_creating(false)}>
          <Textbox name="Name">Channel Name</Textbox>
          <Checkbox name="Public">Is Public</Checkbox>
          <Submitter>Create Channel</Submitter>
          <Button on_click={() => set_creating(false)} colour="Danger">
            Cancel
          </Button>
        </Form>
      </Modal>

      <View style={styles.channel_list}>
        {channels?.map((c) => (
          <ChannelListItem
            key={c.ChannelId}
            channel={c}
            on_open={() => set_open_channel(c)}
          />
        ))}

        {server?.IsAdmin && (
          <Button on_click={() => set_creating(true)} colour="Secondary">
            +
          </Button>
        )}
      </View>

      <View style={styles.server_view}>
        {open_channel && <ChannelView channel={open_channel} />}
      </View>
    </View>
  );
};
