import {Injectable} from '@angular/core';
import {User} from '../_models/user';
import {Resolve, Router, ActivatedRouteSnapshot} from '@angular/router';
import { UserService } from '../_services/user.service';
import { AlertService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class UserDetailResolver implements Resolve<User> {
    constructor(private service: UserService, private router: Router,
        private alertify: AlertService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<User> {
        return this.service.getUser(route.params['id']).pipe(
            catchError(error => {
                this.alertify.error('Problem z szczeggółami na temat użytkownika');
                this.router.navigate(['/users']);
                return of(null);
            })
        );
    }
}
