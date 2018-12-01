import { TestBed, inject } from '@angular/core/testing';

import { MyAccountService } from '../my-account.service';

describe('MyAccountService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [MyAccountService]
    });
  });

  it('should be created', inject([MyAccountService], (service: MyAccountService) => {
    expect(service).toBeTruthy();
  }));
});
