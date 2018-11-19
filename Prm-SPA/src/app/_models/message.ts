export interface Message {
  id: number;
  idSender: number;
  senderSurname: string;
  senderImage: string;
  recipientId: number;
  recipientSurname: string;
  recipientImage: string;
  content: string;
  isRead: boolean;
  dateRead: Date;
  messageSent: Date;
}
