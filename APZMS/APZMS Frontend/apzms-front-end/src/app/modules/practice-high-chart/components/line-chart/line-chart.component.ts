import { AfterViewChecked, AfterViewInit, Component, effect, inject, OnInit } from '@angular/core';
import { CoingeckoService } from '../../services/coingecko.service';

@Component({
  selector: 'app-line-chart',
  standalone: false,
  templateUrl: './line-chart.component.html',
  styleUrls: ['./line-chart.component.css']
})
export class LineChartComponent implements OnInit {

  constructor(public coinsService: CoingeckoService) { }

  ngOnInit() {

  }

}
