import { OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';

export abstract class ComponentBase implements OnInit, OnDestroy {
    private subscriptions: Array<Subscription> = [];
    constructor() { }

    abstract onInit();
    abstract onDestroy();

    handleSubscription(item: Subscription): void {
        this.subscriptions.push(item);
    }

    ngOnInit(): void {
        this.onInit();
    }

    ngOnDestroy(): void {
        this.onDestroy();
        this.subscriptions.forEach(item => {
            item.unsubscribe();
        });
    }
}
