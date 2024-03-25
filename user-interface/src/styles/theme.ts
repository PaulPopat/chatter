import { StyleSheet } from "react-native";

export const Margins = 6;
export const Padding = 6;
export const BorderRadius = 8;
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
    Background: "#f6f6f6",
  },
  Primary: {
    Foreground: "white",
    Background: "#7209b7",
  },
  Secondary: {
    Foreground: "black",
    Background: "#4cc9f0",
  },
  Info: {
    Foreground: "black",
    Background: "#4361ee",
  },
  Danger: {
    Foreground: "white",
    Background: "#3a0ca3",
  },
};

const ThemeStyles: Record<string, any> = StyleSheet.create({
  ...Object.keys(Colours).reduce(
    (c, n) => ({
      ...c,
      [`colour_${n.toLowerCase()}`]: {
        backgroundColor: Colours[n as keyof typeof Colours].Background,
        color: Colours[n as keyof typeof Colours].Foreground,
      },
    }),
    {}
  ),
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
    borderRadius: 0,
  },
  border_right: {
    borderRightWidth: 2,
    borderRightColor: Colours.Body.Foreground,
    borderRadius: 0,
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
  align_top: {
    alignItems: "flex-start",
  },
  centre: {
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
  },
  max_fill: {
    maxHeight: "100%",
    overflow: "hidden",
  },
  fill: {
    height: "100%",
  },
  fill_all: {
    height: "100%",
    width: "100%",
  },
  flex_fill: {
    flex: 1,
  },
  no_overflow: {
    overflow: "hidden",
  },
  no_gap: {
    gap: 0,
  },
  modal: {
    maxWidth: 450,
    width: "100%",
    margin: "auto",
  },
  modal_background: {
    position: "absolute",
    top: 0,
    left: 0,
    width: "100%",
    height: "100%",
    backgroundColor: "rgba(0, 0, 0, 0.4)",
  },
  flush: { padding: 0 },
});

export type Class = keyof typeof ThemeStyles;

export function Classes(...classes: Array<Class | [Class, boolean]>) {
  return classes
    .map((c) => (typeof c === "string" ? ([c, true] as [Class, boolean]) : c))
    .flatMap(([c, s]) => c.split(" ").map((c) => [c, s] as [Class, boolean]))
    .reduce(
      (c, n) => ({
        ...c,
        ...(n[1] ? { ...ThemeStyles[n[0]] } : {}),
      }),
      {} as any
    );
}
