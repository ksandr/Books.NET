<style lang="scss">
.no-cover {
  width: 300px;
  height: 450px;
  background-color: gray;
}
</style>

<template>
  <div class="row">
    <div class="col-sm-12">
      <div v-if="state == 'loading'"
           class="row">
        <div class="col-sm-12">
          <app-alert variant="info">
            Загрузка...
          </app-alert>
        </div>
      </div>

      <app-error></app-error>

      <template v-if="state == 'loaded'">
        <div class="row">
          <div v-if="details.Cover.Data"
               class="col d-none d-md-block">
            <img :src="`data:${details.Cover.ContentType};base64,${details.Cover.Data}`"
                 :alt="item.Title">
          </div>
          <div v-else>
            <div class="no-cover">
            </div>
          </div>
          <div class="col">
            <h1>{{ item.Title }}</h1>
            <p v-if="item.Series">
              <b>Серия:</b>
              <i>
                <router-link :to="{ name: 'series-details', params: { id: item.Series.Id.toString() } }">{{ item.Series.Title }}</router-link>

                <span v-if="item.SeqNumber">#{{ item.SeqNumber }}</span>
              </i>
            </p>
            <template v-if="item.Authors && item.Authors.length">
              <p>
                <b>Авторы:</b>
              </p>
              <ul>
                <li v-for="author in item.Authors"
                    :key="author.Id">
                  <router-link :to="{name: 'authors-details', params: { id: author.Id.toString() } }">
                    {{ author.LastName || '' }} {{ author.FirstName || ''}} {{ author.MiddleName || ''}}
                  </router-link>
                </li>
              </ul>
            </template>
            <template v-if="item.Genres && item.Genres.length">
              <p>
                <b>Жанры:</b>
              </p>
              <ul>
                <li v-for="genre in item.Genres"
                    :key="genre.Id">
                  <router-link :to="{name: 'genres-details', params: { id: (genre.Id.toString().replace(/\./g, '_')) } }">
                    {{ genre.Name }}
                  </router-link>
                </li>
              </ul>
            </template>
            <template v-if="item.KeyWords">
              <p>
                <b>Ключевые слова:</b>
              </p>
              <ul>
                <li>
                  {{ item.KeyWords }}
                </li>
              </ul>
            </template>
            <hr>
            <p>
              <b>Скачать: </b>
            </p>
            <ul>
              <li>
                <a :href="`/odata/books(${item.Id})/books.download`">
                  <i class="fas fa-book"
                     aria-hidden="true"></i>
                  Скачать книгу
                </a>
              </li>
              <li>
                <a :href="`/odata/books(${item.Id})/books.download?zip`">
                  <i class="fas fa-file-archive"
                     aria-hidden="true"></i>
                  Скачать ZIP
                </a>
              </li>
            </ul>
            <p class="text-right">
              <small class="text-muted">
                {{ new Date(item.UpdateDate).toLocaleDateString() }},
                {{ item.Lang.toUpperCase() }}
              </small>
            </p>
          </div>
        </div>
        <template v-if="details.Annotation">
          <hr>
          <div class="row">
            <div class="col"
                 v-html="details.Annotation">
            </div>
          </div>
        </template>
      </template>

      <template v-if="state != 'none' && state != 'loading'">
        <div class="row">
          <div class="col">
            <hr>
          </div>
        </div>
        <div class="row">
          <div class="col-sm-6 col-md-4 col-lg-3">
            <app-back-button block></app-back-button>
          </div>
        </div>
      </template>
    </div>
  </div>
</template>

<script>
import http from "../../utils/http.js";

export default {
  name: "books-details-page",
  props: {
    id: {
      type: [Number, String],
      required: true,
    },
  },
  data() {
    return {
      loadingDelay: 500,
      state: "none",
      item: null,
      details: {},
    };
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
    load() {
      let timeout = setTimeout(() => {
        this.state = "loading";
      }, this.loadingDelay);

      return http
        .get(`/odata/books(${this.id})?$expand=Series,Authors,Genres`)
        .then(result => {
          this.item = result.data;

          return http.get(`/odata/books(${this.id})/books.details`);
        })
        .then(result => {
          this.details = result.data;
          return null;
        })
        .then(() => {
          clearTimeout(timeout);
          this.state = "loaded";
        })
        .catch(e => {
          clearTimeout(timeout);
          this.error = e;
          this.state = "error";
        });
    },
  },
};
</script>

