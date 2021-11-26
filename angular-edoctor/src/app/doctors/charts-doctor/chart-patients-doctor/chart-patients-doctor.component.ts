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
  selector: 'app-chart-patients-doctor',
  templateUrl: './chart-patients-doctor.component.html',
  styleUrls: ['./chart-patients-doctor.component.css']
})
export class ChartPatientsDoctorComponent implements OnInit {
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

  constructor(private doctorsService: DoctorsService) { }

  ngOnInit(): void {
    this.showNumberOfPatientsForDoctor();
  }

  showNumberOfPatientsForDoctor() {
    this.doctorsService.showNumberOfPatientsForDoctor().subscribe(
      result => {
        this.data = [];
        this.title = 'Patients';
        this.type = ChartType.PieChart;
        console.log(result.list);
        for (const data in result.list) {
          if (data) {
            this.data.push([result.list[data].practiceType.toString(),
              result.list[data].numberOfPatients]);
          }
        }
      },
      error => {
        console.log(error);
      }
    );
  }

}
