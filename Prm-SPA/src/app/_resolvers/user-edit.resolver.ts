import {Injectable} from '@angular/core';
import {User} from '../_models/user';
import {Resolve, Router, ActivatedRouteSnapshot} from '@angular/router';
import { UserService } from '../_services/user.service';
import { AlertService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from '../_services/auth.service';

@Injectable()
export class UserEditResolver implements Resolve<User> {
    constructor(private service: UserService, private router: Router,
        private alertify: AlertService, private auth: AuthService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<User> {
        return this.service.getUser(this.auth.token.nameid).pipe(
            catchError(error => {
                this.alertify.error('Problem z uzyskaniem danych do edycji użytkownika');
                this.router.navigate(['/users']);
                return of(null);
            })
        );
    }
}
