export default function JoinUrl(...parts: Array<string>) {
  return parts.reduce(
    (c, n) =>
      (c && !c.endsWith("/") ? c + "/" : c) +
      (n.startsWith("/") ? n.replace("/", "") : n),
    ""
  );
}
