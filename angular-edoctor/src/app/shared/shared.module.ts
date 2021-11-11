import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import {BsDatepickerModule} from 'ngx-bootstrap/datepicker';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { TextInputComponent } from './components/text-input/text-input.component';
import { DateInputComponent } from './components/date-input/date-input.component';
import { MultipleSelectorComponent } from './components/multiple-selector/multiple-selector.component';
import { LeafletModule} from '@asymmetrik/ngx-leaflet';
import { MapComponent } from './components/map/map.component';
import 'leaflet/dist/images/marker-icon-2x.png';
import { PagerComponent } from './components/pager/pager.component';
import { HasRoleDirective } from './directives/has-role.directive';


@NgModule({
  declarations: [
    TextInputComponent,
    DateInputComponent,
    MultipleSelectorComponent,
    MapComponent,
    PagerComponent,
    HasRoleDirective
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    BsDropdownModule.forRoot(),
    CollapseModule.forRoot(),
    BsDatepickerModule.forRoot(),
    LeafletModule,
    PaginationModule.forRoot()
  ],
  exports: [
    BsDropdownModule,
    CollapseModule,
    ReactiveFormsModule,
    TextInputComponent,
    DateInputComponent,
    MultipleSelectorComponent,
    LeafletModule,
    BsDatepickerModule,
    PaginationModule,
    PagerComponent,
    MapComponent,
  HasRoleDirective  ]

})
export class SharedModule { }
