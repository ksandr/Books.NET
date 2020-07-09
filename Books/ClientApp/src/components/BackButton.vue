<template>
  <button class="btn btn-outline-primary"
          type="button"
          :class="btnClass"
          v-on:click.prevent="click">
    <i class="fas fa-arrow-left"
       aria-hidden="true"></i>
    Назад
  </button>
</template>

<script>
export default {
  name: "app-back-button",
  props: {
    block: {
      type: Boolean,
      default: false,
    },
  },
  computed: {
    btnClass() {
      return this.block ? " btn-block" : "";
    },
  },
  methods: {
    click() {
      this.$router.back();
    },
  },
};
</script>
