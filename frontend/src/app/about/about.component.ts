import { Component } from '@angular/core';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.less']
})
export class AboutComponent {
  name = 'Innocent Sibiya';
  title = 'Software Engineer';
  description = `
    I am a passionate software engineer who loves turning ideas into working software.
    Currently, I am building a personal project designed to sharpen my coding skills.
    Through this journey, I am focusing on writing clean code, exploring new technologies,
    improving problem-solving, and learning software architecture principles.
  `;

  skills = [
    'Clean & maintainable code',
    'Exploring new frameworks',
    'Problem-solving',
    'Software design principles',
    'Continuous learning'
  ];

  technologies = [
    'Angular',
    'TypeScript',
    '.Net C#',
    'HTML & CSS',
    'JavaScript',
    'MongoDB'
  ];

  hobbies = [
    'Gaming',
    'Waching sports and playing fpl',
    'Going to the beach',
    'Sometimes playing chess'
  ];
}
