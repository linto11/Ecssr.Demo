/*
 * This component is used to fetch the details of a particlar news.
 */

import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { constants } from '../../common/constants';
import { DownloadDetail } from '../../entites/download-detail';
import { DownloadFormat } from '../../entites/download-format';
import { NewsDetail } from '../../entites/news-detail';
import { NewsDownload } from '../../entites/news-download';
import { Success } from '../../entites/success';
import { ApiService } from '../../services/api/api.service';
import { LocalStorageService } from '../../services/localStorage/local-storage.service';

@Component({
  selector: 'app-demo-component',
  templateUrl: './news-detail.component.html',
  styleUrls: ['./news-detail.component.css']
})
export class NewsDetailComponent {
  fontSize: number = new LocalStorageService().get("fontSize", "number") != null ?
    new LocalStorageService().get("fontSize", "number") : constants.defaults.fontSize;
  minFontSize = constants.defaults.minFontSize;
  maxFontSize = constants.defaults.maxFontSize;

  id: string = '';
  message: string = '';
  fetchNewsFailed: boolean = false;
  fetchNewsSuccess: boolean = false;
  newsDetail: NewsDetail = {
    id: '',
    title: '',
    detail: '',
    imageUrl: '',
    newsDownloads: []
  }
  downloadFormat!: DownloadFormat;
  newsDownloads!: NewsDownload[];
  a4PdfDownloadCount: number = 0;
  mobilePdfDownloadCount: number = 0;
  mobileImageDownloadCount: number = 0;

  isLoading: boolean = false;
  showA4DownloadLoader: boolean = false;
  showMobilePdfDownloadLoader: boolean = false;
  showMobileImageDownloadLoader: boolean = false;

  constructor(private apiService: ApiService, private route: ActivatedRoute) { }

  async ngOnInit() {
    this.id = this.route.snapshot.params['id'];

    //check if fontsize has been manipulated
    this.fontSize = this.fontSize > constants.defaults.maxFontSize ? constants.defaults.fontSize :
      this.fontSize < constants.defaults.minFontSize ? constants.defaults.fontSize : this.fontSize;
    new LocalStorageService().set('fontSize', this.fontSize.toString());

    //fet the news detail by id
    await this.fetchNewsDetailAsync(this.id);
  }

  fetchNewsDetailAsync(id: string) {
    this.fetchNewsFailed = false;
    this.fetchNewsSuccess = false;

    this.isLoading = true;

    //call api to fetch news details
    this.apiService.fethNewsDetailAsync(id).toPromise()
      .then(
        _success => {
          const success = (_success as Success<NewsDetail>);
          if (success != undefined) {
            this.newsDetail = success.responseData as NewsDetail;
            this.newsDownloads = this.newsDetail.newsDownloads;

            this.getDownloadCounts(this.newsDownloads);
          }

          this.fetchNewsSuccess = true;
          this.isLoading = false;
        },
        _error => {
          const error = (_error.error as Error);
          if (error.message)
            this.message = error.message;
          else
            this.message = "No records found";

          this.fetchNewsFailed = true;
          this.isLoading = false;
        });
  }

  //decrease the font size
  decreaseFont() {
    this.fontSize -= 1;
    new LocalStorageService().set('fontSize', this.fontSize.toString());
  }

  //increase the font size
  increaseFont() {
    this.fontSize += 1;
    new LocalStorageService().set('fontSize', this.fontSize.toString());
  }

  //download the news based on the type of format
  downloadNews(format: string) {
    if (format === 'a4Pdf') {
      this.downloadFormat = DownloadFormat.a4Pdf;
      this.showA4DownloadLoader = true;
    }
    else if (format === 'mobilePdf') {
      this.downloadFormat = DownloadFormat.mobilePdf;
      this.showMobilePdfDownloadLoader = true;
    }
    else if (format === 'mobileImage') {
      this.downloadFormat = DownloadFormat.mobileImage;
      this.showMobileImageDownloadLoader = true;
    }

    this.apiService.downloadNewsDetail(this.id, this.downloadFormat, this.fontSize).toPromise().then(
      _success => {
        const success = (_success as Success<DownloadDetail>);
        if (success != undefined) {
          this.newsDownloads = success.responseData.newsDownloads as NewsDownload[];

          //parse and get the updated download count
          this.getDownloadCounts(this.newsDownloads);

          let extension: string = ".pdf";
          let applicationType: string = "application/pdf";
          if (this.downloadFormat == DownloadFormat.mobileImage) {
            extension = ".jpg";
            applicationType = "application/jpg";
          }

          //download the file
          this.downloadFile(success.responseData.fileBase64, this.id + extension, applicationType);

          this.showA4DownloadLoader = false;
          this.showMobileImageDownloadLoader = false;
          this.showMobilePdfDownloadLoader = false;
        }
      },
      _error => {
        const error = (_error.error as Error);
        if (error.message)
          this.message = error.message;
        else
          this.message = "No records found";

        this.showA4DownloadLoader = false;
        this.showMobileImageDownloadLoader = false;
        this.showMobilePdfDownloadLoader = false;
      });
  }

  //function to download the file
  downloadFile(pdfBase64: string, fileName: string, applicationType: string) {
    const binary = atob(pdfBase64.replace(/\s/g, ''));
    const array = [];
    for (let i = 0; i < binary.length; i++) {
      array.push(binary.charCodeAt(i));
    }
    const blob = new Blob([new Uint8Array(array)], { type: applicationType });
    const url = window.URL.createObjectURL(blob);

    const a = document.createElement('a');
    a.style.display = 'none';
    a.href = url;
    a.download = fileName;
    document.body.appendChild(a);
    a.click();

    // rewoke URL after 15 minutes
    setTimeout(() => {
      window.URL.revokeObjectURL(url);
    }, 15 * 60 * 1000);
  }

  //this function is display the download counts based on different format
  getDownloadCounts(newsDownloads: NewsDownload[]) {
    if (newsDownloads) {
      let hasA4Download = this.newsDownloads.filter(nd => nd.downloadFormat as string === DownloadFormat.a4Pdf)[0];
      if (hasA4Download)
        this.a4PdfDownloadCount = hasA4Download.count;
      else
        this.a4PdfDownloadCount = 0;

      let hasMobileDownload = this.newsDownloads.filter(nd => nd.downloadFormat === DownloadFormat.mobilePdf)[0];
      if (hasMobileDownload)
        this.mobilePdfDownloadCount = hasMobileDownload.count;
      else
        this.mobilePdfDownloadCount = 0;

      let hasImageDownload = this.newsDownloads.filter(nd => nd.downloadFormat === DownloadFormat.mobileImage)[0];
      if (hasImageDownload)
        this.mobileImageDownloadCount = hasImageDownload.count;
      else
        this.mobileImageDownloadCount = 0;
    }
    else {
      this.a4PdfDownloadCount = 0;
      this.mobilePdfDownloadCount = 0;
      this.mobileImageDownloadCount = 0;
    }
  }

  //function to check if image url is null or not.and if null, show default image
  getImage(url: string) {
    return url ? url : 'assets/img/not_found.jpg';
  }
}
