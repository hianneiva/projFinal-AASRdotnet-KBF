import { Injectable } from '@angular/core';
import jwtDecode from 'jwt-decode';
import moment from 'moment';
import { environment } from 'src/environments/environment.development';
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
      tokenData.expirationDatetime = moment(tokenData.ExpectedExpiration!, "YYYY-MM-DD HH:mm:ss.SSSSSSS zzz").toDate();

      return tokenData;
    } catch (e) {
      return failure;
    }
  }
}
