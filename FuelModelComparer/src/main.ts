import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'

import 'vuetify/styles'; // Import Vuetify styles
import { createVuetify } from 'vuetify';
import * as components from 'vuetify/components';
import * as directives from 'vuetify/directives';

//createApp(App).mount('#app')

const vuetify = createVuetify({
  components,
  directives,
});

createApp(App)
  .use(vuetify)
  .mount('#app');
