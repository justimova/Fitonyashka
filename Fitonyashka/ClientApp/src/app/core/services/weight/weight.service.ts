import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IResult } from '../../models/result';
import { IWeightCreate, IWeight, IWeightUpdate, IWeightInfo } from '../../models/weight/weight';

@Injectable({
  providedIn: 'root'
})
export class WeightService {

  constructor(
    private http: HttpClient,
  ) { }

  public getWeights(): Observable<IWeight[]> {
    return this.http.get<IWeight[]>('/api/weight');
  }

  public getInfo(id: number): Observable<IWeightInfo> {
    return this.http.get<IWeightInfo>(`/api/weight/${id}`);
  }

  public create(model: IWeightCreate): Observable<IResult> {
    return this.http.post<IResult>('/api/weight', model);
  }

  public update(model: IWeightUpdate): Observable<IResult> {
    return this.http.put<IResult>('/api/weight', model);
  }

  public delete(id: number): Observable<IResult> {
    return this.http.delete<IResult>(`/api/weight/${id}`);
  }
}
