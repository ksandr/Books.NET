import {mapGetters} from "vuex";

export default {
  data() {
    return {
      loadingDelay: 500,
    };
  },
  computed: {
    ...mapGetters({
      isLoaded: "app/isLoaded",
    }),
  },
  created() {
    return this.load();
  },
  watch: {
    $route() {
      return this.load();
    },
  },
  methods: {
    load() {
      let timeout = setTimeout(() => {
        this.$store.commit("app/loading");
      }, this.loadingDelay);

      return this.doLoad()
        .then(() => {
          clearTimeout(timeout);
        })
        .catch(() => {
          clearTimeout(timeout);
          this.$store.commit("app/error");
        });
    },
  },
};
