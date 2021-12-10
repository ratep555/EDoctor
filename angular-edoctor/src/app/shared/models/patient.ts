export interface Patient {
    id: number;
    applicationUserId: number;
    genderId: number;
    name: string;
    dateOfBirth: Date;
    gender: string;
    phoneNumber: string;
    email: string;
    country: string;
    city: string;
    street: string;
    mbo: string;
}

export class PatientEdit {
    id: number;
    applicationUserId: number;
    genderId: number;
    name: string;
    dateOfBirth: Date;
    phoneNumber: string;
    country: string;
    city: string;
    street: string;
    mbo: string;
}
