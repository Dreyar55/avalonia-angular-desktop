import { Component, OnInit, computed, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  imports: [],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App implements OnInit {
  metadata = signal<any | null>(null);

  constructor(private httpClient: HttpClient) {}

  ngOnInit(): void {
    this.httpClient
      .get<any>('http://localhost:54321/api/metadata')
      .subscribe({
        next: (data) => this.metadata.set(data),
        error: () => this.metadata.set({ name: 'DesktopApp', version: 'N/A', environment: 'N/A' })
      });
  }
}
