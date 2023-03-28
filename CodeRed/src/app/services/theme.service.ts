import { Injectable } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ThemeService {
  darkTheme = new BehaviorSubject<string>('light');

  public getTheme() {
    return this.darkTheme.asObservable();
  }

  constructor() {}

  public toggleDarkTheme() {
    document.documentElement.classList.toggle('dark-theme');
    this.darkTheme.next(this.darkTheme.value === 'light' ? 'dark' : 'light');
  }
}
