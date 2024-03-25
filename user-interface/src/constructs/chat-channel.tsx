import UseChat, { Message } from "../data/use-chat";
import { Form } from "../atoms/form";
import Textbox from "../atoms/textbox";
import Hidden from "../atoms/hidden";
import UsePublicProfile from "../data/use-public-profile";
import Submitter from "../atoms/submitter";
import Icon from "../atoms/icon";
import Image from "../atoms/image";
import SsoAsset from "../utils/sso-asset";
import { View, Text } from "../atoms/native";
import { FlatList } from "react-native";

const ChatMessage = (props: { message: Message }) => {
  const profile = UsePublicProfile(props.message.Who);

  return (
    <View class="highlight spacer column align_top">
      <View class={"row container"}>
        <Image
          src={
            new SsoAsset("/profile/pictures/:user_id", {
              user_id: props.message.Who,
            })
          }
          size={24}
          class="card"
        />
        <Text class="important_text">{profile?.UserName}</Text>
        <Text class="small_text">{props.message.When.toLocaleString()}</Text>
      </View>
      <Text class="body_text container">{props.message.Text}</Text>
    </View>
  );
};

export default (props: { channel_id: string }) => {
  const {
    state: messages,
    actions: { send, fetch_more },
  } = UseChat(props.channel_id);

  return (
    <View class="fill">
      <FlatList
        data={messages}
        renderItem={({ item: message }) => (
          <ChatMessage message={message} key={message.When.toISOString()} />
        )}
        inverted
        onEndReached={() => fetch_more()}
      />

      <Form fetcher={send} hide_notification>
        <Hidden name="channel_id" value={props.channel_id} />
        <View class="row">
          <View class="flex_fill spacer">
            <Textbox name="Text" clear_on_submit multiline>
              Message
            </Textbox>
          </View>
          <View class="spacer">
            <Submitter colour="Highlight">
              <Icon area="Business" icon="send-plane-2" />
            </Submitter>
          </View>
        </View>
      </Form>
    </View>
  );
};
