import { Component, OnInit } from '@angular/core';
import { Validators, UntypedFormBuilder } from '@angular/forms';
import { IUserInfo, IUserProfileUpdate } from '../../../../core/models/account/user';
import { ErrorHandlingService } from '../../../../core/services/error-handling.service';
import { UserProfileService } from 'src/app/core/services/account/user-profile.service';

@Component({
  selector: 'app-profile',
  standalone: false,
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent implements OnInit {
  protected isEditing = false;
  protected isDataLoading = false;
  protected isLoading = false;

  protected userInfo!: IUserInfo;

  protected editForm = this.formBuilder.group({
    userId: [0],
    email: ['', [Validators.required, Validators.email]],
    firstName: ['', [Validators.required]],
    birthday: [new Date().toISOString().split('T')[0], [Validators.required]],
    gender: [0, [Validators.required, Validators.min(1), Validators.max(3)]],
    height: [0, [Validators.required, Validators.min(0), Validators.pattern(/^\d+([.,]\d+)?$/)]],
    weight: [0, [Validators.required, Validators.min(0), Validators.pattern(/^\d+([.,]\d+)?$/)]],
  });

  constructor(
    //private router: Router,
    private errorHandlingService: ErrorHandlingService,
    private formBuilder: UntypedFormBuilder,
    private userProfileService: UserProfileService,
  ) { }

  public ngOnInit() { // OnGET
    this._loadData();
  }

  protected get avatarUrl(): string {
    return this.userInfo.avatarFileName
      ? `assets/UserImages/${this.userInfo.avatarFileName}`
      : 'assets/default-avatar.png';
  }

  protected checkError(controlName: string, errorName: string): boolean {
    const control = this.editForm.get(controlName);
    return control ? control.hasError(errorName) && control.touched : false;
  }
  
  protected onEdit() { // OnPost
    if (this.editForm.invalid) {
      return;
    }

    this.isEditing = true;
    this.userProfileService.updateUser(this.updateModel).subscribe({
      next: (response) => {
        this.isEditing = false; 
        this.errorHandlingService.handleResponse(response);
        if (response.isSuccess) {
          this._loadData();
        }
      },
      error: (error) => {
        this.isEditing = false;
        this.errorHandlingService.handleError(error, 'PROFILE_EDIT_ERROR');
      }
    });
  }

  private get updateModel(): IUserProfileUpdate{
    return {
      userId: this.editForm.controls['userId'].value,
      email: this.editForm.controls['email'].value,
      firstName: this.editForm.controls['firstName'].value,
      birthday: this.editForm.controls['birthday'].value,
      gender: this.editForm.controls['gender'].value,
      height: this.editForm.controls['height'].value,
      weight: this.editForm.controls['weight'].value
    } as IUserProfileUpdate;
  }

  private _loadData() {
    this.isDataLoading = false;
    this.userProfileService.getCurrentUser().subscribe({
      next: (response: IUserInfo) => {
        this.isDataLoading = true;
        this.userInfo = response;
        this.editForm.controls['userId'].patchValue(this.userInfo.userId);
        this.editForm.controls['email'].patchValue(this.userInfo.email);
        this.editForm.controls['firstName'].patchValue(this.userInfo.firstName);
        this.editForm.controls['birthday'].patchValue(this.userInfo.birthday);
        this.editForm.controls['gender'].patchValue(this.userInfo.gender);
        this.editForm.controls['height'].patchValue(this.userInfo.height);
        this.editForm.controls['weight'].patchValue(this.userInfo.weight);
        
        this.editForm.controls['weight'].disable();
      },
      error: (error) => {
        this.isDataLoading = false;
        this.errorHandlingService.handleError(error, 'PROFILE_EDIT_ERROR');
      }
    });
  }
}
