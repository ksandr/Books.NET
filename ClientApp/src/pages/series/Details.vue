<template>
  <div class="row">
    <div class="col-sm-12">
      <div class="row">
        <div class="col-sm-12">
          <h1>
            Серии
            <small v-if="state == 'loaded'"
                   class="text-muted">{{ item.Title }}</small>
          </h1>
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
          <app-book-list :items="item.Books"></app-book-list>
        </div>
      </div>
      <div v-if="state == 'error'"
           class="row">
        <div class="col-sm-12">
          <app-alert variant="danger">
            Ошибка
          </app-alert>
        </div>
      </div>
      <template v-if="state != 'none' && state != 'loading'">
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
      </template>
    </div>
  </div>
</template>

<script>
import http from "../../utils/http.js";

export default {
  name: "series-details-page",
  props: {
    id: {
      type: [Number, String],
      required: true,
    },
  },
  data() {
    return {
      loadingDelay: 500,
      state: "none",
      item: null,
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
        this.state = "loading";
      }, this.loadingDelay);

      return http
        .get(`/odata/series(${this.id})`)
        .then(result => {
          this.item = result.data;

          return http.get(`/odata/series(${this.id})/books.books?$expand=Series,Authors,Genres&$orderby=SeqNumber,SearchTitle,UpdateDate`);
        })
        .then(result => {
          this.item.Books = result.data.value;
          return null;
        })
        .then(() => {
          clearTimeout(timeout);
          this.state = "loaded";
        })
        .catch(e => {
          clearTimeout(timeout);
          this.error = e;
          this.state = "error";
        });
    },
  },
};
</script>
