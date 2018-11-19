import { Component, OnInit } from '@angular/core';
import { AuthService } from './_services/auth.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { User } from './_models/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  jwt = new JwtHelperService();

  constructor(private auth: AuthService) {}

  ngOnInit() {
    const token = localStorage.getItem('token');
    const user: User = JSON.parse(localStorage.getItem('user'));
    if (token) {
      this.auth.token = this.jwt.decodeToken(token);
    }
    if (user) {
      this.auth.current = user;
      this.auth.changeUserPhoto(user.photoSrc);
    }
  }
}
