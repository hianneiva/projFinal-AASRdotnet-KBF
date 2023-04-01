import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { ApiService } from 'src/app/services/api-service.service';
import { TokenDecodeService } from 'src/app/services/token-decode.service';
import { Utils } from 'src/app/utils/utils';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  private utils: Utils;

  public alertActive: boolean = false;
  public alertMsg: string = '';
  public password?: string;
  public username?: string;

  constructor(private api: ApiService, private cookie: CookieService, private router: Router, _: TokenDecodeService) {
    this.utils = new Utils(cookie, _, router);
  }

  public login(): void {
    this.alertActive = (this.utils.stringIsNullOrEmpty(this.username) || this.utils.stringIsNullOrEmpty(this.password));

    if (this.alertActive) {
      this.alertMsg = "Login ou senha não são válidos";

      setTimeout(() => {
        this.alertActive = false;
        this.alertMsg = '';
      }, 5000);

      return;
    }

    this.api.login(this.username!, this.password!).subscribe(
      response => {
        this.alertActive = !response.result;
        this.alertMsg = !response.result ? '' : response.message!;

        if (response.result) {
          this.cookie.set(environment.cookieToken, response.token!, { expires: 0.3, secure: true, sameSite: 'Lax' });
          this.cancelar(true);
        }
      },
      error => {
        this.alertMsg = error.error.message;
        this.alertActive = true;

        setTimeout(() => {
          this.alertActive = false;
          this.alertMsg = '';
        }, 5000);
      }
    );
  }

  public cancelar(reload: boolean = false): void {
    if (reload) {
      this.router.navigate(['/']).then(() => { window.location.reload(); });
    }
    else {
      this.router.navigate(['/']);
    }
  }
}
