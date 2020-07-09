<template>
  <div>
    <div class="row">
      <div v-if="details.Cover.Data"
           class="col-md-6 d-none d-md-block">
        <img :src="`data:${details.Cover.ContentType};base64,${details.Cover.Data}`"
             :alt="item.Title"
             class="cover">
      </div>
      <div v-else>
        <div class="no-cover">
        </div>
      </div>

      <div class="col">
        <h1>{{ item.Title }}</h1>

        <template v-if="item.Series">
          <p>
            <b>Серия:</b>
            <i>
              <router-link :to="{ name: 'series-details', params: { id: item.Series.Id.toString() } }">{{ item.Series.Title }}</router-link>

              <span v-if="item.SeqNumber">#{{ item.SeqNumber }}</span>
            </i>
          </p>
        </template>

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
  </div>
</template>

<script>
export default {
  name: "app-book-details",
  props: {
    item: {
      required: true,
      type: Object,
    },
    details: {
      required: true,
      type: Object,
    },
  },
};
</script>

