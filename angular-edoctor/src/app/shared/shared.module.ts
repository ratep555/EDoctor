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
import 'leaflet/dist/images/marker-shadow.png';
import 'leaflet/dist/images/marker-icon-2x.png';
import { PagerComponent } from './components/pager/pager.component';
import { HasRoleDirective } from './directives/has-role.directive';
import { ImgInputComponent } from './components/img-input/img-input.component';
import { RatingComponent } from './components/rating/rating.component';
import {NgxPrintModule} from 'ngx-print';
import { ModalModule } from 'ngx-bootstrap/modal';
import { RolesModalComponent } from './components/roles-modal/roles-modal.component';
import { GoogleChartsModule } from 'angular-google-charts';


@NgModule({
  declarations: [
    TextInputComponent,
    DateInputComponent,
    MultipleSelectorComponent,
    MapComponent,
    PagerComponent,
    HasRoleDirective,
    ImgInputComponent,
    RatingComponent,
    RolesModalComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    BsDropdownModule.forRoot(),
    CollapseModule.forRoot(),
    BsDatepickerModule.forRoot(),
    LeafletModule,
    PaginationModule.forRoot(),
    TabsModule.forRoot(),
    NgxPrintModule,
    ModalModule.forRoot(),
    GoogleChartsModule.forRoot()
  ],
  exports: [
    BsDropdownModule,
    CollapseModule,
    ReactiveFormsModule,
    TextInputComponent,
    DateInputComponent,
    MultipleSelectorComponent,
    LeafletModule,
    TabsModule,
    BsDatepickerModule,
    PaginationModule,
    NgxPrintModule,
    ModalModule,
    GoogleChartsModule,
    PagerComponent,
    MapComponent,
    HasRoleDirective,
    ImgInputComponent,
    RatingComponent,
    RolesModalComponent
  ]

})
export class SharedModule { }
