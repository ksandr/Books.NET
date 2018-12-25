<template>
  <div class="col-sm-12">
    <div class="row">
      <div class="col-sm-12">
        <h1>Новые книги</h1>
      </div>
    </div>
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
  name: "books-recent-page",
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
        .orderBy("UpdateDate desc,Search")
        .top(this.pageSize)
        .build();
    },
  },
};
</script>
