export interface Patient {
    id: number;
    applicationUserId: number;
    name: string;
    dateOfBirth: Date;
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
    name: string;
    dateOfBirth: Date;
    phoneNumber: string;
    country: string;
    city: string;
    street: string;
    mbo: string;
}
