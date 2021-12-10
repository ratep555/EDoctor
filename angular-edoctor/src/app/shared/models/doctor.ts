import { Hospital } from './hospital';
import { Office } from './office';
import { Specialty } from './specialty';

export interface Doctor {
    id: number;
    applicationUserId: number;
    name: string;
    resume: string;
    averageVote: number;
    userVote: number;
    count: number;
    startedPracticing: Date;
    picture: string;
    qualifications: string;
    specialties: Specialty[];
    hospitals: Hospital[];
    offices: Office[];
}

export interface DoctorEditDto {
    id: number;
    name: string;
    applicationUserId: number;
    resume: string;
    qualifications: string;
    specialtiesIds: number[];
    hospitalsIds: number[];
    picture: File;
}

export interface DoctorPutGetDto {
    doctor: Doctor;
    selectedSpecialties: Specialty[];
    nonSelectedSpecialties: Specialty[];
    selectedHospitals: Hospital[];
    nonSelectedHospitals: Hospital[];
    offices: Office[];

}




