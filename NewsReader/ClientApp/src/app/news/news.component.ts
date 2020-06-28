import { Component, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";

@Component({
  selector: "app-news",
  templateUrl: "./news.component.html"
})
export class NewsComponent {
  newsStories: IStory[];
  showNextButton: boolean;
  showPreviousButton: boolean;
  pageCount: number;
  currentPage: number;
  httpClient: HttpClient;
  searchString: string;
  pageString: string;

  constructor(http: HttpClient) {
    this.currentPage = 1;
    this.httpClient = http;
    this.loadPage();
  }

  next() {
    this.currentPage++;
    this.loadPage();
  }

  previous() {
    this.currentPage--;
    this.loadPage();
  }

  search() {
    this.currentPage = 1;
    this.loadPage();
  }

  clearSearch() {
    this.searchString = null;
    this.currentPage = 1;
    this.loadPage();
  }

  loadPage() {
    this.setPageCount();
    this.loadStories();
  }

  loadStories() {
    let url;
    if (this.searchString) {
      url = "/newsfeed/stories/search/" + this.currentPage + "/" + encodeURIComponent(this.searchString);
    } else {
      url = "/newsfeed/stories/" + this.currentPage;
    }

    this.httpClient.get<IStory[]>(url).subscribe(result => {
        this.newsStories = result;
      },
      error => console.error(error));
  }

  setPageCount() {
    let url;
    if (this.searchString) {
      url = "/newsfeed/pages/search/" + encodeURIComponent(this.searchString);
    } else {
      url = "/newsfeed/pages";
    }

    this.httpClient.get<number>(url).subscribe(result => {
        this.pageCount = result;
        this.updatePageButtons();
      },
      error => console.error(error));
  }

  updatePageButtons() {
    this.showNextButton = (this.currentPage < this.pageCount);
    this.showPreviousButton = (this.currentPage > 1);
    this.pageString = `Page ${this.currentPage} of ${this.pageCount}`;
  }
}

interface IStory {
  title: string;
  url: string;
}
