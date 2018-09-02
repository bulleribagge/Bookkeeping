import Vue from 'vue'
import axios from 'axios'
import router from './router'
import store from './store'
import { sync } from 'vuex-router-sync'
import App from 'components/app-root'

Vue.prototype.$http = axios;

sync(store, router)

Vue.filter('shortDate', function (value) {
    return value.substr(0, 10);
})

const app = new Vue({
    store,
    router,
    ...App
})

export {
    app,
    router,
    store
}
