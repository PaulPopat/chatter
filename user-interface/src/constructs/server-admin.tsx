import { StyleSheet, View } from "react-native";
import FileUpload from "../atoms/file-upload";
import { Form } from "../atoms/form";
import Submitter from "../atoms/submitter";
import Textbox from "../atoms/textbox";
import UseServerMetadata from "../data/use-server-metadata";
import { Margins } from "../styles/theme";

const styles = StyleSheet.create({
  config_container: {
    margin: Margins,
  },
});

export default () => {
  const {
    actions: { update },
  } = UseServerMetadata();
  return (
    <View style={styles.config_container}>
      <Form fetcher={update}>
        <Textbox name="ServerName">Server Name</Textbox>
        <FileUpload name="Icon">Server Icon</FileUpload>
        <Submitter>Save Changes</Submitter>
      </Form>
    </View>
  );
};
