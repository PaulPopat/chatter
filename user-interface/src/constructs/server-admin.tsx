import { Pressable, ScrollView, Text, View } from "react-native";
import FileUpload from "../atoms/file-upload";
import { Form, RawForm } from "../atoms/form";
import Submitter from "../atoms/submitter";
import Textbox from "../atoms/textbox";
import UseServerMetadata from "../data/use-server-metadata";
import { Classes } from "../styles/theme";
import UseServerUsers, { ServerUser } from "../data/use-server-users";
import UsePublicProfile from "../data/use-public-profile";
import { z } from "zod";
import Checkbox from "../atoms/checkbox";
import UseUserPermission from "../data/use-user-permission";
import Icon from "../atoms/icon";
import Button from "../atoms/button";
import { useState } from "react";
import Modal from "../atoms/modal";
import UseChannels from "../data/use-channels";
import TopBar from "../atoms/top-bar";

const PermissionForm = z.object({
  read: z.boolean(),
  write: z.boolean(),
});

const UserPermissions = (props: { user: ServerUser }) => {
  const {
    state: permissions,
    actions: { add_user_to_channel, kick_user_from_channel },
  } = UseUserPermission({
    user_id: props.user.UserId,
  });
  const { state: channels } = UseChannels();
  return (
    <ScrollView>
      <View style={Classes("column", "spacer")}>
        {channels?.map((c) => (
          <View
            key={c.ChannelId}
            style={Classes("card", "colour_body", "container")}
          >
            <Text>{c.Name}</Text>

            <RawForm
              on_submit={async (data) => {
                await kick_user_from_channel({
                  user_id: props.user.UserId,
                  channel_id: c.ChannelId,
                });

                if (data.read)
                  await add_user_to_channel({
                    channel_id: c.ChannelId,
                    UserId: props.user.UserId,
                    AllowWrite: data.write,
                  });
              }}
              form_type={PermissionForm}
            >
              <Checkbox
                name="read"
                default_value={
                  !!permissions?.find((p) => p.ChannelId === c.ChannelId)
                }
                clear_on_submit
                submit_on_change
              >
                May View
              </Checkbox>
              <Checkbox
                name="write"
                default_value={
                  !!permissions?.find(
                    (p) => p.ChannelId === c.ChannelId && p.Write
                  )
                }
                clear_on_submit
                submit_on_change
              >
                May Post
              </Checkbox>
            </RawForm>
          </View>
        ))}
      </View>
    </ScrollView>
  );
};

const UserForm = z.object({
  admin: z.boolean(),
});

const ServerUserDiplay = (props: {
  user: ServerUser;
  set_admin: (admin: boolean) => void;
  ban: () => void;
}) => {
  const profile = UsePublicProfile(props.user.UserId);
  const [is_banning, set_is_banning] = useState(false);
  const [is_permissioning, set_is_permissioning] = useState(false);

  return (
    <View style={Classes("row", "card", "container")}>
      <View style={Classes("flex_fill", "body_text")}>
        <Text>{profile?.UserName}</Text>
      </View>

      <Button colour="Danger" on_click={() => set_is_banning(true)}>
        Ban
      </Button>

      <RawForm
        on_submit={(data) => props.set_admin(data.admin)}
        form_type={UserForm}
      >
        <Checkbox
          name="admin"
          default_value={props.user.Admin}
          submit_on_change
        >
          Admin
        </Checkbox>
      </RawForm>

      {!props.user.Admin && (
        <Pressable onPress={() => set_is_permissioning(true)}>
          <Icon area="System" icon="lock-2" />
        </Pressable>
      )}

      <Modal open={is_banning} set_open={set_is_banning}>
        <Text>Are you sure?</Text>
        <Text>
          This will block the user from performing any server actions.
        </Text>

        <Button on_click={() => props.ban()}>Yes. Ban Them!</Button>
        <Button on_click={() => set_is_banning(false)} colour="Danger">
          Cancel
        </Button>
      </Modal>

      {!props.user.Admin && (
        <Modal open={is_permissioning} set_open={set_is_permissioning}>
          <UserPermissions user={props.user} />
          <Button on_click={() => set_is_permissioning(false)} colour="Danger">
            Close
          </Button>
        </Modal>
      )}
    </View>
  );
};

export default (props: { blur: () => void }) => {
  const {
    state: metadata,
    actions: { update },
  } = UseServerMetadata();

  const {
    state: users,
    actions: { give_admin, ban, revoke_admin },
  } = UseServerUsers();

  return (
    <View style={Classes("fill")}>
      <TopBar click={props.blur} title={metadata?.ServerName}></TopBar>
      <ScrollView>
        <Form fetcher={update} classes={["container", "column"]}>
          <Textbox name="ServerName" default_value={metadata?.ServerName}>
            Server Name
          </Textbox>
          <FileUpload name="Icon">Server Icon</FileUpload>
          <Submitter>Save Changes</Submitter>
        </Form>

        <View style={Classes("column", "spacer")}>
          {users?.map((u) => (
            <ServerUserDiplay
              key={u.UserId}
              user={u}
              set_admin={(a) =>
                a
                  ? give_admin({ UserId: u.UserId })
                  : revoke_admin({ user_id: u.UserId })
              }
              ban={() => ban({ UserId: u.UserId })}
            />
          ))}
        </View>
      </ScrollView>
    </View>
  );
};
