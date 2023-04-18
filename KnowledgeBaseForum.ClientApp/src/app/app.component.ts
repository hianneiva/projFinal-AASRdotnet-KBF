import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { TokenData } from './model/token-data';
import { TokenDecodeService } from './services/token-decode.service';
import { Utils } from './utils/utils';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Fórum de Compartilhamento de Conhecimento';
  user?: TokenData;
  formulario?: FormGroup;

  constructor(private cookie: CookieService, private decoder: TokenDecodeService, private _: Router) { }

  updateHeader(_: Event): void {
    const utils = new Utils(this.cookie, this.decoder, this._);
    this.user = utils.getUserDataFromToken();
  }
}
