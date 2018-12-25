<template>
  <div v-if="error"
       class="row">
    <div class="col-sm-12">
      <app-alert variant="danger">
        <h4 class="alert-heading">Ошибка</h4>
        <hr />
        <p>{{ error.message }}</p>
      </app-alert>
    </div>
  </div>
</template>

<script>
export default {
  name: "app-error",
  computed: {
    error() {
      return this.$store.state.error;
    },
  },
};
</script>
