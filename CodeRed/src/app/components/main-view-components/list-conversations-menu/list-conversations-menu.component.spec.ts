import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListConversationsMenuComponent } from './list-conversations-menu.component';

describe('ListConversationsMenuComponent', () => {
  let component: ListConversationsMenuComponent;
  let fixture: ComponentFixture<ListConversationsMenuComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ListConversationsMenuComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListConversationsMenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
