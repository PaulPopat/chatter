import { z } from "zod";
import UseRemoteState from "../utils/remote-state";
import UseFetcher from "../utils/fetch";

const Channel = z.object({
  ChannelId: z.string(),
  Type: z.string(),
  Name: z.string(),
});

export default function UseChannels() {
  const [channels, refresh] = UseRemoteState("/api/v1/channels", {
    method: "GET",
    expect: z.array(Channel),
    area: "server",
  });

  const create_channel = UseFetcher("/api/v1/channels", {
    method: "POST",
    area: "server",
    on_success: refresh,
  });

  return {
    channels,
    create_channel,
  };
}
