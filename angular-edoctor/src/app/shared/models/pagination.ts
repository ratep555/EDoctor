import { Appointment } from './appointment';
import { Doctor } from './doctor';
import { Office } from './office';
import { MedicalRecord } from './medicalrecord';
import { Patient } from './patient';
import { User } from './user';
import { Hospital } from './hospital';
import { Specialty } from './specialty';

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

export interface PaginationForUsers {
    page: number;
    pageCount: number;
    count: number;
    data: User[];
  }

export interface PaginationForHospitals {
    page: number;
    pageCount: number;
    count: number;
    data: Hospital[];
  }

export interface PaginationForSpecialties {
    page: number;
    pageCount: number;
    count: number;
    data: Specialty[];
  }


