import { z } from "zod";
import UseRemoteState from "../utils/remote-state";
import { ToBase64 } from "../utils/file";

const ServerMetadata = z.object({
  ServerName: z.string(),
  Icon: z.object({
    Base64Data: z.string(),
    MimeType: z.string(),
  }),
});

const ConfigUpdate = z.object({
  ServerName: z.string(),
  Icon: z.instanceof(File),
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
        mapper: async (data: unknown) => {
          const parsed = ConfigUpdate.parse(data);

          const file = await ToBase64(parsed.Icon);
          return {
            ServerName: parsed.ServerName,
            IconBase64: file.base64,
            IconMimeType: file.mime,
          };
        },
      },
    ],
  }
);
