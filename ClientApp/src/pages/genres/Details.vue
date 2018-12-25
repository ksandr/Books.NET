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
import indexMixin from "../../mixins/index";
import http from "../../utils/http.js";
import URLBuilder from "../../utils/urlBuilder";

export default {
  name: "genres-details-page",
  mixins: [indexMixin],
  props: {
    id: {
      type: [Number, String],
      required: true,
    },
  },
  data() {
    return {
      item: {},
    };
  },
  methods: {
    doLoad() {
      let url = new URLBuilder(`genres(${this.id})`).build();
      return http
        .get(url)
        .then(result => {
          this.item = result.data;

          let url = new URLBuilder(`genres(${this.id})/books.books`)
            .expand("Series,Authors,Genres")
            .withSearch(this.query)
            .orderBy("Search")
            .page(this.pageNumber, this.pageSize)
            .build();
          return http.get(url);
        })
        .then(result => {
          this.items = result.data;
          this.$store.commit(`app/${this.items.value.length == 0 ? "noData" : "loaded"}`);
          return null;
        });
    },
  },
};
</script>
