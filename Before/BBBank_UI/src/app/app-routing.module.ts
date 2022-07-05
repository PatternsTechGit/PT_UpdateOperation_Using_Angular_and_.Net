import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateAccountComponent } from './create-account/create-account.component';
import { ManageAccountComponent } from './manage-account/manage-account.component';


const  routes: Routes = [
  { path: '', component: ManageAccountComponent},
    { path: 'create-account', component: CreateAccountComponent },
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {

  

  
  
 }
