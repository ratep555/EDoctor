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
import { DoctorStatistics } from 'src/app/shared/models/statistics';
import { DoctorsService } from '../doctors.service';

@Component({
  selector: 'app-statistics-doctor',
  templateUrl: './statistics-doctor.component.html',
  styleUrls: ['./statistics-doctor.component.css']
})
export class StatisticsDoctorComponent implements OnInit {
  doctorstatistics: DoctorStatistics;

  title = '';
  type = ChartType.PieChart;
  data = [];
  columnNames: Column[] = ['Hospital and Private', 'Hospital'];
  options = {
    width: 1000,
    height: 700,
    hAxis: { title: '' },
  };
  width = 777;
  height = 300;

  constructor(private doctorsService: DoctorsService) { }

  ngOnInit(): void {
    this.loadDoctorStatistics();
    this.showNumberOfAppointmentsForDoctor();
  }

  loadDoctorStatistics() {
    return this.doctorsService.getCountForEntitiesForDoctor()
    .subscribe(response => {
    this.doctorstatistics = response;
    }, error => {
    console.log(error);
    });
    }

  showNumberOfAppointmentsForDoctor() {
    this.doctorsService.showNumberOfAppointmentsForDoctor().subscribe(
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
