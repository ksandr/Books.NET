<template>
  <div class="row">
    <div class="col-sm-12">
      <div class="row">
        <div class="col-sm-12">
          <h1>
            Авторы
            <small v-if="isLoaded"
                   class="text-muted">{{ item.LastName }} {{ item.FirstName }} {{ item.MiddleName }}</small>
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
  name: "authors-details-page",
  mixins: [detailsMixin],
  data() {
    return {
      entity: "authors",
    };
  },
};
</script>
