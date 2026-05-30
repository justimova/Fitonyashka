import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IResult } from '../../models/result';
import { IGoalCreate, IGoalInfo } from '../../models/goal/goal';

@Injectable({
  providedIn: 'root'
})
export class GoalService {
  constructor(
    private http: HttpClient,
  ) { }

  public setGoal(model: IGoalCreate): Observable<IResult> {
    return this.http.post<IResult>('/api/goal', model);
  }

  public getCurrentGoal(): Observable<IGoalInfo | null> {
    return this.http.get<IGoalInfo | null>('/api/goal/currentGoal');
  }

  public deleteGoal(id: number): Observable<IResult> {
    return this.http.delete<IResult>(`/api/goal/${id}`);
  }
}
