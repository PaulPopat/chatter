import { FlatList, Text, StyleSheet, View } from "react-native";
import UseChat, { Message } from "../data/use-chat";
import {
  BorderRadius,
  Colours,
  FontSizes,
  Margins,
  Padding,
} from "../styles/theme";
import { Form } from "../atoms/form";
import Textbox from "../atoms/textbox";
import Hidden from "../atoms/hidden";
import UsePublicProfile from "../data/use-public-profile";
import Submitter from "../atoms/submitter";
import Icon from "../atoms/icon";

const styles = StyleSheet.create({
  message_container: {
    marginVertical: Margins,
    backgroundColor: Colours.Highlight.Background,
    borderRadius: BorderRadius,
  },
  metadata_container: {
    flexDirection: "row",
    alignItems: "center",
  },
  name_text: {
    fontWeight: "bold",
    padding: Padding,
    fontSize: FontSizes.Label,
  },
  time_text: {
    fontSize: FontSizes.Small,
  },
  message_text: {
    padding: Padding,
    fontSize: FontSizes.Label,
  },
  channel_container: {
    padding: Margins,
    height: "100%",
  },
  chat_input: {
    flexDirection: "row",
    alignItems: "center",
  },
  send_button: {
    margin: Margins,
  },
  text_box: {
    flex: 1,
  },
});

const ChatMessage = (props: { message: Message }) => {
  const profile = UsePublicProfile(props.message.Who);
  return (
    <View style={styles.message_container}>
      <View style={styles.metadata_container}>
        <Text style={styles.name_text}>{profile?.UserName}</Text>
        <Text style={styles.time_text}>
          {props.message.When.toLocaleString()}
        </Text>
      </View>
      <Text style={styles.message_text}>{props.message.Text}</Text>
    </View>
  );
};

export default (props: { channel_id: string }) => {
  const {
    state: messages,
    actions: { send, fetch_more },
  } = UseChat(props.channel_id);

  return (
    <View style={styles.channel_container}>
      <FlatList
        data={messages}
        renderItem={({ item: message }) => (
          <ChatMessage message={message} key={message.When.toISOString()} />
        )}
        inverted
        onEndReached={() => fetch_more()}
      />

      <Form fetcher={send}>
        <Hidden name="channel_id" value={props.channel_id} />
        <View style={styles.chat_input}>
          <View style={styles.text_box}>
            <Textbox name="Text" clear_on_submit>
              Message
            </Textbox>
          </View>
          <View style={styles.send_button}>
            <Submitter colour="Highlight">
              <Icon area="Business" icon="send-plane-2" />
            </Submitter>
          </View>
        </View>
      </Form>
    </View>
  );
};
