import { useEffect, useState } from "react";
import { View, StyleSheet, Text } from "react-native";
import { Classes, Colours, Padding } from "./styles/theme";
import ServerList from "./constructs/server-list";
import Authenticate from "./constructs/authenticate";
import Server from "./constructs/server";
import UseSso, { SsoProvider, UseSsoControls } from "./data/use-sso";
import { ServerProvider } from "./data/use-server";
import useProfile from "./data/use-profile";
import UseOrientation from "./utils/orientation";
import Loading from "./atoms/loading";
import Button from "./atoms/button";
import Icon from "./atoms/icon";
import Modal from "./atoms/modal";
import { Form } from "./atoms/form";
import FileUpload from "./atoms/file-upload";
import Submitter from "./atoms/submitter";
import Textbox from "./atoms/textbox";

const MainPanel = () => {
  const profile = useProfile();
  const [open, set_open] = useState("");
  const [updating_profile, set_updating_profile] = useState(false);
  const orientation = UseOrientation();

  return (
    <View style={Classes("row", "fill", "no_overflow", "no_gap")}>
      <View
        style={{
          ...Classes("highlight", "border_right", "fill"),
          ...(orientation === "landscape"
            ? {
                width: 90,
              }
            : {
                width: "100%",
                borderRightWidth: 0,
                overflow: "hidden",
              }),
        }}
      >
        <View style={Classes("fill", "column")}>
          <ServerList on_open={set_open} profile={profile} />
          <Button on_click={() => set_updating_profile(true)}>
            <Icon area="User & Faces" icon="user" />
          </Button>
        </View>
      </View>
      <View style={Classes("flex_fill", "fill")}>
        {profile.state?.Servers.map((s) => (
          <ServerProvider key={s.Url} url={s.Url}>
            <Server open={open === s.Url} blur={() => set_open("")} />
          </ServerProvider>
        ))}
      </View>

      <Modal open={updating_profile} set_open={set_updating_profile}>
        <Form fetcher={profile.actions.update_profile} classes={["column"]}>
          <Textbox name="UserName" default_value={profile.state?.UserName}>
            User Name
          </Textbox>
          <Textbox
            name="Biography"
            multiline
            default_value={profile.state?.Biography}
          >
            Biography
          </Textbox>
          <FileUpload name="Picture">Profile Picture</FileUpload>
          <Submitter>Update Profile</Submitter>
          <Button on_click={() => set_updating_profile(false)} colour="Danger">
            Close
          </Button>
        </Form>
      </Modal>
    </View>
  );
};

const Main = () => {
  const auth = UseSso();
  const { refresh } = UseSsoControls();

  useEffect(() => {
    const interval = setInterval(() => {
      if (auth.AdminToken && auth.IsExpired)
        refresh({ token: auth.RefreshToken });
    }, 10000);

    return () => {
      clearInterval(interval);
    };
  }, [auth]);

  if (auth.AdminToken && auth.IsExpired) return <Loading />;
  if (!auth.AdminToken) return <Authenticate />;

  return <MainPanel />;
};

export default () => {
  return (
    <SsoProvider>
      <Main />
    </SsoProvider>
  );
};
