import { Component, OnInit } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { TokenData } from 'src/app/model/token-data';
import { TokenDecodeService } from 'src/app/services/token-decode.service';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  public personalizedMessage: string;

  constructor(private tokenDecoder: TokenDecodeService, private cookie: CookieService) {
    this.personalizedMessage = "";
  }

  ngOnInit(): void {
    const token: string = this.cookie.get(environment.cookieToken);
    const tokenData: TokenData = this.tokenDecoder.decodeToken(token);

    if (tokenData.result) {
      this.personalizedMessage = `Logado como usuário(a): ${tokenData.name}`;
    }
  }
}
