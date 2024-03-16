import { StyleSheet, ViewStyle } from "react-native";

export const Margins = 6;
export const Padding = 6;
export const BorderRadius = 4;
export const BorderRadiusLarge = 12;
export const BorderWidth = 2;

export const FontSizes = {
  Title: 24,
  Label: 16,
  Small: 9,
};

export const Colours = {
  Body: {
    Foreground: "black",
    Background: "white",
  },
  Highlight: {
    Foreground: "#333",
    Background: "#e5e5e5",
  },
  Primary: {
    Foreground: "white",
    Background: "#14213d",
  },
  Secondary: {
    Foreground: "white",
    Background: "black",
  },
  Danger: {
    Foreground: "black",
    Background: "#fca311",
  },
};

const ThemeStyles = StyleSheet.create({
  colour_body: {
    backgroundColor: Colours.Body.Background,
    color: Colours.Body.Foreground,
  },
  colour_highlight: {
    backgroundColor: Colours.Highlight.Background,
    color: Colours.Highlight.Foreground,
  },
  card: {
    shadowRadius: 12,
    shadowColor: "#222",
    shadowOpacity: 0.3,
    borderWidth: BorderWidth,
    borderRadius: BorderRadius,
  },
  bordered: {
    borderWidth: 2,
    borderColor: Colours.Body.Foreground,
  },
  border_bottom: {
    borderBottomWidth: 2,
    borderBottomColor: Colours.Body.Foreground,
    marginBottom: Margins,
  },
  border_right: {
    borderRightWidth: 2,
    borderRightColor: Colours.Body.Foreground,
    marginRight: Margins,
  },
  container: {
    padding: Padding,
    borderRadius: BorderRadius,
  },
  edge_container: {
    padding: Padding,
  },
  highlight: {
    padding: Padding,
    backgroundColor: Colours.Highlight.Background,
    borderRadius: BorderRadius,
  },
  spacer: {
    margin: Margins,
  },
  body_text: {
    fontSize: FontSizes.Label,
  },
  important_text: {
    fontSize: FontSizes.Label,
    fontWeight: "bold",
  },
  small_text: {
    fontSize: FontSizes.Small,
  },
  title: {
    fontSize: FontSizes.Title,
  },
  row: {
    flexDirection: "row",
    alignItems: "center",
    gap: Margins,
  },
  column: {
    flexDirection: "column",
    gap: Margins,
  },
  centre: {
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
  },
  max_fill: {
    maxHeight: "100%",
  },
  fill: {
    height: "100%",
    width: "100%"
  },
  flex_fill: {
    flex: 1,
  },
  no_overflow: {
    overflow: "hidden",
  },
});

export type Class = keyof typeof ThemeStyles;

export function Classes(...classes: Array<Class | [Class, boolean]>) {
  return classes.reduce(
    (c, n) =>
      typeof n === "string"
        ? {
            ...c,
            ...ThemeStyles[n],
          }
        : {
            ...c,
            ...(n[1] ? { ...ThemeStyles[n[0]] } : {}),
          },
    {} as ViewStyle
  );
}
