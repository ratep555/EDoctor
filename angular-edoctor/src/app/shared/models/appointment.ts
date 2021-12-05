export interface Appointment {
    id: number;
    doctor: string;
    patientId: number;
    doctorId: number;
    officeId: number;
    appointmentId: number;
    patient: string;
    country: string;
    officeAddress: string;
    city: string;
    hospital: string;
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
