import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MainViewRoutingModule } from './main-view-routing.module';
import { MainViewComponent } from './main-view.component';
import { SideMenuComponent } from '../components/side-menu/side-menu.component';




@NgModule({
  declarations: [
    MainViewComponent,
    SideMenuComponent
  ],
  imports: [
    CommonModule,
    MainViewRoutingModule
  ]
})
export class MainViewModule { }
