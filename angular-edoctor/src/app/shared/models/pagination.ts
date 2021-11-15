import { Appointment } from './appointment';
import { Doctor } from './doctor';
import { Office } from './office';
import { MedicalRecord } from './medicalrecord';

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

export interface PaginationFofMedicalRecords {
    page: number;
    pageCount: number;
    count: number;
    data: MedicalRecord[];
  }
