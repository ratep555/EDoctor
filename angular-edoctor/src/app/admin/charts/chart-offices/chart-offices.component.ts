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
  selector: 'app-chart-offices',
  templateUrl: './chart-offices.component.html',
  styleUrls: ['./chart-offices.component.css']
})
export class ChartOfficesComponent implements OnInit {
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
    this.showNumberOfOffices();
  }

  showNumberOfOffices() {
    this.adminService.showNumberOfOffices().subscribe(
      result => {
        this.data = [];
        this.title = 'Offices';
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
