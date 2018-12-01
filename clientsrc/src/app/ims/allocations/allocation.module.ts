import { AllocationsComponent } from './allocations.component';
import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { SharedModule } from 'src/app/shared/shared.module';
import { CommonModule } from '@angular/common';
import { NgSelectModule } from '@ng-select/ng-select';
import { BootstrapModule } from 'src/app/shared/bootstrap.module';

const allocationRoutes: Routes = [
    { path: '', component: AllocationsComponent },
];

@NgModule({
    imports: [
        CommonModule,
        SharedModule,
        RouterModule.forChild(allocationRoutes),
        ReactiveFormsModule,
        NgSelectModule,
        FormsModule,
        BootstrapModule
    ],
    declarations: [
        AllocationsComponent
    ],
    entryComponents: [
    ]
})
export class AllocationModule { }
