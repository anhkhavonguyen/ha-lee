import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteSettingValueComponent } from './delete-setting-value.component';

describe('DeleteSettingValueComponent', () => {
  let component: DeleteSettingValueComponent;
  let fixture: ComponentFixture<DeleteSettingValueComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DeleteSettingValueComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DeleteSettingValueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
