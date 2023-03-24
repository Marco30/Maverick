import { Component, ElementRef, Renderer2 } from '@angular/core';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { NavigationMenuComponent } from '../navigation-menu/navigation-menu.component';

interface Item {
  name: string;
  data: any;
  position: number;
}

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css'],
})
export class SidebarComponent {
  // Define properties
  dragMode = false;
  isCollapsed = [false, false, false];
  items = [
    { name: 'Column 1', data: NavigationMenuComponent, position: 0 },
    { name: 'Column 2', data: null, position: 1 },
    { name: 'Column 3', data: null, position: 2 },
  ];

  // Inject ElementRef and Renderer2 into the constructor
  constructor(private elementRef: ElementRef, private renderer: Renderer2) {}

  ngOnInit() {
    this.getStoredItem();
  }

  // This method retrieves the items from local storage and parses them into an array of objects.
  // The 'data' property of each object is a component name that is used to recreate the component instance later.
  getStoredItem() {
    // Retrieve items from local storage
    const itemsJson = localStorage.getItem('items');

    // Check if items are stored in local storage
    if (itemsJson) {
      // Parse the stored items JSON into an array of objects
      const items = JSON.parse(itemsJson, (key, value) => {
        // Check if the current key is 'data'
        if (key === 'data') {
          // Recreate the component instance based on its name
          switch (value) {
            case 'NavigationMenuComponent':
              // If the component name is 'NavigationMenuComponent', return the component class
              return NavigationMenuComponent;
            case 'empty':
              // If the component name is 'empty', return null
              return null;
            default:
              // If the component name is neither 'NavigationMenuComponent' nor 'empty', return null
              return null;
          }
        }
        // Return the current value for other keys
        return value;
      });
      // Set the items array
      this.items = items;
    }
  }

  // Define the toggleCollapse function, which toggles the isCollapsed property of the selected index
  toggleCollapse(index: number) {
    this.isCollapsed[index] = !this.isCollapsed[index];
  }

  // Define the onDrop function, which handles the drop event
  onDrop(event: CdkDragDrop<Item[]>) {
    // Get the dragged element, drop element, and parent element
    const draggedElement =
      event.previousContainer.element.nativeElement.children[
        event.previousIndex
      ];
    const dropElement =
      event.container.element.nativeElement.children[event.currentIndex];
    const parentElement = dropElement.parentNode;

    // Check if parentElement is not null
    if (parentElement) {
      // Handle case where the element is dropped in the same container
      if (event.previousContainer === event.container) {
        const nextSibling = dropElement.nextSibling;
        if (nextSibling && event.previousIndex < event.currentIndex) {
          // Handle case where the dragged element is dropped after the current element
          this.renderer.insertBefore(
            parentElement,
            draggedElement,
            nextSibling
          );
        } else {
          // Handle case where the dragged element is dropped before the current element
          this.renderer.insertBefore(
            parentElement,
            draggedElement,
            dropElement
          );
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
          this.renderer.insertBefore(
            parentElement,
            draggedElement,
            siblings[0]
          );
        } else {
          // Handle case where the dragged element is dropped in the middle of the container
          const nextSibling = siblings[targetIndex];
          this.renderer.insertBefore(
            parentElement,
            draggedElement,
            nextSibling
          );
        }
      }
    }

    // Update the position of the items in the array
    moveItemInArray(this.items, event.previousIndex, event.currentIndex);
    this.items.forEach((item, index) => (item.position = index));

    // Save the updated items to local storage
    const itemsData = this.items.map((item) => ({
      name: item.name,
      data: item.data ? item.data.name : 'empty',
      position: item.position,
    }));
    localStorage.setItem('items', JSON.stringify(itemsData));
  }

  // Define the toggleDragMode function, which toggles the dragMode property
  toggleDragMode() {
    this.dragMode = !this.dragMode;
  }
}
