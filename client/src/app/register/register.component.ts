import { Component, Input, Output, EventEmitter, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { EventManager } from '@angular/platform-browser';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  private accountService = inject(AccountService);


  @Input() usersFromHomeComponent: any;
  @Output() cancelRegister = new EventEmitter();
//cancelRegister = output<boolean>(); //moze i ovako da se zapise

  model: any = {};

  register() {
    console.log(this.model);
    this.accountService.register(this.model).subscribe({
      next: response => {
        console.log(response);
        this.cancel();
      },
      error: error=> console.log(error)
      
      
    });
  }

  cancel(){
    this.cancelRegister.emit(false);
  }
}
