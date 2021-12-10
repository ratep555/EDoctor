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
  selector: 'app-chart-offices-patient',
  templateUrl: './chart-offices-patient.component.html',
  styleUrls: ['./chart-offices-patient.component.css']
})
export class ChartOfficesPatientComponent implements OnInit {
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

  constructor(private patientsService: PatientsService) { }

  ngOnInit(): void {
    this.showNumberOfOfficesForPatient();
  }

  showNumberOfOfficesForPatient() {
    this.patientsService.showNumberOfOfficesForPatient().subscribe(
      result => {
        this.data = [];
        this.title = 'Offices visited';
        this.type = ChartType.PieChart;
        console.log(result.list);
        for (const data in result.list) {
          if (data) {
            this.data.push([result.list[data].officeType.toString(),
              result.list[data].numberOfOffices]);
          }
        }
      },
      error => {
        console.log(error);
      }
    );
  }

}
