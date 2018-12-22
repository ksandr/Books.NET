<template>
  <div class="col-sm-12">
    <div class="row">
      <div class="col-sm-12">
        <h1>Все книги</h1>
      </div>
    </div>
    <app-navigation>
      <div class="col-md-6">
        <app-search-form :submit="search"
                         :value="query"></app-search-form>
      </div>
      <div class="col-md-6">
        <app-pagination :link="pageLink"
                        :page="pageNumber"
                        :total-pages="totalPages"
                        justifyContent="end"></app-pagination>
      </div>
    </app-navigation>
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

export default {
  name: "books-index-page",
  props: {
    page: {
      type: [Number, String],
      required: false,
      default: "1",
    },
  },
  data() {
    return {
      loadingDelay: 500,
      pageSize: 50,
      items: {},
      query: this.$route.query.q ? decodeURIComponent(this.$route.query.q) : null,
    };
  },
  computed: {
    pageNumber() {
      return parseInt(this.page);
    },
    totalItems: function() {
      return this.items != null ? this.items["@odata.count"] : 0;
    },
    totalPages: function() {
      return Math.ceil(this.totalItems / this.pageSize);
    },
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

      let url = "/odata/books" + (this.query ? `?$filter=contains(Search, '${encodeURIComponent(this.query.toUpperCase())}')&` : "?");

      return http
        .get(
          `${url}$expand=Series,Authors,Genres&$orderby=Search&$skip=${this.pageSize * (this.pageNumber - 1)}&$top=${
            this.pageSize
          }&$count=true`
        )
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
    pageLink(page) {
      return {
        name: "books",
        params: {
          page: page,
        },
        query: this.$route.query.q ? {q: this.$route.query.q} : null,
      };
    },
    search(q) {
      this.query = q;
      this.$router.push({name: "books", params: {page: 1}, query: this.query ? {q: encodeURIComponent(this.query)} : null});
    },
  },
};
</script>
