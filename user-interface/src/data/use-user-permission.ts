import { z } from "zod";
import UseRemoteState from "../utils/remote-state";

export default UseRemoteState(
  "/api/v1/users/:user_id/permissions",
  {
    area: "server",
    method: "GET",
    body_type: z.object({ user_id: z.string() }),
  },
  {
    add_user_to_channel: [
      "/api/v1/channels/:channel_id/users",
      {
        method: "POST",
        area: "server",
        body_type: z.object({
          UserId: z.string(),
          AllowWrite: z.boolean(),
          channel_id: z.string(),
        }),
      },
    ],
    kick_user_from_channel: [
      "/api/v1/channels/:channel_id/users/:user_id",
      {
        method: "DELETE",
        area: "server",
        body_type: z.object({ user_id: z.string(), channel_id: z.string() }),
      },
    ],
  }
);
