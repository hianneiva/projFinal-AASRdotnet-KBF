import { Component } from '@angular/core';
import { ApiServiceService } from 'src/app/services/api-service.service';
import { Utils } from 'src/app/utils/utils';

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

  constructor(private api: ApiServiceService) {
    this.utils = new Utils();
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

    this.api.login(this.username!, this.password!).subscribe(response => {
      this.alertActive = !response.result;
      this.alertMsg = !response.result ? '' : response.message!;
    });
  }
}
