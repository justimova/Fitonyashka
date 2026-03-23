import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthLayoutComponent } from './layouts/auth-layout/components/auth-layout.component';
import { UserLayoutComponent } from './layouts/user-layout/components/user-layout.component';
import { HeaderComponent } from './layouts/user-layout/components/header/header.component';
import { ErrorHandlingInterceptor } from './core/interceptors/error-handling.interceptor';
import { AuthInterceptor } from './core/interceptors/auth-interceptor';
import { AvatarUploadComponent } from './components/shared/avatar-upload/avatar-upload.component';
import { NotificationService } from './core/services/notification.service';

@NgModule({
  declarations: [
    AppComponent,
    AuthLayoutComponent,
    UserLayoutComponent,
    HeaderComponent,
    AvatarUploadComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    BrowserAnimationsModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorHandlingInterceptor,
      multi: true
    },
    NotificationService,
  ],
  bootstrap: [ AppComponent ]
})
export class AppModule {}
