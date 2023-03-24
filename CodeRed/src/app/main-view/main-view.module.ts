import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MainViewRoutingModule } from './main-view-routing.module';
import { MainViewComponent } from './main-view.component';

import { DragDropModule } from '@angular/cdk/drag-drop';
import { SidebarComponent } from '../components/sidebar/sidebar.component';
import { NavigationMenuComponent } from '../components/navigation-menu/navigation-menu.component';

@NgModule({
  declarations: [MainViewComponent, NavigationMenuComponent, SidebarComponent],
  imports: [CommonModule, MainViewRoutingModule, DragDropModule],
})
export class MainViewModule {}
