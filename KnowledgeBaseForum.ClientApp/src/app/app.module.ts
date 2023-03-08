import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TopicoComponent } from './componentes/topico/topico.component';
import { TopicoNovoComponent } from './componentes/topico-novo/topico-novo.component';
import { TopicoAlterarComponent } from './componentes/topico-alterar/topico-alterar.component';
import { TopicoDeletarComponent } from './componentes/topico-deletar/topico-deletar.component';
import { TopicoListarComponent } from './componentes/topico-listar/topico-listar.component';

@NgModule({
  declarations: [
    AppComponent,
    TopicoComponent,
    TopicoNovoComponent,
    TopicoAlterarComponent,
    TopicoDeletarComponent,
    TopicoListarComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
