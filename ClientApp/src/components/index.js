import Alert from "./Alert";
import BackButton from "./BackButton";
import BookList from "./BookList";
import AppError from "./Error";
import NavBar from "./NavBar";
import Pagination from "./Pagination";
import SearchForm from "./SearchForm";

export default {
  install: function(Vue) {
    Vue.component("app-alert", Alert);
    Vue.component("app-back-button", BackButton);
    Vue.component("app-book-list", BookList);
    Vue.component("app-error", AppError);
    Vue.component("app-nav-bar", NavBar);
    Vue.component("app-pagination", Pagination);
    Vue.component("app-search-form", SearchForm);
  },
};
