import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-news',
  templateUrl: './news.component.html'
})
export class NewsComponent {
  public newsStories: Story[];
  public showNextButton: boolean;
  public showPreviousButton: boolean;
  pageCount: number;
  currentPage: number;
  httpClient: HttpClient;
  baseUrl: string;
  searchString: string;
  pageString: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.currentPage = 1;
    this.httpClient = http;
    this.baseUrl = baseUrl;

    http.get<number>(baseUrl + 'newsfeed/pages').subscribe(result => {
      this.pageCount = result;
      this.updatePageButtons();
    }, error => console.error(error));

    http.get<Story[]>(baseUrl + 'newsfeed/stories/1').subscribe(result => {
      this.newsStories = result;
    }, error => console.error(error));
  }

  next() {
    this.currentPage++;

    this.httpClient.get<Story[]>(this.baseUrl + 'newsfeed/stories/' + this.currentPage).subscribe(result => {
      this.newsStories = result;
    }, error => console.error(error));

    this.updatePageButtons();
  }

  previous() {
    this.currentPage--;

    this.httpClient.get<Story[]>(this.baseUrl + 'newsfeed/stories/' + this.currentPage).subscribe(result => {
      this.newsStories = result;
    }, error => console.error(error));

    this.updatePageButtons();
  }

  search() {
    this.currentPage = 1;

    this.httpClient.get<number>(this.baseUrl + 'newsfeed/pages/search/' + encodeURIComponent(this.searchString)).subscribe(result => {
      this.pageCount = result;
      this.updatePageButtons();
    }, error => console.error(error));

    this.httpClient.get<Story[]>(this.baseUrl + 'newsfeed/stories/search/1/' + encodeURIComponent(this.searchString)).subscribe(result => {
      this.newsStories = result;
    }, error => console.error(error));
  }

  clearSearch() {
    this.searchString = '';
    this.currentPage = 1;

    this.httpClient.get<Story[]>(this.baseUrl + 'newsfeed/stories/1').subscribe(result => {
      this.newsStories = result;
    }, error => console.error(error));

    this.updatePageButtons();
  }

  setPageCount(searchString) {

    if (searchString) {

    } else {

    }
  }

  updatePageButtons() {
    this.showNextButton = (this.currentPage < this.pageCount);
    this.showPreviousButton = (this.currentPage > 1);
    this.pageString = 'Page ' + this.currentPage + ' of ' + this.pageCount;
  }
}

interface Story {
  title: string;
  url: string;
}
