import { Component, OnInit } from '@angular/core';
import { Message } from '../../models/message';
import { ChatService } from '../../services/chat-service.service';
import { MessageComponent } from '../message/message.component';


@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css'],

})
export class ChatComponent implements OnInit {
  messages: Message[] = [];
  newContent = '';
  isTyping = false;

  constructor(private chatService: ChatService) {}

  ngOnInit(): void {
    this.chatService.getHistory().subscribe(history => this.messages = history);
  }

  send(): void {
    const userMessage: Message = {
      id: 0,
      sender: 'user',
      content: this.newContent,
      timestamp: new Date().toISOString()
    };

    this.messages.push(userMessage);
    this.isTyping = true;
    this.newContent = '';

    this.chatService.sendMessage(userMessage).subscribe(botReply => {
      setTimeout(() => {
        this.messages.push(botReply);
        this.isTyping = false;
      }, this.simulateTypingTime(botReply.content));
    });
  }

  rateMessage(event: { id: number; rating: number }) {
    this.chatService.rateMessage(event.id, event.rating).subscribe(() => {
      const message = this.messages.find(m => m.id === event.id);
      if (message) message.rating = event.rating;
    });
  }

  private simulateTypingTime(content: string): number {
    return Math.min(3000, content.length * 50); // max 3s
  }
}
