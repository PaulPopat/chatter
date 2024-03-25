import { Channel } from "../data/use-channels";
import ChatChannel from "./chat-channel";
import TopBar from "../atoms/top-bar";
import UseServer from "../data/use-server";
import Icon from "../atoms/icon";
import TriggeredModal from "../molecules/triggered-modal";
import { Fetcher } from "../utils/fetch";
import { Form } from "../atoms/form";
import Hidden from "../atoms/hidden";
import Textbox from "../atoms/textbox";
import Checkbox from "../atoms/checkbox";
import Submitter from "../atoms/submitter";
import { View, Text } from "../atoms/native";

const SubChannel = (props: { channel: Channel }) => {
  switch (props.channel.Type) {
    case "Messages":
      return <ChatChannel channel_id={props.channel.ChannelId} />;
    default:
      return <Text>Unknown Channel Type</Text>;
  }
};

export default (props: {
  channel: Channel;
  update: Fetcher<any>;
  blur: () => void;
}) => {
  const server = UseServer();
  return (
    <View class="fill">
      <TopBar click={props.blur} title={props.channel.Name}>
        {server.IsAdmin && (
          <TriggeredModal
            button={<Icon area="System" icon="settings-2" class="body-text" />}
            title="Edit Channel"
          >
            <Form fetcher={props.update}>
              <Hidden name="channel_id" value={props.channel.ChannelId} />
              <Textbox name="Name" default_value={props.channel.Name}>
                Channel Name
              </Textbox>
              <Checkbox name="Public" default_value={props.channel.Public}>
                Is Public
              </Checkbox>
              <View class="row">
                <Submitter>Update Channel</Submitter>
              </View>
            </Form>
          </TriggeredModal>
        )}
      </TopBar>
      <View class="flex-fill">
        <SubChannel {...props} />
      </View>
    </View>
  );
};
