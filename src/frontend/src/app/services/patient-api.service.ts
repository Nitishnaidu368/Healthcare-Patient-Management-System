import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { environment } from '../../environments/environment';
import { AppointmentRequest, Patient, PatientRequest } from '../models/patient.model';

@Injectable({
  providedIn: 'root'
})
export class PatientApiService {
  private readonly baseUrl = `${environment.apiBaseUrl}/api`;

  constructor(private readonly http: HttpClient) {}

  getPatients(): Promise<Patient[]> {
    return firstValueFrom(this.http.get<Patient[]>(`${this.baseUrl}/patients`));
  }

  createPatient(request: PatientRequest): Promise<Patient> {
    return firstValueFrom(this.http.post<Patient>(`${this.baseUrl}/patients`, request));
  }

  getAppointments(): Promise<AppointmentRequest[]> {
    return firstValueFrom(this.http.get<AppointmentRequest[]>(`${this.baseUrl}/appointments`));
  }

  createAppointment(request: AppointmentRequest): Promise<AppointmentRequest> {
    return firstValueFrom(this.http.post<AppointmentRequest>(`${this.baseUrl}/appointments`, request));
  }
}
