/*
 * This file is used to maintain all the constants
 */

import { getBaseUrl } from "../main";
import { AppCookieService } from "../services/appCookie/app-cookie.service";

export const constants = {
  defaults: {
    pageNumber: 1,
    totalRecords: 8,
    errorNumber: "NoRecordFound",
    fontSize: 20,
    minFontSize: 10,
    maxFontSize: 40
  },
  serviceUrls: {
    xsrfToken: new AppCookieService().get("XSRF-TOKEN"),
    baseUrl: getBaseUrl(),
    news: {
      fetchNews: "api/v1/news",
      fetchNewsDetail: "api/v1/news/",
      downloadNewsDetail: "api/v1/news/download",
    }
  }
}
