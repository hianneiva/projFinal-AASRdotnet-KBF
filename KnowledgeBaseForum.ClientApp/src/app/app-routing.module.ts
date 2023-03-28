import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TopicoComponent } from './components/topico/topico.component';
import { TopicoNovoComponent } from './components/topico-novo/topico-novo.component';
import { TopicoAlterarComponent } from './components/topico-alterar/topico-alterar.component';
import { AlertaComponent } from './components/alerta/alerta.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { MarkdownEditComponent } from './components/markdown-edit/markdown-edit.component';
import { SignupComponent } from './components/signup/signup.component';
import { AuthGuard } from './guards/auth.guard';
import { UsuarioComponent } from './components/usuario/usuario.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignupComponent },
  { path: 'alerta', component: AlertaComponent, canActivate: [AuthGuard] },
  { path: 'testMark', component: MarkdownEditComponent },
  { path: 'usuario', component: UsuarioComponent },
  { path: 'topico', component: TopicoComponent },
  { path: 'topico-novo', component: TopicoNovoComponent },
  { path: 'topico-alterar', component: TopicoAlterarComponent },
  { path: 'topico-alterar/:id', component: TopicoAlterarComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
