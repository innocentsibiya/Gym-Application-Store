import { TestBed } from '@angular/core/testing';
import { GymItemService } from './gym-item-service.service';


describe('GymItemServiceService', () => {
  let service: GymItemService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GymItemService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
