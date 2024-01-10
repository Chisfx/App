import Vue from 'vue'
import App from './App.vue'
import store from './store'
import vuetify from './plugins/vuetify'
import axios from 'axios'
import VueSimpleAlert from 'vue-simple-alert';

Vue.config.productionTip = false
axios.defaults.baseURL = 'http://apifx.somee.com/';
Vue.use(VueSimpleAlert);

new Vue({
  store,
  vuetify,
  render: h => h(App)
}).$mount('#app')
