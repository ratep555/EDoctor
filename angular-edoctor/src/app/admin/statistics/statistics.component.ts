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
import { Statistics } from 'src/app/shared/models/statistics';
import { AdminService } from '../admin.service';

@Component({
  selector: 'app-statistics',
  templateUrl: './statistics.component.html',
  styleUrls: ['./statistics.component.css']
})
export class StatisticsComponent implements OnInit {
  statistics: Statistics;

  title = '';
  type = ChartType.PieChart;
  data = [];
  columnNames: Column[] = ['Hospital and Private', 'Hospital'];
  options = {
    width: 1000,
    height: 700,
   // backgroundColor: '#ffff00',
    hAxis: { title: '' },
  };
  width = 777;
  height = 300;

  constructor(private adminService: AdminService) { }

  ngOnInit() {
    this.loadStatistics();
    this.showNumberOfDoctors();
  }

  loadStatistics() {
    return this.adminService.getCountForEntities()
    .subscribe(response => {
    this.statistics = response;
    }, error => {
    console.log(error);
    });
    }

  showNumberOfDoctors() {
    this.adminService.showNumberOfDoctors().subscribe(
      result => {
        this.data = [];
        this.title = 'Practicing Doctors';
        this.type = ChartType.PieChart;
        console.log(result.list);
        for (const data in result.list) {
          if (data) {
            this.data.push([result.list[data].doctorType.toString(),
              result.list[data].numberOfDoctors]);
          }
        }
      },
      error => {
        console.log(error);
      }
    );
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
