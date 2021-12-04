export interface Office {
    id: number;
    doctor: string;
    doctorId: number;
    initialExaminationFee: number;
    followUpExaminationFee: number;
    street: string;
    city: string;
    country: string;
    latitude: number;
    description: string;
    longitude: number;
    picture: string;
}

export class OfficeCreateEdit {
    id: number;
    initialExaminationFee: number;
    followUpExaminationFee: number;
    street: string;
    city: string;
    country: string;
    description: string;
    latitude: number;
    longitude: number;
    hospitalId: number;
}

export interface OfficeCreateEdit1 {
    id: number;
    initialExaminationFee: number;
    followUpExaminationFee: number;
    street: string;
    city: string;
    country: string;
    description: string;
    latitude: number;
    longitude: number;
    hospitalId: number;
    picture: File;
}

