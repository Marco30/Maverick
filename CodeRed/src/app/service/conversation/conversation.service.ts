import { Injectable } from '@angular/core';
import { HttpRequestService } from '../httpRequest/http-request.service';
import { Question } from 'src/app/model/question/question';
import { environment } from 'src/environments/environment';
import { Answer } from 'src/app/model/answer/answer';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ConversationService {

  constructor(private httpRequestService: HttpRequestService,) { }

  askTheAI(questionData: Question) : Observable<Answer>{
    const url = environment.ask_question;
    const queryParams = { model: questionData};
    return this.httpRequestService.postData<Question>(url, queryParams);
  }

}
