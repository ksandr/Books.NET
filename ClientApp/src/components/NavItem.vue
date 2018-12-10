<template>
  <li class="nav-item"
      :class="activeClass">
    <router-link class="nav-link"
                 :to="to">
      <slot></slot>
      <span v-if="isActive"
            class="sr-only">(current)</span>
    </router-link>
  </li>
</template>

<script>
export default {
  name: "app-nav-item",
  props: {
    to: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      isActive: false,
    };
  },
  computed: {
    activeClass() {
      return this.isActive ? "active" : "";
    },
  },
  created() {
    this.setIsActive();
  },
  watch: {
    $route() {
      this.setIsActive();
    },
  },
  methods: {
    setIsActive() {
      let currentPath = this.$router.currentRoute.fullPath;
      let toPath = this.$router.resolve(this.to).route.fullPath;

      if (toPath == "/") {
        this.isActive = currentPath == toPath;
        return;
      }

      this.isActive = currentPath.startsWith(toPath);
    },
  },
};
</script>
