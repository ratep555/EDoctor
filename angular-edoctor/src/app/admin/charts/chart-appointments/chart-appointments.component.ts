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
  selector: 'app-chart-appointments',
  templateUrl: './chart-appointments.component.html',
  styleUrls: ['./chart-appointments.component.css']
})
export class ChartAppointmentsComponent implements OnInit {
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
    this.showNumberOfAppointments();
  }

  showNumberOfAppointments() {
    this.adminService.showNumberOfAppointments().subscribe(
      result => {
        this.data = [];
        this.title = 'All appointments';
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
