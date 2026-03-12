export interface Patient {
  id: string;
  firstName: string;
  lastName: string;
  dateOfBirth: string;
  email: string;
  phoneNumber: string;
  gender: string;
}

export interface PatientRequest {
  firstName: string;
  lastName: string;
  dateOfBirth: string;
  email: string;
  phoneNumber: string;
  gender: string;
}

export interface AppointmentRequest {
  patientId: string;
  doctorName: string;
  startAtUtc: string;
  endAtUtc: string;
  reason: string;
}
