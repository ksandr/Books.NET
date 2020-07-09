import baseMixin from "./base";
import http from "../utils/http.js";
import URLBuilder from "../utils/urlBuilder";

export default {
  mixins: [baseMixin],
  props: {
    page: {
      type: [Number, String],
      required: false,
      default: "1",
    },
  },
  data() {
    return {
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
  methods: {
    getURL() {
      return new URLBuilder(this.entity)
        .withSearch(this.query)
        .orderBy("Search")
        .page(this.pageNumber, this.pageSize)
        .build();
    },
    doLoad() {
      let url = this.getURL();
      return http.get(url).then(result => {
        this.items = result.data;
        this.$store.commit(`app/${this.items.value.length == 0 ? "noData" : "loaded"}`);
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
