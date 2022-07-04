import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AccountListsResponse } from '../models/account';
import { environment } from '../../environments/environment';


@Injectable({
  providedIn: 'root',
})
export default class AccountsService {
  constructor(private httpClient: HttpClient) { }

  getAllAccountsPaginated(pageIndex: number, pageSize: number): Observable<AccountListsResponse> {
    return this.httpClient.get<AccountListsResponse>(`${environment.apiUrlBase}Accounts/GetAllAccountsPaginated?pageIndex=${pageIndex}&pageSize=${pageSize}`);
  }
}