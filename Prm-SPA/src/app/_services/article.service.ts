import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Article } from '../_models/article';
import { ResultPaginate } from '../_models/pagination';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ArticleService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getArticles(userId: number, page?, itemsPerPage?, articleParams?): Observable<ResultPaginate<Article[]>> {
    const paginatedResult: ResultPaginate<Article[]> = new ResultPaginate<Article[]>();

    let params = new HttpParams();

    if (page != null && itemsPerPage != null) {
      params = params.append('page', page);
      params = params.append('size', itemsPerPage);
    }

    if (articleParams != null) {
       params = params.append('showAll', articleParams.showAll);
    }


    return this.http.get<Article[]>(this.baseUrl + 'article/' + userId + '/GetArticles', { observe: 'response', params})
      .pipe(
        map(response => {
          paginatedResult.score = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return paginatedResult;
        })
      );
  }

  getArticle(userId: number, articleId: number): Observable<Article> {
    let params = new HttpParams();

      params = params.append('id', articleId.toString());


    return this.http.get<Article>(this.baseUrl + 'article/' + userId + '/GetArticle', { params });
  }


  createArticle(userId: number, title: string, content: string, test: string) {
    let params = new HttpParams();

      params = params.append('title', title);
      params = params.append('content', content);
      params = params.append('test', test);

     return this.http.post(this.baseUrl + 'article/' + userId + '/CreateArticle', params);
  }

  editArticle(userId: number, article: Article) {
    return this.http.post(this.baseUrl + 'article/' + userId + '/EditArticle', article);
  }

  deleteArticle(userId: number, articleId: number): Observable<Article> {
    let params = new HttpParams();

    params = params.append('id', articleId.toString());


    return this.http.get<Article>(this.baseUrl + 'article/' + userId + '/DeleteArticle', { params });
  }

  addToArticle(userId: number, articleId: number) {
    let params = new HttpParams();

    params = params.append('id', articleId.toString());

    return this.http.post(this.baseUrl + 'article/' + userId + '/AddToArticle', params );
  }

  removeFromArticle(userId: number, articleId: number) {
    let params = new HttpParams();

    params = params.append('id', articleId.toString());

    return this.http.post(this.baseUrl + 'article/' + userId + '/RemoveFromArticle',  params );
  }
}
