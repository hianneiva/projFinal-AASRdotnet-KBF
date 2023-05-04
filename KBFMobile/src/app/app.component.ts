import { Component } from '@angular/core';
import { UtilsService } from './service/utils.service';
import { CookieService } from 'ngx-cookie-service';
import { TokenDecoderService } from './service/token-decoder.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent {
  private utils: UtilsService;

  constructor(_c: CookieService, _t: TokenDecoderService, _r: Router) {
    this.utils = new UtilsService(_c, _t, _r);
  }

  public logoff(): void {
    this.utils.logoff();
  }
}
