import { Component, OnInit } from '@angular/core';
import {
  ChartErrorEvent,
  ChartMouseLeaveEvent,
  ChartMouseOverEvent,
  ChartSelectionChangedEvent,
  ChartType,
  Column,
  GoogleChartComponent
} from 'angular-google-charts';
import { PatientsService } from '../../patients.service';

@Component({
  selector: 'app-chart-appointments-patient',
  templateUrl: './chart-appointments-patient.component.html',
  styleUrls: ['./chart-appointments-patient.component.css']
})
export class ChartAppointmentsPatientComponent implements OnInit {
  title = '';
  type = ChartType.PieChart;
  data = [];
  columnNames: Column[] = ['Hospital', 'Private'];
  options = {
    width: 1000,
    height: 700,
    hAxis: { title: '' },
  };
  width = 777;
  height = 300;

  constructor(private patientsService: PatientsService) { }

  ngOnInit(): void {
    this.showNumberOfAppointmentsForPatient();
  }

  showNumberOfAppointmentsForPatient() {
    this.patientsService.showNumberOfAppointmentsForPatient().subscribe(
      result => {
        this.data = [];
        this.title = 'Appointments';
        this.type = ChartType.PieChart;
        console.log(result.list);
        for (const data in result.list) {
          if (data) {
            this.data.push([result.list[data].appointmentType.toString(),
              result.list[data].numberOfAppointments]);
          }
        }
      },
      error => {
        console.log(error);
      }
    );
  }

}
