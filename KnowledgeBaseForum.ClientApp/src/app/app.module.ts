// Angular imports
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { CookieService } from 'ngx-cookie-service';
import { MomentModule } from 'ngx-moment';
import { MarkdownModule } from 'ngx-markdown';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AlertaComponent } from './components/alerta/alerta.component';
import { FooterComponent } from './components/footer/footer.component';
import { HeaderComponent } from './components/header/header.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { MarkdownEditComponent } from './components/markdown-edit/markdown-edit.component';
import { SignupComponent } from './components/signup/signup.component';
import { TopicoComponent } from './components/topico/topico.component';
import { TopicoListarComponent } from './components/topico-listar/topico-listar.component';
import { TopicoNovoComponent } from './components/topico-novo/topico-novo.component';
import { TopicoAlterarComponent } from './components/topico-alterar/topico-alterar.component';

@NgModule({
  declarations: [
    AppComponent,
    TopicoComponent,
    TopicoListarComponent,
    LoginComponent,
    SignupComponent,
    HomeComponent,
    HeaderComponent,
    FooterComponent,
    MarkdownEditComponent,
    AlertaComponent,
    TopicoNovoComponent,
    TopicoAlterarComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    MomentModule,
    MarkdownModule.forRoot()
  ],
  providers: [CookieService],
  bootstrap: [AppComponent]
})
export class AppModule { }
