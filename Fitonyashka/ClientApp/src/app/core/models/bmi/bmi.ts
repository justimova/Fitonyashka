export interface ICalculatedBmi {
  bmiValue: number;
  bmiCategory: string;
}

export interface ICalculatedWeight {
  weight: number;
}

export interface IBmiRange {
  min: number;
  max: number;
  category: string;
}
