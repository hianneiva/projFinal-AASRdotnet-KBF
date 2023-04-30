import { Injectable } from '@angular/core';
import { Router, UrlTree } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { Observable } from 'rxjs';
import { TokenDecoderService } from '../service/token-decoder.service';
import { UtilsService } from '../service/utils.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard {
  private utils!: UtilsService;

  constructor(cookie: CookieService, decoder: TokenDecoderService, router: Router) {
    this.utils = new UtilsService(cookie, decoder, router);
  }

  canActivate(): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.utils.validateToken();
  }
}
