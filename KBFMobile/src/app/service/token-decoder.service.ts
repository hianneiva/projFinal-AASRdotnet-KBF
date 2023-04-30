import { Injectable } from '@angular/core';
import jwtDecode from 'jwt-decode';
import { TokenData } from '../model/token-data';

@Injectable({
  providedIn: 'root'
})
export class TokenDecoderService {
  constructor() { }

  public decodeToken(token: string): TokenData {
    let failure: TokenData = new TokenData();
    failure.result = false;

    try {
      let tokenData: TokenData = jwtDecode<TokenData>(token);
      tokenData.result = true;

      return tokenData;
    } catch (e) {
      return failure;
    }
  }
}
