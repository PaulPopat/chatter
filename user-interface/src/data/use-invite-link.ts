import { z } from "zod";
import UseRemoteState from "../utils/remote-state";

const InviteLinkBody = z.object({
  publicUrl: z.string(),
  embedpassword: z.union([z.literal("true"), z.literal("false")]),
  admin: z.union([z.literal("true"), z.literal("false")]),
});

const Response = z.object({
  Url: z.string(),
});

export default UseRemoteState(
  "/api/v1/server/invite-link",
  {
    method: "GET",
    area: "server",
    body_type: InviteLinkBody,
    expect: Response,
  },
  {}
);
