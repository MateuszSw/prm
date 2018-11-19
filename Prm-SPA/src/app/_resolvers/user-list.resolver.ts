import {Injectable} from '@angular/core';
import {User} from '../_models/user';
import {Resolve, Router, ActivatedRouteSnapshot} from '@angular/router';
import { UserService } from '../_services/user.service';
import { AlertService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class UserListResolver implements Resolve<User[]> {
    page = 1;
    size = 10;

    constructor(private service: UserService, private router: Router,
        private alertify: AlertService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<User[]> {
        return this.service.getUsers(this.page, this.size).pipe(
            catchError(error => {
                this.alertify.error('Problem z danymi');
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }
}
