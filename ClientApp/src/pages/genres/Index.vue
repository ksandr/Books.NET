<template>
  <div class="row">
    <div class="col-sm-12">
      <div class="row">
        <div class="col-sm-12">
          <h1>Жанры</h1>
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
        <ul class="list">
          <li v-for="item in items.value"
              :key="item.Id"
              class="list__item list__item--pointer"
              v-on:click.prevent="details(item.Id)">
            <p>
              {{ item.Name }}
            </p>
          </li>
        </ul>
      </app-content>
    </div>
  </div>
</template>

<script>
import http from "../../utils/http.js";

export default {
  name: "genres-index-page",
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

      let url = "/odata/genres" + (this.query ? `?$filter=contains(Search, '${encodeURIComponent(this.query.toUpperCase())}')&` : "?");

      return http
        .get(`${url}$orderby=Search,Name&$skip=${this.pageSize * (this.pageNumber - 1)}&$top=${this.pageSize}&$count=true`)
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
        name: "genres",
        params: {
          page: page,
        },
        query: this.$route.query.q ? {q: this.$route.query.q} : null,
      };
    },
    search(q) {
      this.query = q;
      this.$router.push({name: "genres", params: {page: 1}, query: this.query ? {q: encodeURIComponent(this.query)} : null});
    },
    details(id) {
      this.$router.push({name: "genres-details", params: {id: id}});
    },
  },
};
</script>
