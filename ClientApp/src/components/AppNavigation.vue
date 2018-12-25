<template>
  <div v-if="allowNavigation"
       class="row">
    <slot></slot>
  </div>
</template>

<script>
import {mapGetters} from "vuex";

export default {
  name: "app-navigation",
  computed: {
    ...mapGetters({
      allowNavigation: "app/allowNavigation",
    }),
  },
};
</script>
