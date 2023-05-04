import { Component } from '@angular/core';
import { UtilsService } from '../service/utils.service';
import { CookieService } from 'ngx-cookie-service';
import { Router } from '@angular/router';
import { TokenDecoderService } from '../service/token-decoder.service';
import { environment } from 'src/environments/environment';
import { ApiService } from '../service/api.service';

@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
})
export class HomePage {
  private utils: UtilsService;

  public username?: string;
  public password?: string;
  public loginFailure? :string;

  constructor(private api: ApiService, private cookie: CookieService, private router: Router, _: TokenDecoderService) {
    this.utils = new UtilsService(cookie, _, router);

    if (this.utils.validateToken()) {
      this.router.navigate(['/main']);
    }
  }

  public login(): void {
    this.api.login(this.username!, this.password!).subscribe({
      next: (response) => {
        if (response.result) {
          this.cookie.set(environment.cookieToken, response.token!, { expires: 0.3, secure: true, sameSite: 'Lax' });
          this.router.navigate(['/main']);
        }
        else {
          this.loginFailure = response.message;
        }
      },
      error: (error) => {
        this.loginFailure = "Não foi possível concluir o login: " + error.message;
      }
    });
  }

  public showAlert(): boolean {
    return this.utils.stringIsNullOrEmpty(this.loginFailure);
  }
}
