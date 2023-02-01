import { NewsDownload } from "./news-download";

export interface DownloadDetail {
  newsDownloads: NewsDownload[],
  fileBase64: string
}
