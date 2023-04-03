import { Component, OnInit, EventEmitter,Output, Input } from '@angular/core';

@Component({
  selector: "app-loader",
  templateUrl: "./loader.component.html",
  styleUrls: ["./loader.component.css"],
})
export class LoaderComponent implements OnInit {
  @Input() showCancel = false;
  @Output() cancelLoader: EventEmitter<string> = new EventEmitter();
  constructor() {}

  ngOnInit(): void {}
  exitLoader() {
    this.cancelLoader.emit("exit");
  }
}
