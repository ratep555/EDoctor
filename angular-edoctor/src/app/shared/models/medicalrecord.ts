export interface MedicalRecord {
    appointmentId: number;
    anamnesisDiagnosisTherapy: string;
    created: Date;
    officeId: number;
    doctorId: number;
    patientId: number;
    doctor: string;
    patient: string;
    office: string;
    hospital: string;
}

export class MedicalrecordCreateEdit {
    appointmentId: number;
    anamnesisDiagnosisTherapy: string;
    created: Date;
}

