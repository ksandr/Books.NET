<template>
  <div class="row">
    <div class="col-sm-12">
      <div class="row">
        <div class="col-sm-12">
          <h1>Серии</h1>
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
      </app-content>
    </div>
  </div>
</template>

<script>
import indexMixin from "../../mixins/index";

export default {
  name: "series-index-page",
  mixins: [indexMixin],
  data() {
    return {
      entity: "series",
    };
  },
};
</script>
