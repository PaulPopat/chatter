import { View, StyleSheet, Text } from "react-native";
import { Form } from "../atoms/form";
import Submitter from "../atoms/submitter";
import Textbox from "../atoms/textbox";
import UseOrientation from "../utils/orientation";
import { FontSizes, Margins } from "../styles/theme";
import { UseSsoControls } from "../data/use-sso";
import { useState } from "react";
import Button from "../atoms/button";

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
  const [mode, set_mode] = useState("login" as "login" | "register");

  if (mode === "login")
    return (
      <View
        style={{
          ...styles.container,
          flexDirection: orientation === "landscape" ? "row" : "column",
        }}
      >
        <Form style={styles.form} fetcher={login}>
          <Textbox name="email" keyboard="email-address">
            Email
          </Textbox>
          <Textbox name="password" password>
            Password
          </Textbox>
          <Submitter>Log In</Submitter>
          <Button colour="Secondary" on_click={() => set_mode("register")}>
            No Account?
          </Button>
        </Form>
      </View>
    );

  return (
    <View
      style={{
        ...styles.container,
        flexDirection: orientation === "landscape" ? "row" : "column",
      }}
    >
      <Form style={styles.form} fetcher={register}>
        <Textbox name="UserName">User Name</Textbox>
        <Textbox name="Email" keyboard="email-address">
          Email
        </Textbox>
        <Textbox name="Password" password>
          Password
        </Textbox>
        <Submitter>Register</Submitter>
        <Button colour="Secondary" on_click={() => set_mode("login")}>
          Already Have an Account?
        </Button>
      </Form>
    </View>
  );
};
