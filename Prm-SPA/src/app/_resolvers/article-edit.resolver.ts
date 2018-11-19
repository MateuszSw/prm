import {Injectable} from '@angular/core';
import {Resolve, Router, ActivatedRouteSnapshot} from '@angular/router';
import { ArticleService } from '../_services/article.service';
import { AlertService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Article } from '../_models/article';
import { AuthService } from '../_services/auth.service';

@Injectable()
export class ArticleResolver implements Resolve<Article[]> {

    constructor(private articleService: ArticleService, private router: Router,
        private alertify: AlertService, private auth: AuthService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Article[]> {
        return this.articleService.getArticle(this.auth.token.nameid, route.params['id']).pipe(
            catchError(error => {
                this.alertify.error('Problem z danymi artykułu');
                this.router.navigate(['/articles']);
                return of(null);
            })
        );
    }
}
