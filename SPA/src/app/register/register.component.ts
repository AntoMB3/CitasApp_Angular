import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  registerForm: FormGroup = new FormGroup({});
  maxDate: Date = new Date();
  validationErrors: string[] | undefined;
  
  constructor(private accountService: AccountService,
    private toastr: ToastrService, private fb: FormBuilder, private router: Router) {}

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  initializeForm() {
    this.registerForm = this.fb.group({
      gender:           ["female"],
      username:         ["", Validators.required],
      knownAs:          ["", Validators.required],
      dateOfBirth:      ["", Validators.required],
      city:             ["", Validators.required],
      country:          ["", Validators.required],
      password:         ["", [Validators.required, Validators.minLength(5), Validators.maxLength(15)]],
      confirm_password: ["", [Validators.required, this.matchValues("password")]],
    });
    
    this.registerForm.controls["password"].valueChanges.subscribe({
      next: () => this.registerForm.controls["confirm_password"].updateValueAndValidity()
    });
  }
  

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control.value === control.parent?.get(matchTo)?.value ? null : {_notMatching: true}
    }
  }

  register(): void {
    const dob = this.getDateOnly(this.registerForm.controls['dateOfBirth'].value);
    const values = {...this.registerForm.value, dateOfBirth: dob}  
    console.log(dob)  
    console.log(values)  
    this.accountService.register(values).subscribe({
      next: () => {
        this.router.navigateByUrl("/members");
      },
      error: error => {
        console.log(error);
        this.validationErrors = error;
      }
    });
  }

  cancel(): void {
    this.cancelRegister.emit(false);
  }

  private getDateOnly(dob: string | undefined){
    if (!dob) return;

    let newdob = new Date(dob);
    return new Date(newdob.setMinutes(newdob.getMinutes() - newdob.getTimezoneOffset()))
      .toISOString().substring(0,10);
  }
}
