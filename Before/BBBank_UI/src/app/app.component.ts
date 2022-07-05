import { Component, OnInit, ViewChild } from '@angular/core';
import { lineGraphData } from './models/line-graph-data';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {

  title = 'BBBankUI';
 
  constructor() { }

  ngOnInit(): void {
  }
}
