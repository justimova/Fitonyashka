export interface IUserRegister {
  username: string;
  email: string;
  password: string;
}

export interface IUserLogin {
  username: string;
  password: string;
}

export interface IUserInfo {
  userId: number;
  email: string;
  username: string;
  firstName: string;
  birthday: Date;
  gender: number;
  height: number;
  weight: number;
  avatarFileName: string;
}

export interface AvatarUploadState {
  progress: number;
  avatarUrl: string | null;
  isDone: boolean;
}

export interface IUserProfileUpdate {
  userId: number;
  email: string;
  username: string;
  firstName: string;
  birthday: Date;
  gender: number;
  height: number;
  weight: number;
}
