import { TestBed, inject } from '@angular/core/testing';

import { PageMessageService } from './page-message.service';

describe('PageMessageService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [PageMessageService]
    });
  });

  it('should be created', inject([PageMessageService], (service: PageMessageService) => {
    expect(service).toBeTruthy();
  }));
});
