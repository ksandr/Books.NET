import http from "../utils/http.js";
import URLBuilder from "../utils/urlBuilder";

export default {
  props: {
    page: {
      type: [Number, String],
      required: false,
      default: "1",
    },
  },
  data() {
    return {
      loadingDelay: 500,
      pageSize: 50,
      items: {},
      query: this.$route.query.q ? decodeURIComponent(this.$route.query.q) : null,
    };
  },
  computed: {
    pageNumber() {
      return parseInt(this.page);
    },
    totalItems: function() {
      return this.items != null ? this.items["@odata.count"] : 0;
    },
    totalPages: function() {
      return Math.ceil(this.totalItems / this.pageSize);
    },
  },
  created() {
    this.load();
  },
  watch: {
    $route() {
      this.load();
    },
  },
  methods: {
    getURL() {
      return new URLBuilder(this.entity)
        .withSearch(this.query)
        .orderBy("Search")
        .page(this.pageNumber, this.pageSize)
        .build();
    },
    load() {
      let timeout = setTimeout(() => {
        this.$store.commit("app/loading");
      }, this.loadingDelay);

      let url = this.getURL();
      return http
        .get(url)
        .then(result => {
          clearTimeout(timeout);
          this.items = result.data;
          this.$store.commit(`app/${this.items.value.length == 0 ? "noData" : "loaded"}`);
        })
        .catch(() => {
          clearTimeout(timeout);
          this.$store.commit("app/error");
        });
    },
    pageLink(page) {
      return {
        name: this.entity,
        params: {
          page: page,
        },
        query: this.$route.query.q ? {q: this.$route.query.q} : null,
      };
    },
    search(q) {
      this.query = q;
      this.$router.push({name: this.entity, params: {page: 1}, query: this.query ? {q: encodeURIComponent(this.query)} : null});
    },
    details(id) {
      this.$router.push({name: `${this.entity}-details`, params: {id: id}});
    },
  },
};
