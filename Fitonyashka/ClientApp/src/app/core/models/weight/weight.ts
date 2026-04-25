export interface IWeight {
  id: number;
  date: string;
  weight: number;
}

export interface IWeightInfo {
  id: number;
  date: string;
  weight: number;
}

export interface IWeightBase {
  date: string;
  weight: number;
}

export interface IWeightCreate extends IWeightBase {}

export interface IWeightUpdate extends IWeightBase {
  id: number;
}
