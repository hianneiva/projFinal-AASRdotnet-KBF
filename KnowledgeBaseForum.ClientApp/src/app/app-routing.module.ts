import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AlertaComponent } from './components/alerta/alerta.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { MarkdownEditComponent } from './components/markdown-edit/markdown-edit.component';
import { SignupComponent } from './components/signup/signup.component';
import { AuthGuard } from './guards/auth.guard';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignupComponent },
  { path: 'alerta', component: AlertaComponent, canActivate: [AuthGuard] },
  { path: 'testMark', component: MarkdownEditComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
