// This file can be replaced during build by using the `fileReplacements` array.
// `ng build` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,

  // App settings
  cypher: "r5u8x/A?D*G-KaPdSgVkYp3s6v9y$B&E",
  cookieToken: "jwtToken",
  secret: "*F-JaNdRgUkXp2r5u8x/A?D(G+KbPeSh\"",

  // API settings
  urlApi: "http://localhost:8081/api/",
  urlAlerta: "Alerta",
  urlAuthLogin: "Auth/login",
  urlAuthSignUp: "Auth/signup",
  urlComentario: 'Comentario',
  urlTag: "Tag",
  urlTopico: "Topico",
  urlTopicoAutor: "fromAuthor",
  urlTopicoRecent: "recent",
  urlTopicoSearch: "search",
  urlTopicoTag: "Relational/topicoTag",
  urlUsuario: "Usuario"
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/plugins/zone-error';  // Included with Angular CLI.
