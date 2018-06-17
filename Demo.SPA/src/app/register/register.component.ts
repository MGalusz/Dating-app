import { Router } from '@angular/router';
import { AlertityService } from './../_services/alertity.service';
import { AuthService } from './../_services/auth.service';
import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { User } from '../_models/User';



@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  user: User;
  registerForm: FormGroup;
  bsConfig: Partial<BsDatepickerConfig>;

  constructor(
    private authService: AuthService,
    private alertity: AlertityService,
    private fb: FormBuilder,
    private router: Router,
  ) { }

  ngOnInit() {
    this.bsConfig = {
      containerClass: 'theme-red'
    };
    this.createRegisterForm();
  }
  createRegisterForm() {
    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: [null, Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['',
        [
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(8)
        ]
      ],
      confirmPassword: ['', Validators.required]
    }, { validators: this.passwordMatchValidator });
  }

  passwordMatchValidator(form: FormGroup) {
    return form.get('password').value === form.get('confirmPassword').value ? null : { 'mismatch': true };
  }
  register() {
    if (this.registerForm.valid) {
     this.user = Object.assign({}, this.registerForm.value);
    this.authService.register(this.user).subscribe( () => {
     this.alertity.succes('registianion succes');
     }, error => {
      this.alertity.error(error);
    }, () => {
      this.authService.login(this.user).subscribe( () =>{
        this.router.navigate(['/members']);
      });
    });
    }

  }

  cancel() {
    this.cancelRegister.emit(false);
    this.alertity.message('cancelled');
  }

}
