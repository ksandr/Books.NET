<template>
  <nav v-if="pages">
    <ul class="pagination"
        :class="justify">
      <li class="page-item"
          :class="prevDisabled">
        <router-link class="page-link"
                     title="Предыдущая"
                     aria-label="Previous"
                     :to="link(page-1)">
          <span aria-hidden="true">&laquo;</span>
          <span class="sr-only">Предыдущая</span>
        </router-link>
      </li>

      <li v-for="page in pages"
          :key="page"
          class="page-item"
          :class="isActive(page)">
        <router-link class="page-link"
                     :to="link(page)">{{page}}</router-link>
      </li>

      <li class="page-item"
          :class="nextDisabled">
        <router-link class="page-link"
                     title="Следующая"
                     aria-label="Next"
                     :to="link(page+1)">
          <span aria-hidden="true">&raquo;</span>
          <span class="sr-only">Следующая</span>
        </router-link>
      </li>
    </ul>
  </nav>
</template>

<script>
export default {
  name: "app-pagination",
  props: {
    page: {
      type: Number,
      required: true,
    },
    totalPages: {
      type: Number,
      required: true,
    },
    limit: {
      type: Number,
      default: 7,
    },
    link: {
      type: Function,
      required: true,
    },
    justifyContent: {
      type: String,
      default: "start",
    },
  },
  computed: {
    halfLimit() {
      return Math.floor(this.limit / 2);
    },
    minPage() {
      if (this.totalPages <= this.limit) {
        return 1;
      }

      let page = Math.min(this.totalPages - this.limit + 1, Math.max(1, this.page - this.halfLimit));

      return page;
    },
    maxPage() {
      let page = Math.max(this.limit, Math.min(this.page + this.halfLimit, this.totalPages));
      if (page > this.totalPages) {
        page = this.totalPages;
      }

      return page;
    },
    pages() {
      if (this.minPage == 0) {
        return null;
      }

      let result = [];
      for (let i = this.minPage; i <= this.maxPage; i++) {
        result.push(i);
      }

      return result;
    },
    prevDisabled() {
      return this.page <= 1 ? "disabled" : "";
    },

    nextDisabled() {
      return this.page >= this.totalPages ? "disabled" : "";
    },
    justify() {
      if (this.justifyContent == "start") {
        return "";
      }

      return "justify-content-" + this.justifyContent;
    },
  },
  methods: {
    isActive(page) {
      return page == this.page ? "active" : "";
    },
  },
};
</script>
