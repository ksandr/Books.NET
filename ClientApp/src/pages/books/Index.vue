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
import indexMixin from "../../mixins/index";
import URLBuilder from "../../utils/urlBuilder";

export default {
  name: "books-index-page",
  mixins: [indexMixin],
  data() {
    return {
      entity: "books",
    };
  },
  methods: {
    getURL() {
      return new URLBuilder("books")
        .expand("Series,Authors,Genres")
        .withSearch(this.query)
        .orderBy("Search")
        .page(this.pageNumber, this.pageSize)
        .build();
    },
  },
};
</script>
