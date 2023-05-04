import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { UsusarioPageRoutingModule } from './ususario-routing.module';

import { UsusarioPage } from './ususario.page';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    UsusarioPageRoutingModule
  ],
  declarations: [UsusarioPage]
})
export class UsusarioPageModule {}
