import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { IBmiRange, ICalculatedBmi } from "../../models/bmi/bmi";

@Injectable({
  providedIn: 'root'
})
export class BmiService {
  constructor(
    private http: HttpClient,
  ) { }

  public calculate(height: number, weight: number): Observable<ICalculatedBmi> {
    return this.http.get<ICalculatedBmi>(`/api/bmi?height=${height}&weight=${weight}`) as Observable<ICalculatedBmi>;
  }

  public getCategory(): Observable<IBmiRange[]> {
    return this.http.get<IBmiRange[]>('/api/bmi/categories');
  }
}
