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
import { AdminService } from '../../admin.service';

@Component({
  selector: 'app-chart-genders',
  templateUrl: './chart-genders.component.html',
  styleUrls: ['./chart-genders.component.css']
})
export class ChartGendersComponent implements OnInit {
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

  constructor(private adminService: AdminService) { }

  ngOnInit(): void {
    this.showNumberOfPatientsWithGender();
  }

  showNumberOfPatientsWithGender() {
    this.adminService.showNumberOfPatientsWithGender().subscribe(
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
