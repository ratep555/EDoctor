import { Appointment } from './appointment';
import { Office } from './office';

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
