import Alert from "./Alert";
import AlertError from "./AlertError";
import AlertLoading from "./AlertLoading";
import AlertNoData from "./AlertNoData";
import AppContent from "./AppContent";
import AppNavigation from "./AppNavigation";
import BackButton from "./BackButton";
import BookList from "./BookList";
import NavBar from "./NavBar";
import Pagination from "./Pagination";
import SearchForm from "./SearchForm";

export default {
  install: function(Vue) {
    Vue.component("app-alert", Alert);
    Vue.component("app-error", AlertError);
    Vue.component("app-loading", AlertLoading);
    Vue.component("app-no-data", AlertNoData);
    Vue.component("app-content", AppContent);
    Vue.component("app-navigation", AppNavigation);
    Vue.component("app-back-button", BackButton);
    Vue.component("app-book-list", BookList);
    Vue.component("app-nav-bar", NavBar);
    Vue.component("app-pagination", Pagination);
    Vue.component("app-search-form", SearchForm);
  },
};
