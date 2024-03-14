import { z } from "zod";
import UseRemoteState from "../utils/remote-state";

const ServerMetadata = z.object({
  ServerName: z.string(),
  Icon: z.object({
    Base64Data: z.string(),
    MimeType: z.string(),
  }),
});

export default UseRemoteState(
  "/api/v1/server/metadata",
  {
    method: "GET",
    area: "server",
    expect: ServerMetadata,
  },
  {
    update: [
      "/api/v1/server/metadata",
      {
        method: "PUT",
        area: "server",
      },
    ],
  }
);
