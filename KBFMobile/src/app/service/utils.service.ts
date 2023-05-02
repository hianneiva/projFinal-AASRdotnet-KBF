import { Injectable } from '@angular/core';
import { CookieService } from "ngx-cookie-service";
import { environment } from "src/environments/environment";
import { TokenData } from "../model/token-data";
import { TokenDecoderService } from "./token-decoder.service";
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class UtilsService {
  private cookie: CookieService;
  private tokenDecoder: TokenDecoderService;
  private router: Router;

  constructor(cookieSvc: CookieService, tdSvc: TokenDecoderService, route: Router) {
    this.cookie = cookieSvc;
    this.tokenDecoder = tdSvc;
    this.router = route;
  }

  public stringIsNullOrEmpty(input: string | null | undefined) {
    return input === '' || input === null || input === undefined;
  }

  public arrayFromAny<Type>(input?: Type | Type[]): Type[] {
    if (input === null || input === undefined) {
        return [];
    }
    else {
        if (Array.isArray(input)) {
            return input;
        }
        else {
            return [input];
        }
    }
  }

  public getJwtToken(): string {
    return this.cookie.get(environment.cookieToken);
  }

  public getUserDataFromToken(): TokenData {
    const token = this.getJwtToken();
    return this.tokenDecoder.decodeToken(token);
  }

  public validateToken(): boolean {
    const token: string = this.getJwtToken();

    if (this.stringIsNullOrEmpty(token)) {
        this.router.navigate(['/']);
        this.cookie.delete(environment.cookieToken);

        return false;
    }

    const data: TokenData = this.tokenDecoder.decodeToken(token);
    const currentTime: number = Date.now().valueOf() / 1000;

    if ((data.exp && data.exp! < currentTime) || (data.nbf && data.nbf > currentTime)) {
      this.router.navigate(['/']);
      this.cookie.delete(environment.cookieToken);

      return false;
    }

    return true;
  }

  public logoff() : void {
    this.cookie.delete(environment.cookieToken);
    this.router.navigate(['/']).then(() => window.location.reload());
  }
}
