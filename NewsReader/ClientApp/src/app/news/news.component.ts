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
  url: string;


  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.currentPage = 1;
    this.httpClient = http;
    this.url = baseUrl;

    http.get<Story[]>(baseUrl + 'newsfeed/stories/1').subscribe(result => {
      this.newsStories = result;
    }, error => console.error(error));

    http.get<number>(baseUrl + 'newsfeed/pages').subscribe(result => {
      this.pageCount = result;
      this.updateButtonVisibility();
    }, error => console.error(error));
  }

  next() {
    this.currentPage++;

    this.httpClient.get<Story[]>(this.url + 'newsfeed/stories/' + this.currentPage).subscribe(result => {
      this.newsStories = result;
    }, error => console.error(error));

    this.updateButtonVisibility();
  }

  previous() {
    this.currentPage--;

    this.httpClient.get<Story[]>(this.url + 'newsfeed/stories/' + this.currentPage).subscribe(result => {
      this.newsStories = result;
    }, error => console.error(error));

    this.updateButtonVisibility();
  }

  search() {
    this.httpClient.get<Story[]>(this.url + 'newsfeed/stories/search/1/' + encodeURIComponent('minute')).subscribe(result => {
      this.newsStories = result;
    }, error => console.error(error));
  }

  updateButtonVisibility() {
    this.showNextButton = (this.currentPage < this.pageCount);
    this.showPreviousButton = (this.currentPage > 1);
  }
}

interface Story {
  title: string;
  url: string;
}
