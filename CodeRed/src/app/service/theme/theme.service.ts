import { Injectable } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ThemeService {
  currentTheme = new BehaviorSubject<string>('light');

  public getTheme() {
    return this.currentTheme.asObservable();
  }

  constructor() {}

  public togglecurrentTheme() {
    document.documentElement.classList.toggle('dark-theme');
    this.currentTheme.next(
      this.currentTheme.value === 'light' ? 'dark' : 'light'
    );
  }
}
