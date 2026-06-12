import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IResult } from '../../models/result';
import { IGoalCreate, IGoalUpdate, IGoalInfo } from '../../models/goal/goal';

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

  public updateGoal(model: IGoalUpdate): Observable<IResult> {
    return this.http.put<IResult>('/api/goal', model);
  }

  public getCurrentGoal(): Observable<IGoalInfo | null> {
    return this.http.get<IGoalInfo | null>('/api/goal/currentGoal');
  }

  public deleteGoal(id: number): Observable<IResult> {
    return this.http.delete<IResult>(`/api/goal/${id}`);
  }

  public completeGoal(id: number): Observable<IResult> {
    return this.http.post<IResult>(`/api/goal/${id}/complete`, {});
  }

  public getGoalById(id: number): Observable<IGoalInfo> {
    return this.http.get<IGoalInfo>(`/api/goal/${id}`);
  }

  public completeGoalIfNeeded(): Observable<boolean> {
    return this.http.put<boolean>('/api/goal/completeIfNeeded', {});
  }
}
