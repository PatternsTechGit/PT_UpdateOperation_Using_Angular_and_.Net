
import { Component, ElementRef, ViewChild,OnInit } from '@angular/core';
// import { Account, User } from './models/account';
// import { AzureAdUser } from './models/azureAdUser';
 import AccountsService from '../services/accounts.service';
 import AzureAdService from '../services/azuread.service';
import { Account, User } from '../models/account';
import { AzureAdUser } from '../models/azureAdUser';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-create-account',
  templateUrl: './create-account.component.html',
  styleUrls: ['./create-account.component.css']
})
export class CreateAccountComponent  {

  @ViewChild('photo') photo: ElementRef;
  // Entire form and its controls are two way binded to this property.
  // This is the property when populated will be send back to server.
  public account: Account;
  // Array used to populate AD Users drop down.
  azureAdUsers: AzureAdUser[];
  // Selected user on drop down.
  selectedAdUser: AzureAdUser;
  isEditMode: boolean;
  constructor(private azureAdService: AzureAdService, private accountsService: AccountsService,  public router: Router, private activatedRoute: ActivatedRoute) {
    this.activatedRoute.queryParamMap.subscribe((params: any) => {
      if (params.params.accountId == null) {
        // if parameter was not passed means this component is to be used to create new account.
        this.isEditMode = false;
        this.initializeEmptyForm();
      }
      else {
        // if parameter is passed then populating the account object (that is two way binded to form controls) with the object passed in
        this.isEditMode = true;
        let accountId = params.params.accountId;
        this.accountsService
        // sending the two way binded and updated account object to the server.
        .getAccountById(accountId)
        .subscribe({
          next: (data:any) => {
           this.account = data
          },
          error: (error) => {
            console.log(error);
          },
        });
      }
    })
  }
  onAdUserSelect() {
    // few of the  properties of account object will be populated from the properties of Azure AD User
    this.account.user.id = this.selectedAdUser.id
    this.account.user.firstName = this.selectedAdUser.givenName;
    this.account.user.lastName = this.selectedAdUser.surname;
    this.account.user.email = this.selectedAdUser.mail;
    /*     this.azureAdService
        .getAzureAdUsersPic(this.selectedAdUser.id)
        .subscribe({
          next: (data) => {
            console.log(data);
            this.photo['src'] = data['value'];
          },
          error: (error) => {
            console.log(error);
          },
        }); */
  }
  
  onSubmit(form:any) {
    // calling update api if component is in edit mode
    // otherwise calling create account api if the form is not in edit mode
    if (this.isEditMode) {
      this.accountsService
        // sending the two way binded and updated account object to the server.
        .updateAccount(this.account)
        .subscribe({
          next: (data) => {
            // once update is performed navigating back to manage accounts page 
            this.router.navigate(['manage-accounts'])
          },
          error: (error) => {
            console.log(error);
          },
        });
    } else {
      this.accountsService
        // sending the two way binded and populated account object to the server to get persisted.
        .openAccount(this.account)
        .subscribe({
          next: (data) => {
            // clearing up the form again after form data is sent to server.
            this.initializeEmptyForm();
          },
          error: (error) => {
            console.log(error);
          },
        });
    }
  }

  initializeEmptyForm() {
    // fetching list of Azure AD Users from azureAdService
    this.azureAdService
      .getAzureAdUsersList()
      .subscribe({
        next: (data:any) => {
          //console.log(data['value']);
          this.azureAdUsers = data['value'];
        },
        error: (error) => {
          console.log(error);
        },
      });
      // Setting up empty or default controls.
    //this.selectedAdUser = null;
    this.account = new Account();
    this.account.accountStatus = 1;
    this.account.user = new User();
    this.account.user.profilePicUrl = '../../assets/images/No-Image.png';
  }

}
