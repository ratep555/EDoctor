export interface Statistics {
    patientsCount: number;
    doctorsCount: number;
    officesCount: number;
    allAppointmentsCount: number;
    appointmentsCount: number;
}

export interface PatientStatistics {
    appointmentsCount: number;
    allAppointmentsCount: number;
    doctorsCount: number;
    medicalRecordsCount: number;
    officesCount: number;
}

export interface DoctorStatistics {
    allAppointmentsCount: number;
    availableAppointmentsCount: number;
    upcomingAppointmentsCount: number;
    medicalRecordsCount: number;
    patientsCount: number;
}
