import { News } from "./news";
import { Pagination } from "./pagination";

export interface NewsList {
  news: News[],
  pagination: Pagination
}
