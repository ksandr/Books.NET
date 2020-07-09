import Vue from "vue";
import VueRouter from "vue-router";

import store from "./store";

import BooksIndexPage from "./pages/books/Index";
import BooksRecentPage from "./pages/books/Recent";
import BooksDetailsPage from "./pages/books/Details";

import AuthorsIndexPage from "./pages/authors/Index";
import AuthorsDetailsPage from "./pages/authors/Details";

import SeriesIndexPage from "./pages/series/Index";
import SeriesDetailsPage from "./pages/series/Details";

import GenresIndexPage from "./pages/genres/Index";
import GenresDetailsPage from "./pages/genres/Details";

Vue.use(VueRouter);

const router = new VueRouter({
  mode: "history",
  routes: [
    {name: "home", path: "/", redirect: {name: "books"}},

    {name: "books", path: "/books/:page(\\d+)?", component: BooksIndexPage, props: true},
    {name: "books-recent", path: "/recent", component: BooksRecentPage},
    {name: "books-details", path: "/books/details/:id(\\d+)", component: BooksDetailsPage, props: true},

    {name: "authors", path: "/authors/:page(\\d+)?", component: AuthorsIndexPage, props: true},
    {name: "authors-details", path: "/authors/details/:id(\\d+)", component: AuthorsDetailsPage, props: true},

    {name: "series", path: "/series/:page(\\d+)?", component: SeriesIndexPage, props: true},
    {name: "series-details", path: "/series/details/:id(\\d+)", component: SeriesDetailsPage, props: true},

    {name: "genres", path: "/genres/:page(\\d+)?", component: GenresIndexPage, props: true},
    {name: "genres-details", path: "/genres/details/:id/:page(\\d+)?", component: GenresDetailsPage, props: true},

    {path: "*", redirect: "/"},
  ],
});

router.beforeEach((to, from, next) => {
  store.commit("app/navigate");
  next();
});

export default router;
