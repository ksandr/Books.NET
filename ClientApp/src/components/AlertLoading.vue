<template>
  <div v-if="isLoading"
       class="row">
    <div class="col-sm-12">
      <app-alert variant="info">
        Загрузка...
      </app-alert>
    </div>
  </div>
</template>

<script>
import {mapGetters} from "vuex";

export default {
  name: "app-loading",
  computed: {
    ...mapGetters({
      isLoading: "app/isLoading",
    }),
  },
};
</script>
