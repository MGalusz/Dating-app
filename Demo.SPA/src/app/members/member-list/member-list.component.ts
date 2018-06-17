import { AlertityService } from './../../_services/alertity.service';
import { Component, OnInit } from '@angular/core';
import { User } from '../../_models/User';
import { UserService } from '../../_services/user.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  users: User[];
  constructor(private userService: UserService, private alertify: AlertityService,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(date => {
      this.users = date['users'];
    });
  }
}
