<template>
  <form v-on:submit.prevent="search">
    <div class="input-group">
      <input type="search"
             class="form-control"
             placeholder="Поиск"
             aria-label="Search"
             v-model="query">
      <div class="input-group-append">
        <button class="btn btn-outline-success"
                title="Искать"
                type="submit">
          <i class="fas fa-search"
             aria-hidden="true"></i>
        </button>
      </div>
    </div>
  </form>
</template>

<script>
export default {
  name: "app-search-form",
  props: {
    submit: {
      type: Function,
      required: true,
    },
    value: {
      type: String,
      default: "",
    },
  },
  data() {
    return {
      query: null,
    };
  },
  created() {
    this.query = this.value;
  },
  methods: {
    search() {
      this.submit(this.query);
    },
  },
};
</script>
