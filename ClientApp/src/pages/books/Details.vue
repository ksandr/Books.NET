<template>
  <div class="row">
    <div class="col-sm-12">
      <app-loading></app-loading>
      <app-error></app-error>
      <app-content>
        <app-book-details :item="item"
                          :details="details"></app-book-details>
      </app-content>
      <app-navigation>
        <div class="col-sm-12">
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
import detailsMixin from "../../mixins/details";
import http from "../../utils/http";
import URLBuilder from "../../utils/urlBuilder";

export default {
  name: "books-details-page",
  mixins: [detailsMixin],
  data() {
    return {
      details: {
        Cover: {},
      },
    };
  },
  methods: {
    doLoad() {
      let url = new URLBuilder(`books(${this.id})`).expand("Series,Authors,Genres").build();
      return http
        .get(url)
        .then(result => {
          this.item = result.data;

          return http.get(`/odata/books(${this.id})/books.details`);
        })
        .then(result => {
          this.details = result.data;
          this.$store.commit("app/loaded");
          return null;
        });
    },
  },
};
</script>
