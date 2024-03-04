import { z } from "zod";
import UseRemoteState from "../utils/remote-state";
import { useCallback } from "react";
import UseFetcher from "../utils/fetch";

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

function ToBase64(file: File) {
  return new Promise<string>((res, rej) => {
    const reader = new FileReader();
    reader.onload = () => {
      if (typeof reader.result !== "string")
        throw new Error("Could not read file");
      res(reader.result);
    };

    reader.onerror = (error) => {
      rej(error);
    };

    reader.readAsDataURL(file);
  });
}

export default function UseProfile() {
  const [profile, refresh] = UseRemoteState("/api/v1/user/profile", {
    method: "GET",
    expect: Profile,
    area: "sso",
  });

  const join_server_fetcher = UseFetcher("/api/v1/user/servers", {
    method: "POST",
    expect: z.object({ Success: z.literal(true) }),
    area: "sso",
  });
  const join_server = useCallback(
    async (server_url: string) => {
      await join_server_fetcher({ ServerUrl: server_url });
      refresh();
    },
    [join_server_fetcher]
  );

  const update_profile_fetcher = UseFetcher("/api/v1/user/profile", {
    method: "PUT",
    expect: z.any(),
    area: "sso",
  });
  const update_profile = useCallback(
    async (user_name: string, biography: string, picture: File) => {
      await update_profile_fetcher({
        UserName: user_name,
        Biography: biography,
        Picture: {
          MimeType: picture.type,
          Base64Data: await ToBase64(picture),
        },
      });
      refresh();
    },
    [join_server_fetcher]
  );

  return {
    profile,
    join_server,
    update_profile,
  };
}
