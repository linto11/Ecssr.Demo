/*
 * This component is used to fetch the list of all the news list.
 */

import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { constants } from '../../common/constants';
import { Utility } from '../../common/utility/utility';
import { Error } from '../../entites/error';
import { News } from '../../entites/news';
import { NewsList } from '../../entites/news-list';
import { Pagination } from '../../entites/pagination';
import { Success } from '../../entites/success';
import { ApiService } from '../../services/api/api.service';

@Component({
  selector: 'app-demo-component',
  templateUrl: './news-list.component.html',
  styleUrls: ['./news-list.component.css']
})
export class NewsListComponent implements OnInit {
  fetchNewsFailed: boolean = false;
  fetchNewsSuccess: boolean = false;

  message: string = '';
  news: News[] = [{ brief: '', id: '', imageUrl: '', title: '' }];
  pagination: Pagination = {
    currentPage: 0,
    pageCount: 0,
    pageSize: 0,
    rowCount: 0
  };
  pageNumbers: number[] = [];
  currentPageNumber: number = -1;
  isPreviousClicked: boolean = false;
  isNextClicked: boolean = false;
  isLoading: boolean = false;

  constructor(private apiService: ApiService, private router: Router) { }

  async ngOnInit() {
    //fetch all the list of news from DB
    await this.fetchNewsAsync(constants.defaults.totalRecords, constants.defaults.pageNumber);
  }

  fetchNewsAsync(totalRecords: number, pageNumber: number) {
    this.fetchNewsFailed = false;
    this.fetchNewsSuccess = false;
    this.pageNumbers = [];

    this.isLoading = true;

    //call api to fetch news by pagination
    this.apiService.fethNewsAsync(totalRecords, pageNumber).toPromise()
      .then(
        _success => {
          const success = (_success as Success<NewsList>);
          if (success != undefined) {
            this.news = success.responseData.news as News[];
            this.pagination = success.responseData.pagination as Pagination;
            this.pageNumbers = new Utility().createRange(this.pagination.pageCount);

            this.currentPageNumber = this.pagination.currentPage;
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

  //click event to navigate previous page
  previousPage() {
    this.fetchNewsAsync(constants.defaults.totalRecords, this.currentPageNumber - 1);
  }

  //click event to navigate next page
  nextPage() {
    this.fetchNewsAsync(constants.defaults.totalRecords, this.currentPageNumber + 1);
  }

  //change even to open a specific page
  openPage(selectedPageNumber: number) {
    this.fetchNewsAsync(constants.defaults.totalRecords, selectedPageNumber);
  }

  //even to navigate to details component
  showDetails(id: string) {
    this.router.navigate(['/news-detail', id]); 
  }

  //function to check if image url is null or not.and if null, show default image
  getImage(url: string) {
    return url ? url : 'assets/img/not_found.jpg';
  }
}
