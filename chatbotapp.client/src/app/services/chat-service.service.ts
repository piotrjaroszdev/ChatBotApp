import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Message } from '../models/message';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ChatService {
  private apiUrl = 'https://localhost:7028/api/chat';

  constructor(private http: HttpClient) {}

  getHistory(): Observable<Message[]> {
    return this.http.get<Message[]>(`${this.apiUrl}/history`);
  }

  sendMessage(message: Message): Observable<Message> {
    return this.http.post<Message>(`${this.apiUrl}/send`, message);
  }

  rateMessage(id: number, rating: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/rate?messageId=${id}&rating=${rating}`, {});
  }
}
