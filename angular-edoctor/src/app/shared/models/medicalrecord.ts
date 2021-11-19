export interface MedicalRecord {
    id: number;
    anamnesisDiagnosisTherapy: string;
    created: Date;
    doctor: string;
    patient: string;
    office: string;
    hospital: string;
    patientId: number;
    doctorId: number;
}

export class MedicalrecordCreateEdit {
    id: number;
    anamnesisDiagnosisTherapy: string;
    created: Date;
    patientId: number;
    officeId: number;
}

