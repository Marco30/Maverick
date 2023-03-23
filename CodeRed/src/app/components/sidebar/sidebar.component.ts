import { Component, ElementRef, Renderer2 } from '@angular/core';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';

interface Item {
  name: string;
}

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent {

  // Define properties
  dragMode = false;
  isCollapsed = [false, false, false];
  items: Item[] = [
    { name: 'Item 1' },
    { name: 'Item 2' },
    { name: 'Item 3' }
  ];

  // Inject ElementRef and Renderer2 into the constructor
  constructor(private elementRef: ElementRef, private renderer: Renderer2) {}

  // Define the toggleCollapse function, which toggles the isCollapsed property of the selected index
  toggleCollapse(index: number) {
    this.isCollapsed[index] = !this.isCollapsed[index];
  }

  // Define the onDrop function, which handles the drop event
  onDrop(event: CdkDragDrop<Item[]>) {
    // Get the dragged element, drop element, and parent element
    const draggedElement = event.previousContainer.element.nativeElement.children[event.previousIndex];
    const dropElement = event.container.element.nativeElement.children[event.currentIndex];
    const parentElement = dropElement.parentNode;
  
    // Check if parentElement is not null
    if (parentElement) {
      // Handle case where the element is dropped in the same container
      if (event.previousContainer === event.container) {
        const nextSibling = dropElement.nextSibling;
        if (nextSibling && event.previousIndex < event.currentIndex) {
          // Handle case where the dragged element is dropped after the current element
          this.renderer.insertBefore(parentElement, draggedElement, nextSibling);
        } else {
          // Handle case where the dragged element is dropped before the current element
          this.renderer.insertBefore(parentElement, draggedElement, dropElement);
        }
      } else {
        // Handle case where the element is dropped in a different container
        const targetIndex = event.currentIndex;
        const siblings = parentElement.children;
        if (targetIndex === siblings.length) {
          // Handle case where the dragged element is dropped at the end of the container
          this.renderer.appendChild(parentElement, draggedElement);
        } else if (targetIndex === 0) {
          // Handle case where the dragged element is dropped at the beginning of the container
          this.renderer.insertBefore(parentElement, draggedElement, siblings[0]);
        } else {
          // Handle case where the dragged element is dropped in the middle of the container
          const nextSibling = siblings[targetIndex];
          this.renderer.insertBefore(parentElement, draggedElement, nextSibling);
        }
      }
    }
  }

  // Define the toggleDragMode function, which toggles the dragMode property
  toggleDragMode() {
    this.dragMode = !this.dragMode;
  }
}