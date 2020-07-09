import axios from "axios";
import indicator from "./loading-indicator";
import store from "../store";

//import delay from "./delay";

let http = axios.create({headers: {Pragma: "no-cache"}});

function start() {
  if (store.state.error) {
    store.commit("clearError", null);
  }

  indicator.start();
}

function done(error) {
  if (error) {
    store.commit("error", error);
  }

  indicator.stop();
}

http.interceptors.request.use(
  config => {
    start();
    return config;
  },
  error => {
    done(error);

    return Promise.reject(error);
  }
);

http.interceptors.response.use(
  response => {
    done();
    return response;

    // return delay(() => {
    //   indicator.stop();
    //   return response;
    // }, 2000);
  },
  error => {
    done(error);

    return Promise.reject(error);
  }
);

export default http;
