import { ActivatedRouteSnapshot, CanActivate, Router } from '@angular/router';
import { Injectable } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertService } from '../_services/alertify.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private auth: AuthService, private alertify: AlertService, private router: Router
    ) {}

  canActivate(n: ActivatedRouteSnapshot): boolean {
    const roles = n.firstChild.data['roles'] as Array<string>;
    if (roles) {
      const match = this.auth.role(roles);
      if (match) {
        return true;
      } else {
        this.router.navigate(['users']);
        this.alertify.error('Nie masz dostepu do tego zasobu ');
      }
    }

    if (this.auth.logIn()) {
      return true;
    }

    this.alertify.error('nie podałeś hasła!!!');
    this.router.navigate(['/home']);
    return false;
  }
}
