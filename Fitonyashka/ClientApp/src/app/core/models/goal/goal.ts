export interface IGoalCreate {
  initialWeight: number;
  targetWeight: number;
}

export interface IGoalUpdate {
  id: number;
  initialWeight: number;
  targetWeight: number;
}

export interface IGoalInfo {
  id: number;
  startDate: string;
  initialWeight: number;
  targetWeight: number;
}
