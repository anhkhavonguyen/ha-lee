import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../shared/components/auth/auth.guard';

const appRoutes: Routes = [
  {
    path: '',
    redirectTo: '/products',
    pathMatch: 'full'
  },
  {
    path: 'categories',
    loadChildren: './categories/category.module#CategoryModule',
    canActivate: [AuthGuard]
  },
  {
    path: 'brands',
    loadChildren: './brands/brand.module#BrandModule',
    canActivate: [AuthGuard]
  },
  {
    path: 'products',
    loadChildren: './products/product.module#ProductModule',
    canActivate: [AuthGuard]
  },
  {
    path: 'locations',
    loadChildren: './locations/location.module#LocationModule',
    canActivate: [AuthGuard]
  },
  {
    path: 'fields',
    loadChildren: './fields/field.module#FieldModule',
    canActivate: [AuthGuard]
  },
  {
    path: 'field-templates',
    loadChildren: './field-templates/field-template.module#FieldTemplateModule',
    canActivate: [AuthGuard]
  },
  {
    path: 'assortments',
    loadChildren: './assortments/assortment.module#AssortmentModule',
    canActivate: [AuthGuard]
  },
  {
    path: 'channels',
    loadChildren: './channels/channel.module#ChannelModule',
    canActivate: [AuthGuard]
  },
  {
    path: 'allocations',
    loadChildren: './allocations/allocation.module#AllocationModule',
    canActivate: [AuthGuard]
  },
  {
    path: 'activities',
    loadChildren: './activities/activity.module#ActivityModule',
    canActivate: [AuthGuard]
  },
  {
    path: '**',
    redirectTo: '/products'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(appRoutes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
