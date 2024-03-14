import { useEffect, useState } from "react";
import { z } from "zod";
import Url from "../utils/url";
import UseFetcher from "../utils/fetch";
import UseServer from "./use-server";
import Json from "../utils/json";

export type Message = {
  Text: string;
  Who: string;
  When: Date;
};

const Backlog = z.array(
  z.object({
    Text: z.string(),
    Who: z.string(),
    When: z.date(),
  })
);

const ChannelNotification = z.object({
  Text: z.string(),
  Who: z.string(),
  When: z.date(),
  Type: z.literal("Message"),
});

const SendBody = z.object({ Text: z.string(), channel_id: z.string() });

export default function UseChat(channel_id: string) {
  const [messages, set_messages] = useState<Array<Message>>([]);
  const [socket, set_socket] = useState<WebSocket | null>(null);
  const server = UseServer();

  useEffect(() => {
    if (!server?.BaseUrl) return;
    if (socket) return;

    const url = new Url(
      "/ws/chat/:channel_id",
      { channel_id, token: server.LocalToken },
      true
    );

    const connection = new WebSocket(
      url.href(server.BaseUrl).replace("http", "ws")
    );

    connection.onmessage = (event) => {
      const data = event.data;
      if (typeof data !== "string") throw new Error("Invalid message data");

      const notification = ChannelNotification.parse(Json.Parse(data));

      switch (notification.Type) {
        case "Message":
          set_messages((m) => [
            {
              Text: notification.Text,
              Who: notification.Who,
              When: notification.When,
            },
            ...m,
          ]);
          break;
        default:
          throw new Error("Unknown message type");
      }
    };

    set_socket(connection);

    connection.onclose = (event) => {
      set_socket(null);
    };

    return () => {
      connection.close();
      set_socket(null);
    };
  }, [server, channel_id, socket]);

  const backlog_fetcher = UseFetcher("/api/v1/channels/:channel_id/messages", {
    method: "GET",
    area: "server",
    on_success(_, data) {
      const backlog = Backlog.parse(data);
      set_messages((m) => [...m, ...backlog]);
    },
  });

  const fetch_more = () =>
    backlog_fetcher({ channel_id, offset: messages.length.toString() });

  useEffect(() => {
    set_messages([]);
    fetch_more();
  }, [channel_id]);

  return {
    state: messages,
    actions: {
      fetch_more,
      send: UseFetcher("/api/v1/channels/:channel_id/messages", {
        method: "POST",
        area: "server",
        body_type: SendBody,
        cachable: false,
      }),
    },
  };
}
