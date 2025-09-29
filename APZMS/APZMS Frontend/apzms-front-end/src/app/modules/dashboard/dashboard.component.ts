import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { User } from '../../core/models/user.model';

@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})

export class DashboardComponent implements OnInit {
  userId!: string | undefined
  userName!: string | undefined
  userRole!: string | undefined
  user!: User | null

  constructor(private auth: AuthService) { }

  ngOnInit() {
    this.user = this.auth.loadUserFromStorage()

    this.userId = this.user?.id
    this.userName = this.user?.name
    this.userRole = this.user?.role
  }
}