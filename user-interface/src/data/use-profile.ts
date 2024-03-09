import { z } from "zod";
import UseRemoteState from "../utils/remote-state";
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

export type Profile = z.infer<typeof Profile>;

const ProfileBody = z.object({
  UserName: z.string(),
  Biography: z.string(),
  Picture: z.instanceof(File),
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

  const join_server = UseFetcher("/api/v1/user/servers", {
    method: "POST",
    expect: z.object({ Success: z.literal(true) }),
    area: "sso",
    on_success: refresh,
  });

  const update_profile = UseFetcher("/api/v1/user/profile", {
    method: "PUT",
    expect: z.any(),
    area: "sso",
    on_success: refresh,
    async mapper(body) {
      const { UserName, Biography, Picture } = ProfileBody.parse(body);
      return {
        UserName: UserName,
        Biography: Biography,
        Picture: {
          MimeType: Picture.type,
          Base64Data: await ToBase64(Picture),
        },
      };
    },
  });

  return {
    profile,
    join_server,
    update_profile,
  };
}
