export type AssetDto = {
  Data: string;
  Mime: string;
  Encoding: string;
};

export default abstract class Asset {
  abstract get Uri(): string;

  abstract DataTransferObject(): Promise<AssetDto>;
}
