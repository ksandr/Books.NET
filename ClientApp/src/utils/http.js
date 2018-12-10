import axios from "axios";
import indicator from "./loading-indicator";

//import delay from "./delay";

let http = axios.create({headers: {Pragma: "no-cache"}});

http.interceptors.request.use(
  config => {
    indicator.start();

    return config;
  },
  error => {
    indicator.stop();

    return Promise.reject(error);
  }
);

http.interceptors.response.use(
  response => {
    indicator.stop();
    return response;

    // return delay(() => {
    //   indicator.stop();
    //   return response;
    // }, 2000);
  },
  error => {
    indicator.stop();

    return Promise.reject(error);
  }
);

export default http;
