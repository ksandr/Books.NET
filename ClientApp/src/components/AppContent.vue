<template>
  <div v-if="isLoaded"
       class="row">
    <div class="col-sm-12">
      <slot></slot>
    </div>
  </div>
</template>

<script>
import {mapGetters} from "vuex";

export default {
  name: "app-content",
  computed: {
    ...mapGetters({
      isLoaded: "app/isLoaded",
    }),
  },
};
</script>
