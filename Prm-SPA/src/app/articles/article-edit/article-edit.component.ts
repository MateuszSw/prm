import { ActivatedRoute, Router } from '@angular/router';
import { ArticleService } from './../../_services/article.service';
import { Article } from './../../_models/article';
import { Component, OnInit, Input } from '@angular/core';
import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { User } from '../../_models/user';
import { AuthService } from '../../_services/auth.service';
import { UserService } from '../../_services/user.service';
import { AlertService } from '../../_services/alertify.service';

@Component({
  selector: 'app-article-edit',
  templateUrl: './article-edit.component.html',
  styleUrls: ['./article-edit.component.css']
})
export class ArticleEditComponent implements OnInit {
  @Input() user: User;

  public Editor = ClassicEditor;
  article: Article;

  constructor(private auth: AuthService, private articleService: ArticleService,
    private route: ActivatedRoute, private alertify: AlertService, private router: Router) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.article = data['article'];
    });
  }

  saveArticle() {
    this.articleService.editArticle(this.auth.token.nameid, this.article).subscribe(next => {
      this.alertify.success('Kursy został edytowany poprawnie');
      this.router.navigate(['/articles/' + this.article.id]);
    }, error => {
      this.alertify.error(error);
    });
  }

  }


