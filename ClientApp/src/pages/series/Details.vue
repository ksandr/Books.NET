<template>
  <div class="row">
    <div class="col-sm-12">
      <div class="row">
        <div class="col-sm-12">
          <h1>
            Серии
            <small v-if="isLoaded"
                   class="text-muted">{{ item.Title }}</small>
          </h1>
        </div>
      </div>
      <app-loading></app-loading>
      <app-error></app-error>
      <app-content>
        <app-book-list :items="item.Books"></app-book-list>
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
import URLBuilder from "../../utils/urlBuilder";

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
      item: {},
    };
  },
  computed: {
    ...mapGetters({
      isLoaded: "app/isLoaded",
    }),
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

      let url = new URLBuilder(`series(${this.id})`).build();

      return http
        .get(url)
        .then(result => {
          this.item = result.data;

          let url = new URLBuilder(`series(${this.id})/books.books`)
            .expand("Series,Authors,Genres")
            .orderBy("SeqNumber,Search,UpdateDate")
            .build();
          return http.get(url);
        })
        .then(result => {
          this.item.Books = result.data.value;
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
  },
};
</script>
