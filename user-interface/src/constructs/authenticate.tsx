import { View, StyleSheet, Text } from "react-native";
import { Form } from "../atoms/form";
import Submitter from "../atoms/submitter";
import Textbox from "../atoms/textbox";
import { Sso } from "../auth/sso";
import UseOrientation from "../utils/orientation";
import { FontSizes, Margins } from "../styles/theme";
import { UseSsoControls } from "../data/use-sso";

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
  form: {
    flex: 1,
    padding: Margins,
  },
  form_title: {
    fontSize: FontSizes.Title,
  },
});

export default () => {
  const orientation = UseOrientation();
  const { login, register } = UseSsoControls();

  return (
    <View
      style={{
        ...styles.container,
        flexDirection: orientation === "landscape" ? "row" : "column",
      }}
    >
      <Form style={styles.form} fetcher={login}>
        <Text style={styles.form_title}>Already have an account?</Text>
        <Textbox name="email" keyboard="email-address">
          Email
        </Textbox>
        <Textbox name="password" password>
          Password
        </Textbox>
        <Submitter>Log In</Submitter>
      </Form>
      <Form style={styles.form} fetcher={register}>
        <Text style={styles.form_title}>Alternatively</Text>
        <Textbox name="UserName">User Name</Textbox>
        <Textbox name="Email" keyboard="email-address">
          Email
        </Textbox>
        <Textbox name="Password" password>
          Password
        </Textbox>
        <Submitter>Register</Submitter>
      </Form>
    </View>
  );
};
