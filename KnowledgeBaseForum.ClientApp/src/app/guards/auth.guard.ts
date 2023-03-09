import { Injectable } from '@angular/core';
import { Router, UrlTree } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { Observable } from 'rxjs';
import { TokenDecodeService } from '../services/token-decode.service';
import { Utils } from '../utils/utils';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard {
  private utils!: Utils;

  constructor(cookie: CookieService, decoder: TokenDecodeService, router: Router) {
    this.utils = new Utils(cookie, decoder, router);
  }

  canActivate(): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.utils.validateToken();
  }
}
