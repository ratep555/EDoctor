export interface Appointment {
    id: number;
    doctor: string;
    patientId: number;
    doctorId: number;
    officeId: number;
    patient: string;
    officeAddress: string;
    city: string;
    startDateAndTimeOfAppointment: Date;
    endDateAndTimeOfAppointment: Date;
    status: boolean;
    remarks: string;
}

export interface AppointmentSingle {
    id: number;
    doctor: string;
    patient: string;
    patientId: number;
    officeId: number;
    office: string;
    city: string;
    country: string;
    startDateAndTimeOfAppointment: Date;
    endDateAndTimeOfAppointment: Date;
    status: boolean;
    remarks: string;
}

export class AppointmentCreateEdit {
        id: number;
        officeId: number;
        patientId: number;
        startDateAndTimeOfAppointment: Date;
        endDateAndTimeOfAppointment: Date;
        remarks: string;
        status: string;
}
