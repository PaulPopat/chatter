import { z } from "zod";
import UseRemoteState from "../utils/remote-state";

export default UseRemoteState(
  "/api/v1/users",
  {
    area: "server",
    method: "GET",
    expect: z.array(
      z.object({
        UserId: z.string(),
        Admin: z.boolean(),
        Banned: z.boolean(),
      })
    ),
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
