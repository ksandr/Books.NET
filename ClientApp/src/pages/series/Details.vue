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
        <app-book-list :items="items"></app-book-list>
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
import detailsMixin from "../../mixins/details";

export default {
  name: "series-details-page",
  mixins: [detailsMixin],
  data() {
    return {
      entity: "series",
    };
  },
};
</script>
