<template>
  <div class="alert"
       :class="this.class"
       role="alert">
    <slot></slot>
  </div>
</template>

<script>
export default {
  name: "app-alert",
  props: {
    variant: {
      type: String,
      default: "primary",
      validator: function(value) {
        return ["primary", "secondary", "success", "danger", "warning", "info", "light", "dark"].indexOf(value) !== -1;
      },
    },
  },
  computed: {
    class() {
      return `alert-${this.variant}`;
    },
  },
};
</script>
