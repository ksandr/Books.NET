import "./scss/site.scss";

import "es6-shim";

import Vue from "vue";

import App from "./App.vue";
import router from "./router";
import components from "./components";
import store from "./store"

if (process.env.NODE_ENV !== "production") {
  console.warn("App is running in development mode!"); /* eslint no-console: "off" */
}

Vue.config.productionTip = false;

Vue.use(components);

new Vue({
  el: "#app",
  router,
  store,
  render: h => h(App),
});