import { Component, OnInit, Output, Input, EventEmitter } from '@angular/core';

@Component({
  standalone: true,
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.css'],
})
export class ModalComponent implements OnInit {
  @Output() closeModal = new EventEmitter();
  constructor() {}

  ngOnInit(): void {}

  close() {
    this.closeModal.emit();
  }

  stopPropegation(e: any) {
    e.stopPropagation();
  }
}
