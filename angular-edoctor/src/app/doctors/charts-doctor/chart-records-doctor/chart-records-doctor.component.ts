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
import { DoctorsService } from '../../doctors.service';

@Component({
  selector: 'app-chart-records-doctor',
  templateUrl: './chart-records-doctor.component.html',
  styleUrls: ['./chart-records-doctor.component.css']
})
export class ChartRecordsDoctorComponent implements OnInit {
  title = '';
  type = ChartType.PieChart;
  data = [];
  columnNames: Column[] = ['Hospital', 'Private'];
  options = {
    width: 700,
    height: 500,
    hAxis: { title: '' },
  };
  width = 777;
  height = 300;

  constructor(private doctorsService: DoctorsService) { }

  ngOnInit(): void {
    this.showNumberOfMedicalRecordsForDoctor();
  }

  showNumberOfMedicalRecordsForDoctor() {
    this.doctorsService.showNumberOfMedicalRecordsForDoctor().subscribe(
      result => {
        this.data = [];
        this.title = 'Medical records';
        this.type = ChartType.PieChart;
        console.log(result.list);
        for (const data in result.list) {
          if (data) {
            this.data.push([result.list[data].medicalRecordType.toString(),
              result.list[data].numberOfMedicalRecords]);
          }
        }
      },
      error => {
        console.log(error);
      }
    );
  }

}
