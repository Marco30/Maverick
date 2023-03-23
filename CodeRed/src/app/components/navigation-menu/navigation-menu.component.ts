import { Component } from '@angular/core';
import { MenuItem } from 'src/app/model/menu-item';

@Component({
  selector: 'app-navigation-menu',
  templateUrl: './navigation-menu.component.html',
  styleUrls: ['./navigation-menu.component.css']
})
export class NavigationMenuComponent {

  menuItems: MenuItem[] = [
    new MenuItem('Home', 'fas fa-home', undefined, '/home'),
    new MenuItem('Users', 'fas fa-users', undefined, '/users'),
    new MenuItem('Categories', 'fas fa-list-ul', [
      new MenuItem('Category 1', '', undefined, '/categories/1'),
      new MenuItem('Category 2', '', undefined, '/categories/2'),
      new MenuItem('Category 3', '', undefined, '/categories/3')
    ], '/categories'),
    new MenuItem('Events', 'far fa-calendar-alt', undefined, '/events'),
    new MenuItem('Settings', 'fas fa-cog', undefined, '/settings'),
    new MenuItem('Help', 'fas fa-question-circle', undefined, '/help')
  ];

  toggleSubItems(item: MenuItem, event: any): void {
   
    if (item.subItems != undefined) {
      event.preventDefault();
    }

    const menuItem = this.menuItems.find(menuItem => menuItem.text === item.text);
  
    if (menuItem) {
      menuItem.showSubItems = !menuItem.showSubItems;
    }
  }


}
