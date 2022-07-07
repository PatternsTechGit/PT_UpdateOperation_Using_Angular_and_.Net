import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Account, AccountListsResponse, GetAccountResponse } from '../models/account';
import { environment } from '../../environments/environment';
import { ApiResponse } from '../models/api-response';
//import { ApiResponse } from "..models//api-response";

@Injectable({
  providedIn: 'root',
})
export default class AccountsService {
  constructor(private httpClient: HttpClient) { }

  getAllAccountsPaginated(pageIndex: number, pageSize: number): Observable<AccountListsResponse> {
    return this.httpClient.get<AccountListsResponse>(`${environment.apiUrlBase}Accounts/GetAllAccountsPaginated?pageIndex=${pageIndex}&pageSize=${pageSize}`);
  }

  openAccount(account: Account): Observable<ApiResponse> {
    const headers = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    }
    console.log(account);
    return this.httpClient.post<ApiResponse>(`${environment.apiUrlBase}Accounts/OpenAccount`, JSON.stringify(account), headers);
  }

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
}