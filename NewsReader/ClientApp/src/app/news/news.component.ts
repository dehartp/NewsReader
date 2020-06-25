import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-news',
  templateUrl: './news.component.html'
})
export class NewsComponent {
  public newsStories: Story[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Story[]>(baseUrl + 'newsfeed').subscribe(result => {
      this.newsStories = result;
    }, error => console.error(error));
  }
}

interface Story {
  title: string;
  url: string;
}
