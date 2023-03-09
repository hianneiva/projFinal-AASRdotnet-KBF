import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { TokenData } from 'src/app/model/token-data';
import { TokenDecodeService } from 'src/app/services/token-decode.service';
import { Utils } from 'src/app/utils/utils';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {
  private utils!: Utils;
  public userData?: TokenData;

  constructor(private cookie: CookieService, tokenDecoder: TokenDecodeService, private router: Router) {
    this.utils = new Utils(cookie, tokenDecoder, router);

    if (this.utils.validateToken()) {
      this.userData = this.utils.getUserDataFromToken();
    }
  }

  public logoff(): void {
    this.cookie.delete(environment.cookieToken);
    this.router.navigate(['/']).then(() => { window.location.reload(); });
  }
}
