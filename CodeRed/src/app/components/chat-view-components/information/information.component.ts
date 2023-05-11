import { Component } from '@angular/core';
import { Subscription, interval } from 'rxjs';

@Component({
  selector: 'app-information',
  templateUrl: './information.component.html',
  styleUrls: ['./information.component.css'],
})
export class InformationComponent {
  infoList = [
    {
      id: 1,
      icon: 'fas fa-lightbulb',
      title: 'Quick Answers',
      description:
        'My vast knowledge base makes me a great tool for finding quick answers to your questions. Simply ask me a question on any topic, from science and technology to history and current events, and I will use my sophisticated algorithms to search the internet and other sources for the most relevant information. I can provide you with a concise and accurate answer to your question in a matter of seconds, saving you time and effort.',
      link: '',
      actionText: 'Start chatting',
      actionIcon: 'fas fa-comments',
    },
    {
      id: 2,
      icon: 'fas fa-microphone',
      title: 'Speech Recognition',
      description:
        "With my speech recognition feature, you can dictate messages or take notes using just your voice. This can be a great time-saver, especially for people who find it difficult to type or have a lot of information to transcribe. My speech recognition feature is highly accurate and can transcribe your speech in real-time, so you can focus on what you're saying rather than worrying about the mechanics of typing.",
      link: '',
      actionText: 'Try it out',
      actionIcon: 'fas fa-microphone-alt',
    },
    {
      id: 3,
      icon: 'fas fa-file-alt',
      title: 'Document Summarization',
      description:
        'My document summarization feature can save you time by quickly summarizing long documents or articles. Simply upload the document or article to me, and I will use my advanced algorithms to extract the key information and create a concise summary. This can be a great tool for students, researchers, or anyone who needs to quickly extract the main points from a long piece of text.',
      link: '',
      actionText: 'Summarize now',
      actionIcon: 'fas fa-scroll',
    },
    {
      id: 4,
      icon: 'fas fa-tasks',
      title: 'Task Management',
      description:
        "My task management feature can help you stay organized by creating and managing your to-do lists and reminders. Simply tell me what you need to do, and I will create a task for you and remind you when it's due. You can also use me to set up recurring tasks or reminders, so you never forget an important deadline again.",
      link: '',
      actionText: 'Get started',
      actionIcon: 'fas fa-clipboard-check',
    },
    {
      id: 5,
      icon: 'fas fa-chart-line',
      title: 'Data Analysis',
      description:
        "I can help with data analysis by assisting you with cleaning and analyzing your data, and creating visualizations to help you better understand your results. Whether you're a researcher, data scientist, or simply someone who needs to analyze data, my advanced algorithms can help you get the job done quickly and accurately.",
      link: '',
      actionText: 'Analyze data',
      actionIcon: 'fas fa-chart-bar',
    },
    {
      id: 6,
      icon: 'fas fa-camera',
      title: 'Image Recognition',
      description:
        "My image recognition feature can help you identify objects and people in your photos. Simply upload a photo to me, and I will use my advanced algorithms to analyze the image and provide you with information on what's in the photo. This can be a great tool for photographers, social media managers, or anyone who needs to quickly identify objects or people in photos.",
      link: '',
      actionText: 'Try it now',
      actionIcon: 'fas fa-images',
    },
    {
      id: 7,
      icon: 'fas fa-translate',
      title: 'Language Translation',
      description:
        'My language translation feature can help you translate text between different languages. Simply enter the text you need to translate and the target language, and I will provide you with a translated version. This can be a great tool for people who need to communicate in different languages, or for those who are learning a new language.',
      link: '',
      actionText: 'Translate now',
      actionIcon: 'fas fa-language',
    },
    {
      id: 8,
      icon: 'fas fa-clock',
      title: 'Time Zone Converter',
      description:
        'My time zone converter can help you easily convert between different time zones. This can be a great tool for scheduling meetings or coordinating with people in different parts of the world. Simply tell me what time it is in your current location and where you need to know the time, and I will provide you with the correct time in the other time zone.',
      link: '',
      actionText: 'Convert now',
      actionIcon: 'fas fa-globe',
    }, {
      icon: 'fas fa-calculator',
      title: 'Financial Calculator',
      description: "My financial calculator feature allows you to quickly calculate interest, payments, and other financial metrics. Simply enter the relevant data and I'll provide you with the answer. This can be a great tool for anyone managing their finances or making financial decisions.",
      link:'',
      actionText: 'Calculate now',
      actionIcon: 'fas fa-dollar'
      }, {
        icon: 'fas fa-weather',
        title: 'Weather Forecast',
        description: "My weather forecast feature can provide you with up-to-date information on the weather in your area. Simply enter your location and I'll provide you with the forecast. This can be a great tool for anyone planning outdoor activities or trying to dress appropriately for the weather.",
        link:'',
        actionText: 'Check now',
        actionIcon: 'fas fa-sun'
        },
        {
          icon: 'fas fa-music',
          title: 'Music Recommendation',
          description: "My music recommendation feature can help you discover new artists and songs based on your preferences. Simply tell me what you like and I'll provide you with a list of options. This can be a great tool for music lovers or anyone looking to broaden their musical horizons.",
          link:'',
          actionText: 'Discover now',
          actionIcon: 'fas fa-note'
          },
          {
            icon: 'fas fa-fitness',
            title: 'Fitness Tracker',
            description: "My fitness tracker feature allows you to track your workouts, set goals, and monitor your progress. Simply enter your workout information and I'll take care of the rest. This can be a great tool for anyone trying to stay active or reach their fitness goals.",
            link:'',
            actionText: 'Track now',
            actionIcon: 'fas fa-heart'
            },
            {
              icon: 'fas fa-recipes',
              title: 'Recipe Finder',
              description: "My recipe finder feature can help you find delicious recipes based on your preferences and dietary restrictions. Simply enter your criteria and I'll provide you with a list of options. This can be a great tool for anyone looking to try new recipes or eat healthier.",
              link:'',
              actionText: 'Find now',
              actionIcon: 'fas fa-utensils'
              },
              {
                icon: 'fas fa-news',
                title: 'News Reader',
                description: "My news reader feature can provide you with up-to-date news stories from a variety of sources. Simply tell me what topics you're interested in and I'll provide you with a list of articles. This can be a great tool for anyone trying to stay informed about current events.",
                link:'',
                actionText: 'Read now',
                actionIcon: 'fas fa-newspaper'
                }
  ];

  visibleInfos: any[] = [];

  private subscription: Subscription = new Subscription();
  
  private counter: number = 0;
  
  ngOnInit(): void {
    // Display the first 6 items in the list initially
    this.visibleInfos = this.infoList.slice(0, 6);
  
    // Rotate the visible items every 30 seconds
    const rotateInterval = interval(30000);
    this.subscription = rotateInterval.subscribe(() => {
      const firstVisibleInfo = this.visibleInfos.shift();
      this.visibleInfos.push(this.getNextInfo(firstVisibleInfo));
    });
  }
  
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
  
  getNextInfo(currentInfo: any): any {
    const currentIndex = this.infoList.indexOf(currentInfo);
    let nextIndex = currentIndex === this.infoList.length - 1 ? 0 : currentIndex + 1;
    
    // Find the next available info that is not already visible
    let nextInfo = this.infoList[nextIndex];
    while (this.visibleInfos.find((info) => info.id === nextInfo.id)) {
      this.counter++;
      if (this.counter >= this.infoList.length) {
        this.counter = 0;
      }
      nextIndex = this.counter;
      nextInfo = this.infoList[nextIndex];
    }
  
    return nextInfo;
  }
  
}
