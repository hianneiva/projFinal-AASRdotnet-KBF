import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TopicoComponent } from './componentes/topico/topico.component';
import { TopicoNovoComponent } from './componentes/topico-novo/topico-novo.component';
import { TopicoAlterarComponent } from './componentes/topico-alterar/topico-alterar.component';
import { TopicoDeletarComponent } from './componentes/topico-deletar/topico-deletar.component';
import { TopicoListarComponent } from './componentes/topico-listar/topico-listar.component';
import { LoginComponent } from './components/login/login.component';

import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CookieService } from 'ngx-cookie-service';
import { MomentModule } from 'ngx-moment';
import { SignupComponent } from './components/signup/signup.component';
import { HomeComponent } from './components/home/home.component';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { MarkdownModule } from 'ngx-markdown';
import { MarkdownEditComponent } from './components/markdown-edit/markdown-edit.component';
import AlertaComponent from './components/alerta/alerta.component';

@NgModule({
  declarations: [
    AppComponent,
    TopicoComponent,
    TopicoNovoComponent,
    TopicoAlterarComponent,
    TopicoDeletarComponent,
    TopicoListarComponent,
    LoginComponent,
    SignupComponent,
    HomeComponent,
    HeaderComponent,
    FooterComponent,
    MarkdownEditComponent,
    AlertaComponent
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
