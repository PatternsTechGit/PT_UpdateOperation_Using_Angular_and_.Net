# Update Operation using Angular & Asp.net Core


 
## About this exercise

**Backend Code Base:**

* Previously we have developed an api solution in asp.net core with EF Code first approach to generate Database of a fictitious bank application called BBBank.
* We have applied **Data Seeding** to the database.
* We have created an `AccountsController` with api function `GetAllAccountsPaginated` which returns accounts using pagination mechanism as below.


![AccountDetails](https://user-images.githubusercontent.com/100709775/174651287-8d6a252e-358c-4d6a-864b-838b908757cf.PNG)

**Frontend Code Base:**

Previously, we scaffolded a new Angular application in which we have

* FontAwesome library for icons.
* Bootstrap library for styling.
* We have implemented data grid using Angular Material's mat-table in BBankUI project.
* We have implemented pagination using mat-table.
* We have implemented sorting using mat-table.
* We have implemented searching and filtering.
* We have **Manage** button in data grid and on button click event the page is navigated to `create-account` component. 


![DataGrid Updated video](https://user-images.githubusercontent.com/100709775/176945677-467a14c3-28ca-42f4-8a17-8f0d6ab1ebc5.gif)

For previous implementation check [Data Grid Lab](https://github.com/PatternsTechGit/PT_DataGrid)

## In this exercise

 * We will implement method e.g. **GetAccountById** to retrieve account by Id.
 * We will implement  **UpdateAccount** method to update the respective account.
 * We will implement parameterized navigation in Angular.
 * We will updating account using **Forms** in Angular.


 Here are the steps to begin with :

# Server Side Implementation 

## Step 1 : Setup Account Controller

As we already have `AccountController` in BBankAPI project, we will create a new **HttpGet** method `GetAccountById` which will be calling the `AccountService` to get the specific account by Id.
We wll create a new **HttpPut** method  `UpdateAccount` which will be calling the `AccountService` to update the account information in database.

 The AccountController will looks like below :

```cs
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : Controller
    {
        [HttpPut]
        [Route("UpdateAccount")]
        public async Task<ActionResult> UpdateAccount(Account account)
        {
            try
            {
                await _accountsService.UpdateAccount(account);
                return new OkObjectResult("New Account Created.");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }
        [HttpGet]
        [Route("GetAccountById/{accountId}")]
        public async Task<ActionResult> GetAccountById(string accountId)
        {
            try
            {
                var account = await _accountsService.GetAccountByID(accountId);
                return new OkObjectResult(account);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }
    }
```

## Step 2 : Setup IAccountsService Interface

In `IAccountsService` interface we will create new methods `GetAccountById` and `UpdateAccount` as below :

```cs
   public interface IAccountsService
    {
        Task UpdateAccount(Account account);
        Task<Account> GetAccountByID(string id);
    }
```

## Step 3 : Implement IAccountsService

In `AccountService` class we will implement the `GetAccountById` and `UpdateAccount` interfaces which will perform there respect behavior as below :

```cs
public class AccountService : IAccountsService
    {
        public async Task UpdateAccount(Account account)
        {
            this._bbBankContext.Users.Update(account.User);
            this._bbBankContext.Accounts.Update(account);
            await this._bbBankContext.SaveChangesAsync();
        }

        public async Task<Account> GetAccountByID(string id)
        {
            return await _bbBankContext.Accounts.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
        }         
    }
```

Run the API project and see its working.



# Client Side Implementation 
## Step 1 : Setup Accounts Service 
We will create new methods `getAccountById` and `updateAccount` in `accounts.service.ts` which will be getting the account by Id and update the account as below :

```ts
 getAccountById(accountId: string): Observable<GetAccountResponse> {
    return this.httpClient.get<GetAccountResponse>(`${environment.apiUrlBase}Accounts/GetAccountById/${accountId}`);
  }

  updateAccount(account: Account): Observable<ApiResponse> {
    const headers = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    }
    return this.httpClient.put<ApiResponse>(`${environment.apiUrlBase}Accounts/UpdateAccount`, JSON.stringify(account), headers);
  }
```



## Step 2 : SetUp Account Response 
To setup the account response we will create a new class `GetAccountResponse` in `Account.ts` that will have account object as below :

```ts
export class GetAccountResponse {
  account: Account;
}
```


## Step 3 : Navigation with Query Params

In `manage-account.component.ts` we will add the **accountId** in query parameters of **router** to  navigate to `create-account` component. The `create-account` component will be using this accountId for fetching the account details by this accountId.

To navigate with query parameters we will be adding value as below: 

```ts
  this.router.navigate(['create-account'], { queryParams: { accountId: account.id } })
```

## Step 4 : Read Query Params
In `create-account.component.ts` we will receive the accountId from query params, to do this we will inject **ActivatedRoute** in constructor. We will subscribe `queryParamMap` to read the query params.

```ts
constructor(private azureAdService: AzureAdService, private accountsService: AccountsService,  public router: Router, private activatedRoute: ActivatedRoute) {
    this.activatedRoute.queryParamMap.subscribe((params: any) => {
      if (params.params.accountId == null) {
         // if parameter was not passed means this component is to be used to create new account.
      }
      else {
       // if parameter is passed then populating the account object (that is two way binded to form controls) with the object passed in
       }
    })
  }
```

If the parameter is not passed then we will be initializing the empty form.

Otherwise we will be calling `getAccountById` method of `AccountsService` to get the account information by accountId.

we will create `isEditMode` variable and set it as **false** if accountId is not passed otherwise we will set it as **true**. 

Here is the code as below : 

```ts
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
```

## Step 5 : Create/Update Account on Submit

On `onSubmit` function we will check the `isEditMode` value. If its **true** then we will call `updateAccount` method of `AccountService`. If the value is  then we will call  `openAccount` method of `AccountService` as below : 

```ts
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
```
 
 ## Step 6 : Disable Input on Edit Mode

We will `disabled` the input fields if the `isEditMode` is **true**, for this we will add disabled property on select user dropdown as below :  

```html
<select class="form-control" name="user" id="user" [(ngModel)]="selectedAdUser"
                      (change)="onAdUserSelect()" [disabled]="isEditMode">
```

We will add disabled property to **AccountNumber**, **Current Balance** and **Submit** button as below :
```html
<form class="example-container" (ngSubmit)="onSubmit(accountForm)" #accountForm="ngForm" novalidate>
    <div class="row">
      <div class="col-12">
        <div class="card card-user">
          <div class="card-body">
            <div class="author">
              <div class="block block-one"></div>
              <div class="block block-two"></div>
              <div class="block block-three"></div>
              <div class="block block-four"></div>
              <!-- all the elements in the form are two way binded to a property called account of type Account.  -->
              <div class="avatar-cont">
                <img id="photo" alt="..." class="avatar" src="{{account.user.profilePicUrl}}" />
                <a href="javascript:void(0)" class="btn-change-avatar"><i class="fas fa-camera"></i></a>
              </div>
  
            </div>
            <div class="row mt-3">
              <!-- Custom Validator https://www.digitalocean.com/community/tutorials/angular-custom-validation -->
              <div class="col-sm-12 ml-auto mr-auto">
                <div class="amount-cont">
                  <div class="form-row row">
                    <!-- currently selected active directory user will be in selectedAdUser -->
                    <!-- on Select's selection change "onAdUserSelect" function will be called -->
                    <select class="form-control" name="user" id="user" [(ngModel)]="selectedAdUser"
                      (change)="onAdUserSelect()" [disabled]="isEditMode">
                      <option [ngValue]="null" selected disabled>Select Azure AD User</option>
                      <!-- All AD Users fetched from the server are in array of type azureAdUser called azureAdUsers  -->
                      <!-- looping on that array to populate Select control and setting the individual value to adUser and picking up displayName from it for display -->
                      <option *ngFor="let adUser of azureAdUsers" [ngValue]="adUser">{{adUser?.displayName}}</option>
                    </select>
                  </div>
                  <div class="form-row row">
                    <div class="col-6">
                      <div class="my-1">
                        <label for="accountTitle">Account Title</label>
                        <!-- #accountTitle gives unique name of this control that is later referenced in validation. -->
                        <!-- this control has required, maxlength, and minlength validators. -->
                        <!-- and this control is two way binded to  account.accountTitle property -->
                        <input #accountTitle="ngModel" type="text" class="form-control" id="accountTitle"
                          placeholder="Account Title" [(ngModel)]="account.accountTitle" name="accountTitle" required
                          minlength="4" maxlength="30" />
                      </div>
                      <div class="my-1">
                        <label for="accountNumber">Account Number</label>
                        <input #accountNumber="ngModel" type="text" class="form-control" id="accountNumber"
                          placeholder="Account Number" [(ngModel)]="account.accountNumber" name="accountNumber"
                          required  [disabled]="isEditMode" />
                      </div>
                      <div class="my-1">
                        <label for="openingBalance">Opening Balance</label>
                        <div class="input-group mb-2">
                          <div class="input-group-prepend">
                            <div class="input-group-text">$</div>
                          </div>
                          <input type="number" class="form-control" id="currentBalance" placeholder="Current Balance"
                            [(ngModel)]="account.currentBalance" name="currentBalance"
                            onKeyPress="if(this.value.length==10) return false;" [disabled]="isEditMode" />
                        </div>
                      </div>
                    </div>
                    <div class="col-6">
                      <div class="my-1">
                        <label for="phoneNumber">Phone Number</label>
                        <input type="tel" class="form-control" id="phoneNumber" placeholder="Phone Number"
                          [(ngModel)]="account.user.phoneNumber" name="phoneNumber"
                          onKeyPress="if(this.value.length==10) return false;" />
                      </div>
                      <div class="my-1">
                        <label for="email">Email</label>
                        <!-- special case where we are validating this text box using an Email Reg Ex along with other built in validators. -->
                        <input type="email" class="form-control" placeholder="Email" [(ngModel)]="account.user.email"
                          id="email" name="email" #email="ngModel" required minlength="6"
                          pattern="^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$" />
                      </div>
                      <div class="my-1">
                        <label for="accountStatus">Account Status</label>
  
                        <!-- two radio buttons with same name binded to same model but different ids and different values make it work in a group  -->
                        <div class="segmented-control">
                          <input type="radio" name="accountStatus" id="accountStatus-1" value="Active"
                            [(ngModel)]="account.accountStatus" />
                          <input type="radio" name="accountStatus" id="accountStatus-2" value="InActive"
                            [(ngModel)]="account.accountStatus" />
  
                          <label for="accountStatus-1" data-value="Active">Active</label>
                          <label for="accountStatus-2" data-value="Inactive">Inactive</label>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <!-- x -->
  
  
    <div class="row">
      <div class="col-12">
        <!-- This block/set of error texts will only appear if accountTitle has any errors and accountTitle is touched ot dirty -->
        <div class="error-message" *ngIf="
            accountTitle.errors && (accountTitle.dirty || accountTitle.touched)
          ">
          <!-- with the set of error messages this particular error will only be shown if "required" validation is failed.  -->
          <p class="text-warning" [hidden]="!accountTitle.errors?.['required']">
            <b>Warning:</b> That Account Title is required.
          </p>
          <p class="text-warning" [hidden]="!accountTitle.errors?.['minlength']">
            <b>Warning:</b> Account Title must be greater than 3 characters.
          </p>
        </div>
        <!-- similar concept is applied on other validation messages -->
        <div class="error-message" *ngIf="email.errors && (email.dirty || email.touched)">
          <p class="text-warning" [hidden]="!email.errors?.['required']">
            <b>Warning:</b> That Email is required.
          </p>
          <p class="text-warning" [hidden]="!email.errors?.['pattern']">
            <b>Warning:</b> Email is not in correct format.
          </p>
          <p class="text-warning" [hidden]="!email.errors?.['minlength']">
            <b>Warning:</b> Email must be greater than 6 characters.
          </p>
        </div>
        <div class="error-message" *ngIf="accountNumber.errors && (accountNumber.dirty || accountNumber.touched)">
          <p class="text-warning" [hidden]="!accountNumber.errors?.['required']">
            <b>Warning:</b> Account Number is required.
          </p>
        </div>
        <div class="row">
          <div>
            <div class="row">
              <div class="col-6 mb-3">
                <button class="btn btn-cancel btn-block" type="button">Cancel</button>
              </div>
              <div class="col-6">
                <!-- like we can track validation of individual forms we can also track validation of entire form -->
                <!-- Create buttons caus the form to trigger submit behavior but is disabled until entire form is in valid sate. -->
                <button [disabled]="!accountForm.form.valid" class="btn btn-danger btn-create btn-block" type="submit">
                  {{isEditMode ? "Submit" : "Create"}}
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </form> 
```

## Final Output 

Run the project and see its working as below :

![New Recording - 7_13_2022, 10_34_49 PM](https://user-images.githubusercontent.com/100709775/178795973-90a4e8c2-bbf9-4606-97cf-a92bc04293c7.gif)


