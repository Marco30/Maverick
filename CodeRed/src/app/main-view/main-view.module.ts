import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MainViewRoutingModule } from './main-view-routing.module';
import { MainViewComponent } from './main-view.component';

import { DragDropModule } from '@angular/cdk/drag-drop';
import { SidebarComponent } from '../components/main-view-components/sidebar/sidebar.component';
import { NavigationMenuComponent } from '../components/main-view-components/navigation-menu/navigation-menu.component';
import { ListConversationsMenuComponent } from '../components/main-view-components/list-conversations-menu/list-conversations-menu.component';

@NgModule({
  declarations: [MainViewComponent, NavigationMenuComponent, SidebarComponent, ListConversationsMenuComponent],
  imports: [CommonModule, MainViewRoutingModule, DragDropModule],
})
export class MainViewModule {}
