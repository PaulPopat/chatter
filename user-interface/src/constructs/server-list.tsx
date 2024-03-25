import { useState } from "react";
import UseProfile, { UseServerListMetadata } from "../data/use-profile";
import { Form } from "../atoms/form";
import Textbox from "../atoms/textbox";
import Submitter from "../atoms/submitter";
import Button from "../atoms/button";
import UseSso from "../data/use-sso";
import Hidden from "../atoms/hidden";
import Modal from "../atoms/modal";
import Image from "../atoms/image";
import DataAsset from "../utils/data-asset";
import UseOrientation from "../utils/orientation";
import UseArgs from "../data/use-args";
import { View, Text, Pressable } from "../atoms/native";

const ServerListItem = (props: { url: string }) => {
  const metadata = UseServerListMetadata(props.url);
  const orientation = UseOrientation();

  return (
    <View class="row">
      <Image
        src={new DataAsset(metadata.Icon.Base64Data, metadata.Icon.MimeType)}
        size={60}
        class="card olour_body"
      />
      {orientation === "portrait" && (
        <Text class="spacer">{metadata.ServerName}</Text>
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

  const {
    state: args,
    actions: { clear_args },
  } = UseArgs();

  const [joining, set_joining] = useState(args.action === "join");

  return (
    <View class="flex_fill">
      <Modal open={joining} set_open={set_joining} title="Join a Server">
        <Form
          fetcher={join_server}
          on_submit={() => {
            set_joining(false);
            clear_args();
          }}
          class="column"
        >
          <Textbox name="ServerUrl" default_value={args.server_url}>
            Server URL
          </Textbox>
          <Hidden name="ServerToken" value={sso.ServerToken} />
          <Textbox name="Password" default_value={args.password} password>
            Password
          </Textbox>
          <View class="row">
            <Submitter>Join Server</Submitter>
            <Button on_click={() => set_joining(false)} colour="Danger">
              Cancel
            </Button>
          </View>
        </Form>
      </Modal>

      <View class="flex_fill column">
        {profile?.Servers.map((s) => (
          <Pressable key={s.Url} onPress={() => props.on_open(s.Url)}>
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
