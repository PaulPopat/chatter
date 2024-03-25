import { SystemMatch } from "./utils";

export default SystemMatch({
  web: () => ({
    prefer_dark: matchMedia("(prefers-color-scheme: dark)").matches,
  }),
});
