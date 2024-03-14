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

const styles = StyleSheet.create({
  message_container: {
    margin: Margins,
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
    <>
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
        <Textbox name="Text" clear_on_submit>
          Message
        </Textbox>
      </Form>
    </>
  );
};
