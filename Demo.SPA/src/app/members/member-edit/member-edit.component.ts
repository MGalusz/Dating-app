import { AuthService } from './../../_services/auth.service';
import { UserService } from './../../_services/user.service';
import { AlertityService } from './../../_services/alertity.service';
import { ActivatedRoute } from '@angular/router';
import { User } from './../../_models/User';
import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  user: User;
  @ViewChild('editForm') editForm: NgForm;
  photoUrl: string;
  constructor(private routr: ActivatedRoute,
    private alertify: AlertityService,
    private authService: AuthService,
    private userService: UserService) { }

  ngOnInit() {
    this.routr.data.subscribe(data => {
      this.user = data['user'];
    });
    this.authService.currentPhotoUrl.subscribe(photoUrl => this.photoUrl = photoUrl );
  }

  updateUser() {
    this.userService.updateUser(this.authService.decodeToken.nameid, this.user).subscribe(next => {
      this.alertify.succes('Profile Update successfully');
      this.editForm.reset(this.user);
    }, error => {
      this.alertify.error(error);
    });
  }
  updateMainPhoto(photoUrl){
    this.user.photoUrl = photoUrl ;
  }

}
