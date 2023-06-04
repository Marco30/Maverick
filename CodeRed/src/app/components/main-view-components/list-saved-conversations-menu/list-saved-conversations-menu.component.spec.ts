import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListSavedConversationsMenuComponent } from './list-saved-conversations-menu.component';

describe('ListSavedConversationsMenuComponent', () => {
  let component: ListSavedConversationsMenuComponent;
  let fixture: ComponentFixture<ListSavedConversationsMenuComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ListSavedConversationsMenuComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListSavedConversationsMenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
