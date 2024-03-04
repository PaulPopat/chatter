const is_date_time =
  /^(\d{4}-[01]\d-[0-3]\dT[0-2]\d:[0-5]\d:[0-5]\d\.\d+([+-][0-2]\d:[0-5]\d|Z))|(\d{4}-[01]\d-[0-3]\dT[0-2]\d:[0-5]\d:[0-5]\d([+-][0-2]\d:[0-5]\d|Z))|(\d{4}-[01]\d-[0-3]\dT[0-2]\d:[0-5]\d([+-][0-2]\d:[0-5]\d|Z)$)/gm;

export default class Json {
  static Parse(input: string) {
    return JSON.parse(input, (key, value) => {
      if (typeof value === "string" && value.match(is_date_time))
        return new Date(value);
      return value;
    });
  }

  static ToString(input: any) {
    return JSON.stringify(input, (_, v) => {
      if (v instanceof Date) return v.toISOString();
      return v;
    });
  }
}
