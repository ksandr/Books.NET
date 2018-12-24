<template>
  <div class="col-sm-12">
    <div class="row">
      <div class="col-sm-12">
        <h1>Новые книги</h1>
      </div>
    </div>
    <app-error></app-error>
    <app-loading></app-loading>
    <app-no-data></app-no-data>
    <app-content>
      <app-book-list :items="items.value"></app-book-list>
    </app-content>
  </div>
</template>

<script>
import http from "../../utils/http.js";
import URLBuilder from "../../utils/urlBuilder";

export default {
  name: "books-recent-page",
  data() {
    return {
      loadingDelay: 500,
      pageSize: 50,
      items: {},
    };
  },
  created() {
    this.load();
  },
  watch: {
    $route() {
      this.load();
    },
  },
  methods: {
    load() {
      let timeout = setTimeout(() => {
        this.$store.commit("app/loading");
      }, this.loadingDelay);

      let url = new URLBuilder("books")
        .expand("Series,Authors,Genres")
        .orderBy("UpdateDate desc,Search")
        .top(this.pageSize)
        .build();

      return http
        .get(url)
        .then(result => {
          clearTimeout(timeout);
          this.items = result.data;
          this.$store.commit(`app/${this.items.value.length == 0 ? "noData" : "loaded"}`);
        })
        .catch(() => {
          clearTimeout(timeout);
          this.$store.commit("app/error");
        });
    },
  },
};
</script>
