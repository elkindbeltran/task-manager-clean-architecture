import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler
} from '@angular/common/http';

import { AuthService } from '@auth0/auth0-angular';

import { switchMap } from 'rxjs/operators';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private auth: AuthService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler) {

    if (!req.url.startsWith(environment.apiUrl)) {

      return next.handle(req);
    }

    return this.auth.getAccessTokenSilently()
      .pipe(
        switchMap(token => {
          const authReq = req.clone({
            setHeaders: {
              Authorization: `Bearer ${token}`
            }
          });
          return next.handle(authReq);
        })
      );
  }
}