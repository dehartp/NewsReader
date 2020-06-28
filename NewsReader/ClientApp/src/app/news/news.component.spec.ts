import { TestBed } from '@angular/core/testing';
import { NewsComponent } from './news.component';
import {
  HttpClientTestingModule,
  HttpTestingController
} from '@angular/common/http/testing';

describe('NewsComponent', () => {
  let httpTestingController: HttpTestingController;
  let comp: NewsComponent;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [NewsComponent],
      imports: [HttpClientTestingModule]
    });

    httpTestingController = TestBed.get(HttpTestingController);
    comp = TestBed.get(NewsComponent);
  });

 it('should be created', () => {
    expect(comp).toBeTruthy();
  });

  it('should get pages on init', () => {
    const req = httpTestingController.expectOne('/newsfeed/pages');
    expect(req.request.method).toEqual("GET");
  });

  it('should get stories on init', () => {
    const req = httpTestingController.expectOne('/newsfeed/stories/1');
    expect(req.request.method).toEqual("GET");
  });

  it('should get next page on next', () => {
    comp.next();
    const req = httpTestingController.expectOne('/newsfeed/stories/2');
    expect(req.request.method).toEqual("GET");
  });

  it('should get previous page on previous', () => {
    comp.currentPage = 5;
    comp.previous();
    const req = httpTestingController.expectOne('/newsfeed/stories/4');
    expect(req.request.method).toEqual("GET");
  });

  it('should get pages on search', () => {
    comp.searchString = 'srch';
    comp.search();
    const req = httpTestingController.expectOne('/newsfeed/pages/search/srch');
    expect(req.request.method).toEqual("GET");
  });

  it('should get stories on search', () => {
    comp.searchString = 'srch';
    comp.search();
    const req = httpTestingController.expectOne('/newsfeed/stories/search/1/srch');
    expect(req.request.method).toEqual("GET");
  });

  it('should get next page on search next', () => {
    comp.searchString = 'srch';
    comp.search();
    comp.next();
    const req = httpTestingController.expectOne('/newsfeed/stories/search/2/srch');
    expect(req.request.method).toEqual("GET");
  });

  it('should get next page on search next', () => {
    comp.searchString = 'srch';
    comp.search();
    comp.currentPage = 5;
    comp.previous();
    const req = httpTestingController.expectOne('/newsfeed/stories/search/4/srch');
    expect(req.request.method).toEqual("GET");
  });

});
