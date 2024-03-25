import { Form } from "../atoms/form";
import Submitter from "../atoms/submitter";
import Textbox from "../atoms/textbox";
import { UseSsoControls } from "../data/use-sso";
import { useState } from "react";
import Button from "../atoms/button";
import { View } from "../atoms/native";

export default () => {
  const { login, register } = UseSsoControls();
  const [mode, set_mode] = useState("login" as "login" | "register");

  return (
    <View class="modal">
      <View class="container">
        {mode === "login" ? (
          <Form fetcher={login}>
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
        ) : (
          <Form fetcher={register}>
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
        )}
      </View>
    </View>
  );
};
