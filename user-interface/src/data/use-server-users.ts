import { z } from "zod";
import UseRemoteState from "../utils/remote-state";

const ServerUser = z.object({
  UserId: z.string(),
  Admin: z.boolean(),
});

export type ServerUser = z.infer<typeof ServerUser>;

export default UseRemoteState(
  "/api/v1/users",
  {
    area: "server",
    method: "GET",
    expect: z.array(ServerUser),
  },
  {
    give_admin: [
      "/api/v1/admin-users",
      {
        method: "POST",
        area: "server",
        body_type: z.object({ UserId: z.string() }),
      },
    ],
    revoke_admin: [
      "/api/v1/admin-users/:user_id",
      {
        method: "DELETE",
        area: "server",
        body_type: z.object({ user_id: z.string() }),
      },
    ],
    ban: [
      "/api/v1/banned-users",
      {
        method: "POST",
        area: "server",
        body_type: z.object({ UserId: z.string() }),
      },
    ],
  }
);
