import { z } from "zod";
import UseRemoteState from "../utils/remote-state";
import { Fetch } from "../utils/fetch";
import { useEffect, useState } from "react";
import ServerIcon from "remixicon/icons/Device/server-line.svg";
import { ToBase64 } from "../utils/file";

const Profile = z.object({
  UserId: z.string(),
  UserName: z.string(),
  Biography: z.string(),
  RegisteredAt: z.date(),
  LastSignIn: z.date(),
  Servers: z.array(
    z.object({
      Url: z.string(),
      JoinedAt: z.date(),
    })
  ),
});

export type Profile = z.infer<typeof Profile>;

const ProfileBody = z.object({
  UserName: z.string(),
  Biography: z.string(),
  Picture: z.optional(z.instanceof(File)),
});

const DefaultImage = btoa(ServerIcon);

export function UseServerListMetadata(url: string) {
  const [metadata, set_metadata] = useState({
    ServerName: url,
    Icon: {
      Base64Data: "",
      MimeType: "",
    },
  });

  useEffect(() => {
    Fetch(
      "/api/v1/server/metadata",
      {
        method: "GET",
        area: "server",
        base_url: url,
        expect: z.object({
          ServerName: z.string(),
          Icon: z.object({
            Base64Data: z.string(),
            MimeType: z.string(),
          }),
        }),
      },
      undefined
    )
      .then(({ data }) => set_metadata(data))
      .catch((err) => console.error(err));
  }, [url]);

  return metadata;
}

export default UseRemoteState(
  "/api/v1/user/profile",
  {
    method: "GET",
    expect: Profile,
    area: "sso",
  },
  {
    join_server: [
      "/api/v1/user/servers",
      {
        method: "POST",
        expect: z.object({ Success: z.literal(true) }),
        area: "sso",
        body_type: z.object({
          ServerUrl: z.string(),
          Password: z.string(),
          ServerToken: z.string(),
        }),
      },
    ],
    update_profile: [
      "/api/v1/user/profile",
      {
        method: "PUT",
        expect: z.any(),
        area: "sso",
        async mapper(body: any) {
          const { UserName, Biography, Picture } = ProfileBody.parse(body);
          if (Picture) {
            const file = await ToBase64(Picture);
            return {
              UserName: UserName,
              Biography: Biography,
              Picture: {
                MimeType: file.mime,
                Base64Data: file.base64,
              },
            };
          }
          return {
            UserName: UserName,
            Biography: Biography,
            Picture: {
              MimeType: "",
              Base64Data: "",
            },
          };
        },
      },
    ],
  }
);
