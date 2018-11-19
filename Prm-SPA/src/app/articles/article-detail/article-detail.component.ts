import { Question } from './../../_models/question';
import { Answer } from './../../_models/answer';
import { ActivatedRoute, Router } from '@angular/router';
import { ArticleService } from './../../_services/article.service';
import { Article } from './../../_models/article';
import { Component, OnInit, Input } from '@angular/core';
import { User } from '../../_models/user';
import { AuthService } from '../../_services/auth.service';
import { AlertService } from '../../_services/alertify.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-article-detail',
  templateUrl: './article-detail.component.html',
  styleUrls: ['./article-detail.component.css']
})
export class ArticleDetailComponent implements OnInit {
  @Input()
  user: User;

  article: Article;

  constructor(
    private authService: AuthService,
    private articleService: ArticleService,
    private route: ActivatedRoute,
    private alertify: AlertService,
    private router: Router
  ) {}

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.article = data['article'];
    });
  }

  checkAuthor(): boolean {
    return this.article.author.id === +this.authService.token.nameid;
  }

  checkAuthorOrAdmin(): boolean {
    const roles: String[] = ['Admin'];

    return (
      this.article.author.id === +this.authService.token.nameid ||
      this.authService.role(roles)
    );
  }

  isStudentOnList(): boolean {
    let isOnList = false;

    this.article.students.forEach(student => {
      if (student.id === +this.authService.token.nameid) {
        isOnList = true;
      }
    });

    return isOnList;
  }

  addToArticle(): void {
    this.articleService
      .addToArticle(this.authService.token.nameid, this.article.id)
      .subscribe(
        next => {
          this.alertify.success('Dziekuje za zapisanie sie na kurs');

          this.article.students.push(this.authService.current);
        },
        error => {
          this.alertify.error(error);
        }
      );
  }

  removeFromArticle(): void {
    this.articleService
      .removeFromArticle(this.authService.token.nameid, this.article.id)
      .subscribe(
        next => {
          this.alertify.success('Anulowano subskrybcje kursu');
          const index = this.article.students.findIndex((u) => u.id === +this.authService.token.nameid);
          this.article.students.splice(index, 1);
        },
        error => {
          this.alertify.error(error);
        }
      );
  }

  deleteArticle() {
    this.articleService
      .deleteArticle(this.authService.token.nameid, this.article.id)
      .subscribe(
        next => {
          this.alertify.success('artyykuł został usuniety pomyślnie');
          this.router.navigate(['/articles']);
        },
        error => {
          this.alertify.error(error);
        }
      );
  }

  setAnswer(answer: Answer, question: Question) {
    question.isSelectedAnswerCorrect = answer.isCorrect;
  }
}
