import { PropsWithChildren, useState } from "react";
import UseProfile from "../data/use-profile";
import { View, Text, Image, StyleSheet, Modal, Pressable } from "react-native";
import {
  BorderRadius,
  BorderWidth,
  Colours,
  Margins,
  Padding,
} from "../styles/theme";
import JoinUrl from "../utils/url-join";
import { Form, RawForm } from "../atoms/form";
import { z } from "zod";
import Textbox from "../atoms/textbox";
import Submitter from "../atoms/submitter";
import Button from "../atoms/button";

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
  modal_body: {
    backgroundColor: Colours.Highlight.Background,
    color: Colours.Highlight.Foreground,
    margin: Margins,
    padding: Padding,
    borderWidth: BorderWidth,
    borderRadius: BorderRadius,
  },
});

export default (props: { on_open: (server: string) => void }) => {
  const { profile, join_server } = UseProfile();
  const [joining, set_joining] = useState(false);

  return (
    <View style={styles.panel_container}>
      <Modal
        animationType="slide"
        transparent={true}
        visible={joining}
        onRequestClose={() => {
          set_joining(false);
        }}
        onDismiss={() => {
          set_joining(false);
        }}
      >
        <View style={styles.modal_body}>
          <Form fetcher={join_server}>
            <Textbox name="ServerUrl">Server URL</Textbox>
            <Submitter>Join Server</Submitter>
            <Button on_click={() => set_joining(false)} colour="Danger">
              Cancel
            </Button>
          </Form>
        </View>
      </Modal>

      <View style={styles.server_list_container}>
        {profile?.Servers.map((s) => (
          <Pressable
            style={styles.server_container}
            key={s.Url}
            onPress={() => props.on_open(s.Url)}
          >
            <Image
              source={{
                uri: JoinUrl(s.Url, "/_/images/icon"),
              }}
            />
            <Text style={styles.server_text}>{new URL(s.Url).host}</Text>
          </Pressable>
        ))}
      </View>

      <Button on_click={() => set_joining(true)} colour="Secondary">
        Join Server
      </Button>
    </View>
  );
};
