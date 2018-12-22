<template>
  <div v-if="isNoData"
       class="row">
    <div class="col-sm-12">
      <app-alert variant="warning">
        Нет данных
      </app-alert>
    </div>
  </div>
</template>

<script>
import {mapGetters} from "vuex";

export default {
  name: "app-no-data",
  computed: {
    ...mapGetters({
      isNoData: "app/isNoData",
    }),
  },
};
</script>
