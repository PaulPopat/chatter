import { Text, View, Pressable, ScrollView } from "react-native";
import UseChannels, { Channel } from "../data/use-channels";
import { PropsWithChildren, useState } from "react";
import Icon from "../atoms/icon";
import { Classes } from "../styles/theme";
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
import ResponsiveModal from "../molecules/responsive-modal";
import ServerAdmin from "./server-admin";

const ChannelListItem = (
  props: PropsWithChildren<{ channel: Channel; on_open: () => void }>
) => {
  return (
    <Pressable onPress={props.on_open}>
      <View style={Classes("card", "row", "colour_body", "container")}>
        <Icon area="Communication" icon="chat-3" />
        <Text>{props.channel.Name}</Text>
      </View>
    </Pressable>
  );
};

export default (props: { open: boolean; blur: () => void }) => {
  const server = UseServer();
  const {
    state: channels,
    actions: { create_channel, update_channel },
  } = UseChannels();
  const { state: metadata } = useServerMetadata();
  const [open_channel, set_open_channel] = useState<Channel | null>(null);
  const [configuring, set_configuring] = useState(false);
  const [creating, set_creating] = useState(false);
  const orientation = UseOrientation();

  return (
    <ResponsiveModal classes={["row", "fill", "no_gap"]} open={props.open}>
      <Modal open={creating} set_open={set_creating} title="Create a Channel">
        <Form
          fetcher={create_channel}
          on_submit={() => set_creating(false)}
          classes={["column"]}
        >
          <Textbox name="Name">Channel Name</Textbox>
          <Checkbox name="Public">Is Public</Checkbox>
          <View style={Classes("row")}>
            <Submitter>Create Channel</Submitter>
            <Button on_click={() => set_creating(false)} colour="Danger">
              Cancel
            </Button>
          </View>
        </Form>
      </Modal>

      <View
        style={{
          ...Classes("colour_highlight", "fill", "border_right"),
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
            : {
                width: 220,
              }),
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
        <ScrollView style={Classes("flex_fill")}>
          <View style={Classes("column", "edge_container")}>
            {channels?.map((c) => (
              <ChannelListItem
                key={c.ChannelId}
                channel={c}
                on_open={() => set_open_channel(c)}
              />
            ))}
          </View>
        </ScrollView>

        {server?.IsAdmin && (
          <Button
            on_click={() => set_creating(true)}
            colour="Secondary"
            classes={["spacer"]}
          >
            +
          </Button>
        )}
      </View>

      <ResponsiveModal
        classes={["flex_fill", "fill"]}
        open={!!open_channel || configuring}
        colour="Body"
      >
        {open_channel ? (
          <ChannelView
            channel={open_channel}
            update={update_channel}
            blur={() => set_open_channel(null)}
          />
        ) : (
          <ServerAdmin
            blur={() => set_configuring(false)}
            url={server.BaseUrl}
          />
        )}
      </ResponsiveModal>
    </ResponsiveModal>
  );
};
