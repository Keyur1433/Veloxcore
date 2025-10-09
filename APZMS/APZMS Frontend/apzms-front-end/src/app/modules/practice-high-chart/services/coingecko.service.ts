import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { Coin } from '../models/coin.model';
import { catchError, map, of, startWith, tap } from 'rxjs';
import { CoinsState } from '../models/coin.model';

@Injectable({
  providedIn: 'root'
})
export class CoingeckoService {
  http = inject(HttpClient)

  coinsState = toSignal(this.http.get<Coin[]>("https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd").pipe(
    tap(
      data => {
        // debugger;
        console.log(data)
      }
    ),
    map(data => ({ loading: false, error: null, data })),
    startWith({ loading: true, error: null, data: [] }),
    catchError(error => of({ loading: false, error, data: [] }))
  ), { initialValue: { loading: true, error: null, data: [] } })
}