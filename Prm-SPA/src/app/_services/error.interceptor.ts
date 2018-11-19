import {Injectable} from '@angular/core';
import { Observable, throwError } from 'rxjs';
import {HttpInterceptor, HttpRequest, HTTP_INTERCEPTORS, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { catchError } from 'rxjs/operators';

@Injectable()
export class Error implements HttpInterceptor {
    intercept(r: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(r).pipe(
            catchError(e => {
                if (e instanceof HttpErrorResponse) {
                    if (e.status === 401) {
                        return throwError(e.statusText);
                    }
                    const appError = e.headers.get('Application-Error');
                    if (appError) {
                        console.error(appError);
                        return throwError(appError);
                    }
                    const server = e.error;
                    let modal = '';
                    if (server && typeof server === 'object') {
                        for (const key in server) {
                            if (server[key]) {
                                modal += server[key] + '\n';
                            }
                        }
                    }
                    return throwError(modal || server || 'Błąd Servera');
                }
            })
        );
    }
}

export const ErrorProvider = {
    provide: HTTP_INTERCEPTORS,
    use: Error,
    multi: true
};
