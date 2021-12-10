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
  selector: 'app-chart-genders-doctor',
  templateUrl: './chart-genders-doctor.component.html',
  styleUrls: ['./chart-genders-doctor.component.css']
})
export class ChartGendersDoctorComponent implements OnInit {
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
    this.showNumberOfPatientsWithGenderForDoctor();
  }

  showNumberOfPatientsWithGenderForDoctor() {
    this.doctorsService.showNumberOfPatientsWithGenderForDoctor().subscribe(
      result => {
        this.data = [];
        this.title = 'Gender structure';
        this.type = ChartType.PieChart;
        console.log(result.list);
        for (const data in result.list) {
          if (data) {
            this.data.push([result.list[data].genderType.toString(),
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








