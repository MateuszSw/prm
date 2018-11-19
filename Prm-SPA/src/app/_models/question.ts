import { Answer } from './answer';
export interface Question {
  isSelectedAnswerCorrect: Boolean;
  value: string;
  answers: Answer[];
}
