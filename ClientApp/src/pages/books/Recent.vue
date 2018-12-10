<template>
  <div class="col-sm-12">
    <div class="row">
      <div class="col-sm-12">
        <h1>Новые книги</h1>
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
        <app-book-list :items="items.value"></app-book-list>
      </div>
    </div>
    <div v-if="state == 'no-data'"
         class="row">
      <div class="col-sm-12">
        <app-alert variant="warning">
          Нет даных
        </app-alert>
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
  </div>
</template>

<script>
import http from "../../utils/http.js";

export default {
  name: "books-recent-page",
  data() {
    return {
      loadingDelay: 500,
      pageSize: 50,
      state: "none",
      items: null,
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
        .get(`/odata/books?$expand=Series,Authors,Genres&$orderby=UpdateDate desc,SearchTitle&&$top=${this.pageSize}`)
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
  },
};
</script>
