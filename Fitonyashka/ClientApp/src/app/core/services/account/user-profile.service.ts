import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { IUserInfo, IUserProfileUpdate } from "../../models/account/user";

@Injectable({
  providedIn: 'root'
})
export class UserProfileService {
  private tokenKey: string = 'auth_token';

  constructor(
    private http: HttpClient,
  ) { }

  public getCurrentUser(): Observable<IUserInfo> {
    return this.http.get<IUserInfo>('/api/userProfile/currentUser') as Observable<IUserInfo>;
  }

  public updateUser(user: IUserProfileUpdate): Observable<any> {
      return this.http.put('/api/userProfile', user);
    }
}
