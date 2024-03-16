import { useState } from "react";
import UseProfile, { UseServerListMetadata } from "../data/use-profile";
import { View, Pressable, Text } from "react-native";
import { Classes } from "../styles/theme";
import { Form } from "../atoms/form";
import Textbox from "../atoms/textbox";
import Submitter from "../atoms/submitter";
import Button from "../atoms/button";
import UseSso from "../data/use-sso";
import Hidden from "../atoms/hidden";
import Modal from "../atoms/modal";
import Image from "../atoms/image";
import DataUrl from "../utils/data-url";
import UseOrientation from "../utils/orientation";

const ServerListItem = (props: { url: string }) => {
  const metadata = UseServerListMetadata(props.url);
  const orientation = UseOrientation();

  return (
    <View style={Classes("row", "colour_body", "card", "container")}>
      <Image
        src={new DataUrl(metadata.Icon.Base64Data, metadata.Icon.MimeType)}
        size={60}
      />
      {orientation === "portrait" && (
        <Text style={Classes("spacer")}>{metadata.ServerName}</Text>
      )}
    </View>
  );
};

export default (props: {
  on_open: (server: string) => void;
  profile: ReturnType<typeof UseProfile>;
}) => {
  const {
    state: profile,
    actions: { join_server },
  } = props.profile;
  const sso = UseSso();
  const [joining, set_joining] = useState(false);

  return (
    <View style={Classes("flex_fill")}>
      <Modal open={joining} set_open={set_joining}>
        <Form
          fetcher={join_server}
          on_submit={() => set_joining(false)}
          classes={["column"]}
        >
          <Textbox name="ServerUrl">Server URL</Textbox>
          <Hidden name="ServerToken" value={sso.ServerToken} />
          <Textbox name="Password" password>
            Password
          </Textbox>
          <View style={Classes("row")}>
            <Submitter>Join Server</Submitter>
            <Button on_click={() => set_joining(false)} colour="Danger">
              Cancel
            </Button>
          </View>
        </Form>
      </Modal>

      <View style={Classes("flex_fill", "column")}>
        {profile?.Servers.map((s) => (
          <Pressable
            key={s.Url}
            onPress={() => props.on_open(s.Url)}
          >
            <ServerListItem url={s.Url} />
          </Pressable>
        ))}
      </View>

      <Button on_click={() => set_joining(true)} colour="Secondary">
        +
      </Button>
    </View>
  );
};
