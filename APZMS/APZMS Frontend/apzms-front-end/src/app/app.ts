import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LoginModule } from './modules/login/login.module';
import { RegistrationModule } from './modules/registration/registration.module';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, LoginModule, RegistrationModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})

export class App {
  protected readonly title = signal('apzms-front-end');
}
