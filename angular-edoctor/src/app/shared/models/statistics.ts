export interface Statistics {
    patientsCount: number;
    doctorsCount: number;
    officesCount: number;
    appointmentsCount: number;
}

export interface PatientStatistics {
    appointmentsCount: number;
    doctorsCount: number;
    medicalRecordsCount: number;
    officesCount: number;
}

export interface DoctorStatistics {
    availableAppointmentsCount: number;
    upcomingAppointmentsCount: number;
    medicalRecordsCount: number;
    patientsCount: number;
}
