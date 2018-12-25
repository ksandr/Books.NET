import Vue from "vue";
import Vuex from "vuex";

import app from "./app";

const debug = process.env.NODE_ENV !== "production";

Vue.use(Vuex);

export default new Vuex.Store({
  modules: {
    app: app,
  },
  state: {
    error: null,
  },
  mutations: {
    clearError(state) {
      state.error = null;
    },
    error(state, err) {
      state.error = err;
    },
  },
  strict: debug,
});
