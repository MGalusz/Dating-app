import { Routes, Router } from '@angular/router';
import { AlertityService } from './../_services/alertity.service';
import { AuthService } from './../_services/auth.service';
import { Component, OnInit } from '@angular/core';
import { error } from 'selenium-webdriver';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  photoUrl: string ;

  constructor(public authService: AuthService, private alertity: AlertityService, private router: Router) { }

  ngOnInit() {
    this.authService.currentPhotoUrl.subscribe(photoUrl => this.photoUrl = photoUrl);
  }
  login() {
this.authService.login(this.model).subscribe(data => {
  this.alertity.succes('logged in successfully');
}, error => {
   this.alertity.error(error);
}, () => {
  this.router.navigate(['/members']);
});

}

logout() {
this.authService.userToken = null;
this.authService.currentUser = null;
localStorage.removeItem('token');
localStorage.removeItem('user');
this.alertity.message('logged out');
this.router.navigate(['/home']);


}

loggedIn() {
return this.authService.loggdIn();
}
}
