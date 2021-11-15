import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppointmentsService } from 'src/app/appointments/appointments.service';
import { Appointment } from 'src/app/shared/models/appointment';
import { CoordinatesMapWithMessage } from 'src/app/shared/models/coordinate';
import { Office } from 'src/app/shared/models/office';
import { MyParams, UserParams } from 'src/app/shared/models/userparams';
import { OfficesService } from '../offices.service';

@Component({
  selector: 'app-info-office',
  templateUrl: './info-office.component.html',
  styleUrls: ['./info-office.component.css']
})
export class InfoOfficeComponent implements OnInit {
  office: Office;
  coordinates: CoordinatesMapWithMessage[] = [];
  appointments: Appointment[];
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  myParams = new MyParams();
  totalCount: number;

  sortOptions = [
    {name: 'Sort by Date Ascending', value: 'dateAsc'},
    {name: 'Sort by Date Descending', value: 'dateDesc'},
  ];

  constructor(private officesService: OfficesService,
              private appointmentsService: AppointmentsService,
              private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
     this.activatedRoute.params.subscribe((params) => {
      this.officesService.getOfficeById(params.id).subscribe((office) => {
        console.log(office);
        this.office = office;
        this.coordinates = [{latitude: office.latitude, longitude: office.longitude, message: office.street}];
        console.log(this.coordinates);
      });
    });
     this.getAppointments();
  }

  getAppointments() {
    this.appointmentsService
    .getAvailableAppointmentsForOffice(+this.activatedRoute.snapshot.paramMap.get('id'), this.myParams)
    .subscribe(response => {
      this.appointments = response.data;
      this.myParams.page = response.page;
      this.myParams.pageCount = response.pageCount;
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    }
    );
  }

 onSearch() {
    this.myParams.query = this.searchTerm.nativeElement.value;
    this.getAppointments();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.myParams = new MyParams();
    this.getAppointments();
  }

  onSortSelected(sort: string) {
    this.myParams.sort = sort;
    this.getAppointments();
  }

  onPageChanged(event: any) {
    if (this.myParams.page !== event) {
      this.myParams.page = event;
      this.getAppointments();
    }
}


}
