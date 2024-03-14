type StringFile = {
  base64: string;
  mime: string;
};

export function ToBase64(file: File) {
  return new Promise<StringFile>((res, rej) => {
    const reader = new FileReader();
    reader.onload = () => {
      if (typeof reader.result !== "string")
        throw new Error("Could not read file");

      const data_url = reader.result;

      const just_data = data_url.replace(
        /^data:[a-z0-9A-Z\-]+\/[a-z0-9A-Z\-]+;[a-z0-9A-Z\-]+,/,
        ""
      );
      res({ base64: just_data, mime: file.type });
    };

    reader.onerror = (error) => {
      rej(error);
    };

    reader.readAsDataURL(file);
  });
}
