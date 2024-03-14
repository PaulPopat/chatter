import { useEffect, useState } from "react";
import { z } from "zod";
import UseFetcher from "../utils/fetch";

const PublicProfile = z.object({
  UserId: z.string(),
  UserName: z.string(),
  Biography: z.string(),
});

type PublicProfile = z.infer<typeof PublicProfile>;

const Profiles: Record<string, Promise<PublicProfile> | undefined> = {};

export default function UsePublicProfile(user_id: string) {
  const [profile, set_profile] = useState<PublicProfile | undefined>();

  const fetcher = UseFetcher("/api/v1/users/:user_id/profile", {
    method: "GET",
    area: "sso",
    no_auth: true,
    expect: PublicProfile,
  });

  useEffect(() => {
    let existing = Profiles[user_id];
    if (!existing) {
      existing = fetcher({ user_id }).then((r) => r.data);
      Profiles[user_id] = existing;
    }

    existing.then(set_profile);
  }, [user_id]);

  return profile;
}
