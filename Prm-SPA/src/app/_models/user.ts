import { Article } from './article';
import { Photo } from './photo';

export interface User {
  id: number;
  userName: string;
  surname: string;
  status: string;
  created: Date;
  lastActive: Date;
  photoSrc: string;
  city: string;

  photos?: Photo[];
  roles?: string[];
  createdArticles: Article[];
  subscribedArticles: Article[];
}
