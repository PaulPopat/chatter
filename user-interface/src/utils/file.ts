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
      res({ base64: reader.result, mime: file.type });
    };

    reader.onerror = (error) => {
      rej(error);
    };

    reader.readAsDataURL(file);
  });
}
