import { transition, trigger, useAnimation } from '@angular/animations';
import { Component } from '@angular/core';
import { bounce, shakeX, tada } from 'ng-animate';
import { lastValueFrom, timer } from 'rxjs';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css'],
    animations:[trigger("shake",[transition(":increment", useAnimation(shakeX,{params:{timing:2}}))]),
                trigger("bounce",[transition(":increment", useAnimation(bounce,{params:{timing:4}}))]),
                trigger("tada",[transition(":increment", useAnimation(tada,{params:{timing:3}}))])],
    standalone: true
})
export class AppComponent {
  title = 'ngAnimations';
  
  rotate = false;

  first = 0;
  second = 0;
  third = 0;

  constructor() {
  }

  rotateLeft() {

    if(this.rotate == false){ 
    this.rotate = true;
    setTimeout(() => {this.rotate = false;},2000);
    }
  }
  async animateAll(boucle: boolean = false){

     do {
    // 1) Shake 2s
    this.first++;
    await lastValueFrom(timer(2000));

    // 2) Bounce 4s
    this.second++;

    // Lancer Tada 1 sec avant la fin → après 3 sec
    await lastValueFrom(timer(3000));
    this.third++;

    // Attendre la fin du bounce
    await lastValueFrom(timer(3000));

  } while (boucle);

  }

}
