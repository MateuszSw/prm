import { Article } from './../../_models/article';
import { AuthService } from './../../_services/auth.service';
import { AlertService } from './../../_services/alertify.service';
import { Component, OnInit } from '@angular/core';
import { ArticleService } from '../../_services/article.service';
import { ActivatedRoute, Router } from '../../../../node_modules/@angular/router';
import { Pagination, ResultPaginate } from '../../_models/pagination';

@Component({
  selector: 'app-article-list',
  templateUrl: './article-list.component.html',
  styleUrls: ['./article-list.component.css']
})
export class ArticleListComponent implements OnInit {
  articles: Article[];
  articleParams: any = { };
  pagination: Pagination;

  constructor(private articleService: ArticleService, private alertify: AlertService,
    private route: ActivatedRoute, private auth: AuthService, private router: Router) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.articles = data['articles'].score;
      this.pagination = data['articles'].pagination;
    });

    this.articleParams.showAll = 'false';
  }

  changePage(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadArticles();
  }

  resetFilters() {
    this.loadArticles();
  }

  loadArticles() {
    this.articleService.getArticles(this.auth.token.nameid,
       this.pagination.currentPage, this.pagination.itemsPerPage, this.articleParams)
      .subscribe((res: ResultPaginate<Article[]>) => {
        this.articles = res.score;
        this.pagination = res.pagination;
    }, error => {
      this.alertify.error(error);
    });
  }

  checkAuthor(article: Article): boolean {
    return article.author.id === +this.auth.token.nameid;
  }

  checkAuthorOrAdmin(article: Article): boolean {
    const roles: String[] = ['Admin'];
    return article.author.id === +this.auth.token.nameid || this.auth.role(roles);
  }

  deleteArticle(article: Article) {
    this.articleService.deleteArticle(this.auth.token.nameid, article.id).subscribe(next => {
      this.alertify.success('Article deleted successfully');
      this.loadArticles();
    }, error => {
      this.alertify.error(error);
    });
  }

}
