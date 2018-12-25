import baseMixin from "./base";
import http from "../utils/http.js";
import URLBuilder from "../utils/urlBuilder";

export default {
  mixins: [baseMixin],
  props: {
    id: {
      type: [Number, String],
      required: true,
    },
  },
  data() {
    return {
      item: {},
      items: [],
    };
  },
  methods: {
    doLoad() {
      let url = new URLBuilder(`${this.entity}(${this.id})`).build();
      return http
        .get(url)
        .then(result => {
          this.item = result.data;

          let url = new URLBuilder(`${this.entity}(${this.id})/books.books`)
            .expand("Series,Authors,Genres")
            .orderBy("SeqNumber,Search,UpdateDate")
            .build();

          return http.get(url);
        })
        .then(result => {
          this.items = result.data.value;
          this.$store.commit("app/loaded");
          return null;
        });
    },
  },
};
