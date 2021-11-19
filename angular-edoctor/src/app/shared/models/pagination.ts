import { Appointment } from './appointment';
import { Doctor } from './doctor';
import { Office } from './office';
import { MedicalRecord } from './medicalrecord';
import { Patient } from './patient';

export interface PaginationForOffices {
    page: number;
    pageCount: number;
    count: number;
    data: Office[];
  }

export interface PaginationForAppointments {
    page: number;
    pageCount: number;
    count: number;
    data: Appointment[];
  }

export interface PaginationForDoctors {
    page: number;
    pageCount: number;
    count: number;
    data: Doctor[];
  }

export interface PaginationForPatients {
    page: number;
    pageCount: number;
    count: number;
    data: Patient[];
  }


export interface PaginationForMedicalRecords {
    page: number;
    pageCount: number;
    count: number;
    data: MedicalRecord[];
  }
