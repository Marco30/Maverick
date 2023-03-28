import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { ThemeService } from './services/theme/theme.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'CodeRed';
  theme$!: Observable<string>;
  constructor(private themeService: ThemeService) {
    this.theme$ = this.themeService.getTheme();
  }

  toggleDarkMode() {
    this.themeService.togglecurrentTheme();
  }
}
