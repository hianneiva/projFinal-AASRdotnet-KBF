import { Injectable } from '@angular/core';
import jwtDecode from 'jwt-decode';
import { TokenData } from '../model/token-data';

@Injectable({
  providedIn: 'root'
})
export class TokenDecodeService {
  constructor() { }

  public decodeToken(token: string): TokenData {
    // const secret = btoa(environment.secret);
    let failure: TokenData = new TokenData();
    failure.result = false;

    /*
    if (!token.includes(secret)) {
      return failure;
    }
     */

    try {
      let tokenData: TokenData = jwtDecode<TokenData>(token);
      tokenData.result = true;

      return tokenData;
    } catch (e) {
      return failure;
    }
  }
}
