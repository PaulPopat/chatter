import { Text, View, StyleSheet, Pressable, ScrollView } from "react-native";
import UseChannels, { Channel } from "../data/use-channels";
import { PropsWithChildren, useState } from "react";
import Icon from "../atoms/icon";
import { BorderRadius, Colours, Margins, Padding } from "../styles/theme";
import ChannelView from "./channel-view";
import UseServer from "../data/use-server";
import Button from "../atoms/button";
import Modal from "../atoms/modal";
import { Form } from "../atoms/form";
import Submitter from "../atoms/submitter";
import Textbox from "../atoms/textbox";
import Checkbox from "../atoms/checkbox";
import useServerMetadata from "../data/use-server-metadata";
import UseOrientation from "../utils/orientation";
import TopBar from "../atoms/top-bar";
import ResponsiveModal from "../atoms/responsive-modal";
import FileUpload from "../atoms/file-upload";

const styles = StyleSheet.create({
  channel_container: {
    flexDirection: "row",
    alignItems: "center",
    padding: Padding,
    backgroundColor: Colours.Body.Background,
    borderRadius: BorderRadius,
    margin: Padding,
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
    overflow: "hidden",
    width: 220,
  },
  channel_list_scroller: {
    flex: 1,
  },
  button_container: {
    margin: Padding,
  },
  server_container: {
    flexDirection: "row",
    height: "100%",
  },
  server_view: {
    flex: 1,
    height: "100%",
  },
  config_container: {
    margin: Margins,
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

export default (props: { open: boolean; blur: () => void }) => {
  const server = UseServer();
  const {
    state: channels,
    actions: { create_channel },
  } = UseChannels();
  const {
    state: metadata,
    actions: { update },
  } = useServerMetadata();
  const [open_channel, set_open_channel] = useState<Channel | null>(null);
  const [configuring, set_configuring] = useState(false);
  const [creating, set_creating] = useState(false);
  const orientation = UseOrientation();

  return (
    <ResponsiveModal style={styles.server_container} open={props.open}>
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

      <View
        style={{
          ...styles.channel_list,
          ...(orientation === "portrait"
            ? !open_channel
              ? {
                  width: "100%",
                  borderRightWidth: 0,
                }
              : {
                  width: 0,
                  overflow: "hidden",
                  borderRightWidth: 0,
                }
            : {}),
        }}
      >
        <TopBar click={props.blur} title={metadata?.ServerName}>
          {server.IsAdmin && (
            <Pressable
              onPress={() => {
                set_open_channel(null);
                set_configuring(true);
              }}
            >
              <Icon area="System" icon="settings-2" />
            </Pressable>
          )}
        </TopBar>
        <ScrollView style={styles.channel_list_scroller}>
          {channels?.map((c) => (
            <ChannelListItem
              key={c.ChannelId}
              channel={c}
              on_open={() => set_open_channel(c)}
            />
          ))}
        </ScrollView>

        {server?.IsAdmin && (
          <View style={styles.button_container}>
            <Button on_click={() => set_creating(true)} colour="Secondary">
              +
            </Button>
          </View>
        )}
      </View>

      <ResponsiveModal
        style={styles.server_view}
        open={!!open_channel || configuring}
        colour="Body"
      >
        {open_channel ? (
          <ChannelView
            channel={open_channel}
            blur={() => set_open_channel(null)}
          />
        ) : (
          <View style={styles.config_container}>
            <Form fetcher={update}>
              <Textbox name="ServerName">Server Name</Textbox>
              <FileUpload name="Icon">Server Icon</FileUpload>
              <Submitter>Save Changes</Submitter>
            </Form>
          </View>
        )}
      </ResponsiveModal>
    </ResponsiveModal>
  );
};
