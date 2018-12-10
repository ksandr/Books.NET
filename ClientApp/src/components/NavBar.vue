<template>
  <nav class="navbar navbar-expand-lg navbar-light bg-light">
    <div class="container">
      <span class="navbar-brand mb-0 h1 app-navbar-brand">
        Books.NET
      </span>
      <button class="navbar-toggler"
              type="button"
              data-toggle="collapse"
              data-target="#navbar-nav"
              aria-controls="navbarNav"
              aria-expanded="false"
              aria-label="Toggle navigation">
        <span class="navbar-toggler-icon"></span>
      </button>
      <div class="collapse navbar-collapse"
           id="navbar-nav">
        <ul class="navbar-nav">
          <nav-item :to="{ name: 'books' }">Все книги</nav-item>
          <nav-item :to="{ name: 'books-recent' }">Новые книги</nav-item>
          <nav-item :to="{ name: 'authors' }">Авторы</nav-item>
          <nav-item :to="{ name: 'series' }">Серии</nav-item>
          <nav-item :to="{ name: 'genres' }">Жанры</nav-item>
        </ul>
      </div>
    </div>
  </nav>
</template>

<script>
import "bootstrap/js/src/collapse";

import NavItem from "./NavItem";

export default {
  name: "app-nav-bar",
  components: {
    NavItem,
  },
};
</script>
