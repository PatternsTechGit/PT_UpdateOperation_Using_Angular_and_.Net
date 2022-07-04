import { Component, OnInit, ViewChild } from '@angular/core';
import { Account } from './models/account';
import { lineGraphData } from './models/line-graph-data';
import AccountsService from './services/accounts.service';
import { TransactionService } from './services/transaction.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from '../environments/environment';
import { map, tap } from 'rxjs';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {

  title = 'BBBankUI';
  lineGraphData: lineGraphData;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  accounts: Array<Account>;
  // current data shown on the grid 
  dataSource: MatTableDataSource<Account>;
  resultCount: number;
  pageSize = environment.gridDefaultPageSize;

  displayedColumns: string[] = [
    'accountTitle',
    'accountNumber',
    'currentBalance',
    'email',
    'phoneNumber',
    'accountStatus',
    'button'
  ];


  constructor(private transactionService: TransactionService, private accountService: AccountsService) { }

  ngOnInit(): void {
    if(this.paginator.pageSize==undefined)
    this.paginator.pageSize=this.pageSize;
   this.loadAccounts(this.paginator.pageIndex, this.paginator.pageSize); 

    this.transactionService
      .GetLast12MonthBalances('aa45e3c9-261d-41fe-a1b0-5b4dcf79cfd3')
      .subscribe({
        next: (data) => {
          this.lineGraphData = data;
        },
        error: (error) => {
          console.log(error);
        },
      });
  }
  ngAfterViewInit(): void {
    this.paginator.page
      .pipe(
        // after view is initialized we are sure that paginator is populated by now
        // paginator emits events when user selects new page. so we are loading page specific data from server.
        tap(() => this.loadAccounts(this.paginator.pageIndex, this.paginator.pageSize))
      )
      .subscribe();
  }

  loadAccounts(pageIndex: number, pageSize: number) {

    // picking up pageIndex and pageSize information from the paginator.
    this.accountService.getAllAccountsPaginated(pageIndex, pageSize).subscribe({
      
      next: (data) => {
        this.paginator.page
        // whenever new set of data comes from server we reinitialize the datastore.
        this.dataSource = new MatTableDataSource(data.accounts);
        this.dataSource.sort = this.sort;
        this.resultCount = data.resultCount;
        this.accounts  = data.accounts;

      },
      error: (error) => {
        console.log(error);
      },
    });
  }

  applyFilter(event: Event) {
    // picking up filter value from text box
    const filterValue = (event.target as HTMLInputElement).value;
    // filtering each property of object array based on filter criteria. 
    // each time data is to be filtered is picked from this.accounts because it has original copy if data returned form teh server
    const filteredData = this.accounts.filter(x => x.accountTitle.includes(filterValue) ||
      x.currentBalance.toString().includes(filterValue) ||
      x.user.email.includes(filterValue) ||
      x.user.phoneNumber.includes(filterValue) ||
      x.accountNumber.includes(filterValue))
    // initializing the datastore with filtered data.
    this.dataSource = new MatTableDataSource(filteredData);
  }
}
