import {
  PropsWithChildren,
  createContext,
  useContext,
  useEffect,
  useState,
} from "react";
import { Form } from "../../atoms/form";
import Textbox from "../../atoms/textbox";
import Submitter from "../../atoms/submitter";
import { Session } from "../../utils/storage";
import { StyleSheet, Text, View } from "react-native";
import { Auth, Sso } from "./sso-object";
import UseOrientation from "../../utils/orientation";
import { FontSizes, Margins } from "../../styles/theme";

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

const AuthContext = createContext(Sso.Stored);

export function UseSso() {
  return useContext(AuthContext);
}

export const SsoProvider = (props: PropsWithChildren) => {
  const [auth, set_auth] = useState(Sso.Stored);

  useEffect(() => {
    const interval = setInterval(() => {
      if (auth.IsExpired) auth.AsRefreshed().then(set_auth);
    }, 10000);

    return () => {
      clearInterval(interval);
    };
  }, [auth]);

  const orientation = UseOrientation();

  if (!auth.AdminToken) {
    return (
      <View
        style={{
          ...styles.container,
          flexDirection: orientation === "landscape" ? "row" : "column",
        }}
      >
        <Form
          style={styles.form}
          area="sso"
          method="GET"
          url="/api/v1/auth/token"
          on_success={(_, d) => {
            set_auth(new Sso(d));
            Session.auth = d;
          }}
          expect={Auth}
        >
          <Text style={styles.form_title}>Already have an account?</Text>
          <Textbox name="email" keyboard="email-address">
            Email
          </Textbox>
          <Textbox name="password" password>
            Password
          </Textbox>
          <Submitter>Log In</Submitter>
        </Form>
        <Form
          style={styles.form}
          area="sso"
          method="POST"
          url="/api/v1/users"
          on_success={(_, d) => {
            set_auth(new Sso(d));
            Session.auth = d;
          }}
          expect={Auth}
        >
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
  }

  return (
    <AuthContext.Provider value={auth}>{props.children}</AuthContext.Provider>
  );
};
