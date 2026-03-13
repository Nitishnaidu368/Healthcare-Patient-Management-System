import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AppointmentRequest, Patient, PatientRequest } from './models/patient.model';
import { PatientApiService } from './services/patient-api.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly api = inject(PatientApiService);

  protected readonly title = 'Healthcare Patient Management';
  protected patients: Patient[] = [];
  protected appointments: AppointmentRequest[] = [];
  protected loading = false;
  protected errorMessage = '';

  protected readonly patientForm = this.fb.nonNullable.group({
    firstName: ['', [Validators.required]],
    lastName: ['', [Validators.required]],
    dateOfBirth: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    phoneNumber: ['', [Validators.required]],
    gender: ['Unknown', [Validators.required]]
  });

  protected readonly appointmentForm = this.fb.nonNullable.group({
    patientId: ['', [Validators.required]],
    doctorName: ['', [Validators.required]],
    startAtUtc: ['', [Validators.required]],
    endAtUtc: ['', [Validators.required]],
    reason: ['', [Validators.required]]
  });

  ngOnInit(): void {
    this.loadData();
  }

  protected async loadData(): Promise<void> {
    this.loading = true;
    this.errorMessage = '';

    try {
      const [patients, appointments] = await Promise.all([
        this.api.getPatients(),
        this.api.getAppointments()
      ]);

      this.patients = patients;
      this.appointments = appointments;
    } catch {
      this.errorMessage = 'Unable to load data from API. Verify backend is running.';
    } finally {
      this.loading = false;
    }
  }

  protected async createPatient(): Promise<void> {
    if (this.patientForm.invalid) {
      this.patientForm.markAllAsTouched();
      return;
    }

    const payload: PatientRequest = this.patientForm.getRawValue();
    await this.api.createPatient(payload);
    this.patientForm.reset({ gender: 'Unknown' });
    await this.loadData();
  }

  protected async createAppointment(): Promise<void> {
    if (this.appointmentForm.invalid) {
      this.appointmentForm.markAllAsTouched();
      return;
    }

    const formData = this.appointmentForm.getRawValue();
    const payload: AppointmentRequest = {
      patientId: formData.patientId,
      doctorName: formData.doctorName,
      startAtUtc: new Date(formData.startAtUtc).toISOString(),
      endAtUtc: new Date(formData.endAtUtc).toISOString(),
      reason: formData.reason
    };

    await this.api.createAppointment(payload);
    this.appointmentForm.reset();
    await this.loadData();
  }
}
