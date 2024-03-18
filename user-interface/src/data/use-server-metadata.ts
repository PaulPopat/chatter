import { z } from "zod";
import UseRemoteState from "../utils/remote-state";
import { ToBase64 } from "../utils/file";
import Asset from "../utils/asset";

const ServerMetadata = z.object({
  ServerName: z.string(),
  Icon: z.object({
    Base64Data: z.string(),
    MimeType: z.string(),
  }),
});

const ConfigUpdate = z.object({
  ServerName: z.string(),
  Icon: z.optional(z.instanceof(Asset)),
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
          const file_dto = await parsed.Icon?.DataTransferObject();
          if (file_dto && file_dto.Encoding !== "base64")
            throw new Error("Currently, only base64 assets are supported");

          return {
            ServerName: parsed.ServerName,
            IconBase64: file_dto?.Data,
            IconMimeType: file_dto?.Mime,
          };
        },
      },
    ],
  }
);
