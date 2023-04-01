import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { TokenData } from 'src/app/model/token-data';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {
  @Input() userData?: TokenData;

  constructor(private cookie: CookieService, private router: Router) { }

  public logoff(): void {
    this.cookie.delete(environment.cookieToken);
    this.userData = undefined;
    this.router.navigate(['/']).then(() => { window.location.reload(); });
  }
}
