import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { TokenData } from 'src/app/model/token-data';
import { TokenDecodeService } from 'src/app/services/token-decode.service';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  public userData?: TokenData;

  constructor(private cookie: CookieService, private tokenDecoder: TokenDecodeService, private router: Router) { }

  ngOnInit(): void {
    const token: string = this.cookie.get(environment.cookieToken);
    this.userData = this.tokenDecoder.decodeToken(token);
  }

  public logoff(): void {
    this.cookie.delete(environment.cookieToken);
    this.router.navigate(['/']).then(() => { window.location.reload(); });
  }
}
