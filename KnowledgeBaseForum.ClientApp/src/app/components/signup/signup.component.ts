import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { ApiService } from 'src/app/services/api-service.service';
import { Utils } from 'src/app/utils/utils';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent {
  private utils: Utils;

  public alertActive: boolean = false;
  public alertMsg: string = '';
  public email?: string;
  public login?: string;
  public name?: string;
  public password?: string;
  public passwordRepeat?: string;

  constructor(private api: ApiService, private cookie: CookieService, private router: Router) {
    this.utils = new Utils();
  }

  public signUp(): void {
    const missingField: boolean = this.utils.stringIsNullOrEmpty(this.email) || this.utils.stringIsNullOrEmpty(this.login) || this.utils.stringIsNullOrEmpty(this.name) ||
      this.utils.stringIsNullOrEmpty(this.password) || this.utils.stringIsNullOrEmpty(this.passwordRepeat);
    const passwordsDoesntMatch = this.password !== this.passwordRepeat;
    this.alertActive = missingField || passwordsDoesntMatch;

    if (this.alertActive) {
      this.alertMsg = missingField ? "Um dos campos não foram informados" : "As senhas informadas não conferem";

      setTimeout(() => {
        this.alertActive = false;
        this.alertMsg = '';
      }, 5000);

      return;
    }

    this.api.signup(this.login!, this.password!, this.name!, this.email!).subscribe(response => {
      this.alertActive = !response.result;
      this.alertMsg = !response.result ? '' : response.message!;

      if (response.result) {
        this.cookie.set(environment.cookieToken, response.token!, { expires: 0.3, secure: true, sameSite: 'Lax' });
        this.cancelar(true);
      }
    });
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
