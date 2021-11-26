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
import { PatientStatistics } from 'src/app/shared/models/statistics';
import { PatientsService } from '../patients.service';

@Component({
  selector: 'app-statistics-patient',
  templateUrl: './statistics-patient.component.html',
  styleUrls: ['./statistics-patient.component.css']
})
export class StatisticsPatientComponent implements OnInit {
  patientstatistics: PatientStatistics;

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

  constructor(private patientsService: PatientsService) { }

  ngOnInit(): void {
    this.loadPatientStatistics();
    this.showNumberOfMedicalRecordsForPatient();
  }

  loadPatientStatistics() {
    return this.patientsService.getCountForEntitiesForPatient()
    .subscribe(response => {
    this.patientstatistics = response;
    }, error => {
    console.log(error);
    });
    }

  showNumberOfMedicalRecordsForPatient() {
    this.patientsService.showNumberOfMedicalRecordsForPatient().subscribe(
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
