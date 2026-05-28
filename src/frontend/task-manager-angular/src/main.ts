import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';

platformBrowserDynamic()
  .bootstrapModule(AppModule)
  .then(() => {

    const loader = document.getElementById('app-loader');

    if (loader) {
      loader.style.display = 'none';
    }
  })
  .catch(err => console.error(err));