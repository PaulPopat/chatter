import { Colours, Margins } from "../styles/theme";
import { UseSubmitter } from "./form";
import { Button, StyleSheet, View } from "react-native";

type Props = {
  children: string;
};

const styles = StyleSheet.create({
  container: {
    marginTop: Margins,
    marginBottom: Margins,
  },
});

export default (props: Props) => {
  const submit = UseSubmitter();

  return (
    <View style={styles.container}>
      <Button
        title={props.children}
        color={Colours.Primary.Background}
        onPress={submit}
      />
    </View>
  );
};
