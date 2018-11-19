import {Injectable} from '@angular/core';
import {Resolve, Router, ActivatedRouteSnapshot} from '@angular/router';
import { UserService } from '../_services/user.service';
import { AlertService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Message } from '../_models/message';
import { AuthService } from '../_services/auth.service';

@Injectable()
export class MessagesResolver implements Resolve<Message[]> {
    page = 1;
    size = 10;
    cointainer = 'Unread';

    constructor(private service: UserService, private router: Router,
        private alertify: AlertService, private auth: AuthService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Message[]> {
        return this.service.getMessages(this.auth.token.nameid,
              this.page, this.size, this.cointainer).pipe(
            catchError(error => {
                this.alertify.error('Problem  z danymi wiadomościami od serwera');
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }
}
