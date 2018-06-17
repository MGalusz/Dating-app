import { AuthService } from './../_services/auth.service';
import { Observable } from 'rxjs/Observable';
import { AlertityService } from './../_services/alertity.service';
import { UserService } from './../_services/user.service';
import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { User } from '../_models/User';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/of';

@Injectable()
export class MemberEditlResolver implements Resolve<User> {

    constructor(private userService: UserService,
         private router: Router,
          private alertity: AlertityService,
        private authService: AuthService) {}
    resolve(route:  ActivatedRouteSnapshot): Observable<User> {
        return this.userService.getUser(this.authService.decodeToken.nameid).catch(error => {
            this.alertity.error('Problem retrieving data');
            this.router.navigate(['/members']);
            return Observable.of(null);
        });
    }
}