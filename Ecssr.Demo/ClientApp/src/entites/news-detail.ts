import { NewsDownload } from "./news-download";

export interface NewsDetail {
  id: string;
  title: string;
  detail: string;
  imageUrl: string;
  newsDownloads: NewsDownload[]
}
