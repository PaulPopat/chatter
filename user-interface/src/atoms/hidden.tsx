import { PropsWithChildren, useEffect, useRef, useState } from "react";
import { UseForm } from "./form";
import { StyleSheet } from "react-native";
import {
  BorderRadius,
  BorderWidth,
  Colours,
  FontSizes,
  Margins,
  Padding,
} from "../styles/theme";

type Props = {
  name: string;
  value: string;
};

const styles = StyleSheet.create({
  view: {
    marginTop: Margins,
    marginBottom: Margins,
    borderColor: Colours.Secondary.Background,
    borderWidth: BorderWidth,
    borderRadius: BorderRadius,
  },
  viewFocus: {
    borderColor: Colours.Primary.Background,
  },
  input: {
    padding: Padding,
    fontSize: FontSizes.Label,
    paddingTop: 0,
  },
  label: {
    fontSize: FontSizes.Label,
    padding: Padding,
  },
});

export default (props: PropsWithChildren<Props>) => {
  const { value, set_value } = UseForm(props.name);

  useEffect(() => {
    set_value(props.value);
  }, [props.value]);

  return <></>;
};
