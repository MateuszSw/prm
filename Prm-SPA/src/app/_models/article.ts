import { Question } from './question';
import { User } from './user';

export interface Article {
  id: number;
  author: User;
  title: string;
  content: string;
  students: User[];
  test: string;
  questions: Question[];
}
