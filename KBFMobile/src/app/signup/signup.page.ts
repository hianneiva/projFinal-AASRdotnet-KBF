import { Component, OnInit } from '@angular/core';
import { UtilsService } from '../service/utils.service';
import { ApiService } from '../service/api.service';
import { CookieService } from 'ngx-cookie-service';
import { Router } from '@angular/router';
import { TokenDecoderService } from '../service/token-decoder.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.page.html',
  styleUrls: ['./signup.page.scss'],
})
export class SignupPage implements OnInit {
  private utils: UtilsService;
  
  public email?: string;
  public login?: string;
  public name?: string;
  public password?: string;
  public passwordRepeat?: string;
  public loginFailure?: string;

  constructor(private api: ApiService, private cookie: CookieService, private router: Router, _: TokenDecoderService) {
    this.utils = new UtilsService (cookie, _, router);
   }

  ngOnInit() { }

  public signUp(): void {
    const missingField: boolean = this.utils.stringIsNullOrEmpty(this.email) ||
                                  this.utils.stringIsNullOrEmpty(this.login) ||
                                  this.utils.stringIsNullOrEmpty(this.name) ||
                                  this.utils.stringIsNullOrEmpty(this.password) ||
                                  this.utils.stringIsNullOrEmpty(this.passwordRepeat);

    if (missingField) {
      this.loginFailure = "Todos os campos devem ser preenchidos";
      return;
    }

    const passwordsDoesntMatch = this.password !== this.passwordRepeat;

    if (passwordsDoesntMatch) {
      this.loginFailure = "As senhas devem conferir";
      return;
    }

    this.api.signup(this.login!, this.password!, this.name!, this.email!).subscribe({
      next: (response) => {
        if (response.result) {
          this.cookie.set(environment.cookieToken, response.token!, { expires: 0.3, secure: true, sameSite: 'Lax' });
          this.router.navigate(['/main']);
        } else {
          this.loginFailure = response.message;
        }
      },
      error: () => {
        this.loginFailure = "Houve uma falha no cadastro";
      }
    });
  }
}
