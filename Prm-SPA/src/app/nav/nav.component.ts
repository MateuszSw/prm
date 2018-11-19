import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertService } from '../_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  photoSrc: string;

  constructor(public auth: AuthService, private alertify: AlertService,
      private router: Router) { }

  ngOnInit() {
    this.auth.currentPhoto.subscribe(photoSrc => this.photoSrc = photoSrc);
  }

  login() {
    this.auth.login(this.model).subscribe(next => {
      this.alertify.success('Logowanie przebiegło pomyślnie');
    }, error => {
      this.alertify.error(error);
    }, () => {
      this.router.navigate(['/articles']);
    });
  }

  logIn() {
    const token = localStorage.getItem('token');
    return !!token;
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.auth.token = null;
    this.auth.current = null;
    this.alertify.message('wylogowano użytkownika');
    this.router.navigate(['/home']);
  }

}
