import { Router } from "@angular/router";
import { CookieService } from "ngx-cookie-service";
import { environment } from "src/environments/environment.development";
import { TokenData } from "../model/token-data";
import { TokenDecodeService } from "../services/token-decode.service";

export class Utils {
    private cookieService!: CookieService;
    private tokenDecoder!: TokenDecodeService;
    private router!: Router;

    constructor(cookieSvc: CookieService, decoder: TokenDecodeService, router: Router) {
        this.cookieService = cookieSvc;
        this.tokenDecoder = decoder;
        this.router = router;
    }

    // Quick and easy verification on whether a string value is null or empty
    public stringIsNullOrEmpty(input: string | null | undefined) {
        return input === '' || input === null || input === undefined;
    }

    // Converts any type to an array of its type, regardless of the amount of data given
    public arrayFromAny<Type>(input?: Type | Type[]): Type[] {
        if (input === null || input === undefined) {
            return [];
        }
        else {
            if (Array.isArray(input)) {
                return input;
            }
            else {
                return [input];
            }
        }
    }

    // Gets the JWT token stored in the cookies
    public getJwtToken(): string {
        return this.cookieService.get(environment.cookieToken);
    }

    // Extracts user data from token
    public getUserDataFromToken(): TokenData {
        const token = this.getJwtToken();
        return this.tokenDecoder.decodeToken(token);
    }

    // Redirect to login screen
    public redirLogin() {
        this.router.navigate(['login']);
    }

    // Validates if the current token is valid, or expired, or not
    public validateToken(): boolean {
        const token: string = this.getJwtToken();

        if (this.stringIsNullOrEmpty(token)) {
            this.redirLogin();
            this.cookieService.delete(environment.cookieToken);

            return false;
        }

        const data: TokenData = this.tokenDecoder.decodeToken(token);
        const currentTime: number = Date.now().valueOf() / 1000;

        if ((data.exp && data.exp! < currentTime) || (data.nbf && data.nbf > currentTime)) {
            this.redirLogin();
            this.cookieService.delete(environment.cookieToken);

            return false;
        }

        return true;
    }
}
