import { bootstrapApplication } from '@angular/platform-browser';
import { App } from './app/app';
import { appConfig } from './app/app.config';
import { AllCommunityModule, ModuleRegistry } from 'ag-grid-community';

bootstrapApplication(App, appConfig)
.catch((err: any) => console.error(err));

ModuleRegistry.registerModules([AllCommunityModule]);