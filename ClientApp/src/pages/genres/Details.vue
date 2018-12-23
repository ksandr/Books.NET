<template>
  <div class="row">
    <div class="col-sm-12">
      <div class="row">
        <div class="col-sm-12">
          <h1>
            Жанры
            <small v-if="isLoaded"
                   class="text-muted">{{ item.Name }}</small>
          </h1>
        </div>
      </div>
      <app-loading></app-loading>
      <app-error></app-error>
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
      <app-content>
        <app-book-list :items="books.value"></app-book-list>
      </app-content>
      <app-navigation>
        <div class="col">
          <div class="row">
            <div class="col">
              <hr>
            </div>
          </div>
          <div class="row">
            <div class="col-sm-6 col-md-4 col-lg-3">
              <app-back-button block></app-back-button>
            </div>
          </div>
        </div>
      </app-navigation>
    </div>
  </div>
</template>

<script>
import {mapGetters} from "vuex";
import http from "../../utils/http.js";

export default {
  name: "series-details-page",
  props: {
    id: {
      type: [Number, String],
      required: true,
    },
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
      item: {},
      books: [],
      query: this.$route.query.q ? decodeURIComponent(this.$route.query.q) : null,
    };
  },
  computed: {
    ...mapGetters({
      isLoaded: "app/isLoaded",
    }),
    pageNumber() {
      return parseInt(this.page);
    },
    totalItems: function() {
      return this.books != null ? this.books["@odata.count"] : 0;
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

      let baseURL = `/odata/genres(${this.id})`;

      return http
        .get(baseURL)
        .then(result => {
          this.item = result.data;

          let url =
            `${baseURL}/books.books` + (this.query ? `?$filter=contains(Search, '${encodeURIComponent(this.query.toUpperCase())}')&` : "?");

          return http.get(
            `${url}$expand=Series,Authors,Genres&$orderby=Search&$skip=${this.pageSize * (this.pageNumber - 1)}&$top=${
              this.pageSize
            }&$count=true`
          );
        })
        .then(result => {
          this.books = result.data;
          return null;
        })
        .then(() => {
          clearTimeout(timeout);
          this.$store.commit("app/loaded");
        })
        .catch(() => {
          clearTimeout(timeout);
          this.$store.commit("app/error");
        });
    },
    pageLink(page) {
      return {
        name: "genres-details",
        params: {
          id: this.id,
          page: page,
        },
        query: this.$route.query.q ? {q: this.$route.query.q} : null,
      };
    },
    search(q) {
      this.query = q;
      this.$router.push({
        name: "genres-details",
        params: {id: this.id, page: 1},
        query: this.query ? {q: encodeURIComponent(this.query)} : null,
      });
    },
  },
};
</script>
