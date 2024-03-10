import { z } from "zod";
import UseRemoteState from "../utils/remote-state";

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
        body_type: z.object({ ServerUrl: z.string() }),
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
          return {
            UserName: UserName,
            Biography: Biography,
            Picture: {
              MimeType: Picture.type,
              Base64Data: await ToBase64(Picture),
            },
          };
        },
      },
    ],
  }
);
