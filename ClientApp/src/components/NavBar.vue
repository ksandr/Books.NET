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
import NavItem from "./NavItem";

export default {
  name: "app-nav-bar",
  components: {
    NavItem,
  },
  mounted: function() {
    this.$nextTick(function() {
      this.$el.querySelectorAll("[data-toggle='collapse']").forEach(element => {
        let targetSelector = element.getAttribute("data-target");
        let targets = document.querySelectorAll(targetSelector);
        element.addEventListener("click", function() {
          let expanded = this.getAttribute("aria-expanded") == "true";
          this.setAttribute("aria-expanded", !expanded);

          if (expanded) {
            this.classList.add("collapsed");
          } else {
            this.classList.remove("collapsed");
          }

          targets.forEach(target => {
            if (expanded) {
              target.classList.remove("show");
            } else {
              target.classList.add("show");
            }
          });
        });
      });
    });
  },
};
</script>
