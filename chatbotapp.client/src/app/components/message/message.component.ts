import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Message } from '../../models/message';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.css']
})
export class MessageComponent {
  @Input() message!: Message;
  @Output() rate = new EventEmitter<{ id: number; rating: number }>();

  setRating(value: number) {
    this.rate.emit({ id: this.message.id, rating: value });
  }
}
