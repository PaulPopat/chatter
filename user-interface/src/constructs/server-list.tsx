import { useState } from "react";
import UseProfile, { UseServerMetadata } from "../data/use-profile";
import { View, Text, Image, StyleSheet, Pressable } from "react-native";
import {
  BorderRadius,
  BorderWidth,
  Colours,
  Margins,
  Padding,
} from "../styles/theme";
import JoinUrl from "../utils/url-join";
import { Form } from "../atoms/form";
import Textbox from "../atoms/textbox";
import Submitter from "../atoms/submitter";
import Button from "../atoms/button";
import UseSso from "../data/use-sso";
import Hidden from "../atoms/hidden";
import Modal from "../atoms/modal";

const styles = StyleSheet.create({
  panel_container: {
    flexDirection: "column",
    height: "100%",
  },
  server_list_container: {
    flex: 1,
  },
  server_container: {
    flexDirection: "row",
  },
  server_text: {
    flex: 1,
  },
});

const ServerListItem = (props: { url: string }) => {
  const metadata = UseServerMetadata(props.url);

  return (
    <>
      <Image
        source={{
          uri: `data:${metadata.Icon.MimeType};base64,${metadata.Icon.Base64Data}`,
          width: 60,
          height: 60
        }}
      />
    </>
  );
};

export default (props: { on_open: (server: string) => void }) => {
  const {
    state: profile,
    actions: { join_server },
  } = UseProfile();
  const sso = UseSso();
  const [joining, set_joining] = useState(false);

  return (
    <View style={styles.panel_container}>
      <Modal open={joining} set_open={set_joining}>
        <Form fetcher={join_server}>
          <Textbox name="ServerUrl">Server URL</Textbox>
          <Hidden name="ServerToken" value={sso.ServerToken} />
          <Textbox name="Password" password>
            Password
          </Textbox>
          <Submitter>Join Server</Submitter>
          <Button on_click={() => set_joining(false)} colour="Danger">
            Cancel
          </Button>
        </Form>
      </Modal>

      <View style={styles.server_list_container}>
        {profile?.Servers.map((s) => (
          <Pressable
            style={styles.server_container}
            key={s.Url}
            onPress={() => props.on_open(s.Url)}
          >
            <ServerListItem url={s.Url} />
          </Pressable>
        ))}
      </View>

      <Button on_click={() => set_joining(true)} colour="Secondary">
        Join Server
      </Button>
    </View>
  );
};
