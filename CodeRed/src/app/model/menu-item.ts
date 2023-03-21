export class MenuItem {
  text: string;
  icon: string;
  subItems?: MenuItem[];
  showSubItems?: boolean;
  link?: string;

  constructor(text: string, icon: string, subItems?: MenuItem[], link?: string) {
    this.text = text;
    this.icon = icon;
    this.subItems = subItems;
    this.showSubItems = false;
    this.link = link;
  }
}