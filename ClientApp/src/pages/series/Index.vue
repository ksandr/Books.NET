<template>
  <div class="row">
    <div class="col-sm-12">
      <div class="row">
        <div class="col-sm-12">
          <h1>Серии</h1>
        </div>
      </div>
      <app-error></app-error>
      <div v-if="state != 'none' && state != 'loading'"
           class="row">
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
      </div>
      <div v-if="state == 'loading'"
           class="row">
        <div class="col-sm-12">
          <app-alert variant="info">
            Загрузка...
          </app-alert>
        </div>
      </div>
      <div v-if="state == 'loaded'"
           class="row">
        <div class="col-sm-12">
          <ul class="list">
            <li v-for="item in items.value"
                :key="item.Id"
                class="list__item list__item--pointer"
                v-on:click.prevent="details(item.Id)">
              <p>
                {{ item.Title }}
              </p>
            </li>
          </ul>
        </div>
      </div>
      <div v-if="state == 'no-data'"
           class="row">
        <div class="col-sm-12">
          <app-alert variant="warning">
            Нет данных
          </app-alert>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import http from "../../utils/http.js";

export default {
  name: "series-index-page",
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
      state: "none",
      items: null,
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
        this.state = "loading";
      }, this.loadingDelay);

      let url = "/odata/series" + (this.query ? `?$filter=contains(Search, '${encodeURIComponent(this.query.toUpperCase())}')&` : "?");

      return http
        .get(`${url}$orderby=Search&$skip=${this.pageSize * (this.pageNumber - 1)}&$top=${this.pageSize}&$count=true`)
        .then(result => {
          clearTimeout(timeout);
          this.items = result.data;
          this.state = this.items.value.length == 0 ? "no-data" : "loaded";
        })
        .catch(e => {
          clearTimeout(timeout);
          this.error = e;
          this.state = "error";
        });
    },
    pageLink(page) {
      return {
        name: "series",
        params: {
          page: page,
        },
        query: this.$route.query.q ? {q: this.$route.query.q} : null,
      };
    },
    search(q) {
      this.query = q;
      this.$router.push({name: "series", params: {page: 1}, query: this.query ? {q: encodeURIComponent(this.query)} : null});
    },
    details(id) {
      this.$router.push({name: "series-details", params: {id: id}});
    },
  },
};
</script>
