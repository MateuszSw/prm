import { Router } from '@angular/router';
import { ArticleService } from './../../_services/article.service';
import { Article } from './../../_models/article';
import { Component, OnInit, Input } from '@angular/core';
import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { User } from '../../_models/user';
import { AuthService } from '../../_services/auth.service';
import { UserService } from '../../_services/user.service';
import { AlertService } from '../../_services/alertify.service';

@Component({
  selector: 'app-article-add',
  templateUrl: './article-add.component.html',
  styleUrls: ['./article-add.component.css']
})
export class ArticleAddComponent implements OnInit {
  @Input() user: User;

  public Editor = ClassicEditor;
  title: string;
  content: string;
  test: string;


  constructor(private auth: AuthService, private articleService: ArticleService,
     private alertify: AlertService, private router: Router) { }

  ngOnInit() {
    this.title = 'Title';
    this.content = '<p>Sample content</p>';
    this.test = 'Q1\n#A1 - correct\nA2 - not correct\nA3 - not correct\n*\nTwo plus two?\nOne\n#Four\nSix';
  }

  saveArticle() {
    this.articleService
      .createArticle(this.auth.token.nameid, this.title, this.content, this.test)
      .subscribe(next => {
      this.alertify.success('Zapisano poprawnie artykuł');
      this.router.navigate(['/articles']);
    }, error => {
      this.alertify.error(error);
    });
  }

  }


