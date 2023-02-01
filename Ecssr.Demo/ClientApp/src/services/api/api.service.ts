/*
 * This service file is a helper file to call REST APIs.
 */

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { constants } from '../../common/constants';
import { Dictionary } from '../../entites/dictionary';
import { DownloadFormat } from '../../entites/download-format';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  getParameters: Dictionary[] = [];

  constructor(private http: HttpClient) { }

  //commom function to generate query parameters using array
  generateQueryParam(dictionary: Dictionary[]) {
    let queryString: string = "?";
    if (dictionary !== undefined && dictionary.length > 0) {
      for (let parameterIndex = 0; parameterIndex < dictionary.length; parameterIndex++) {
        if (queryString !== "?")
          queryString += "&";

        let parameter: Dictionary = dictionary[parameterIndex];
        queryString += parameter.key + "=" + parameter.value
      }
    }

    return queryString === "?" ? "" : queryString;
  }

  //fetch news function 
  fethNewsAsync(totalRecords: number, pageNumber: number) {
    this.getParameters = [];

    this.getParameters.push({
      key: 'totalRecords',
      value: totalRecords.toString()
    });
    this.getParameters.push({
      key: 'pageNumber',
      value: pageNumber.toString()
    });
    const httpOptions = {
      headers: new HttpHeaders({
        "X-XSRF-TOKEN": constants.serviceUrls.xsrfToken || ""
      })
    };
    return this.http.get(constants.serviceUrls.news.fetchNews + this.generateQueryParam(this.getParameters), httpOptions);
  }

  //fetch news detail function
  fethNewsDetailAsync(id: string) {
    const httpOptions = {
      headers: new HttpHeaders({
        "X-XSRF-TOKEN": constants.serviceUrls.xsrfToken || ""
      })
    };
    return this.http.get(constants.serviceUrls.news.fetchNewsDetail + id, httpOptions);
  }

  //download news detail function
  downloadNewsDetail(id: string, downloadFormat: DownloadFormat, fontSize: number) {
    const httpOptions = {
      headers: new HttpHeaders({
        "X-XSRF-TOKEN": constants.serviceUrls.xsrfToken || ""
      })
    };
    return this.http.post(constants.serviceUrls.news.downloadNewsDetail, {
      id: id,
      downloadFormat: downloadFormat,
      fontSize: fontSize
    }, httpOptions);
  }
}
