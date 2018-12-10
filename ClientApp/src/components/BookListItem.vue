<template>
  <li class="list__item">
    <h2>
      <router-link :to="{ name: 'books-details', params: { id: item.Id } }">{{ item.Title }}</router-link>
    </h2>
    <p v-if="item.Series">
      <b>Серия:</b>
      <i>
        <router-link :to="{ name: 'series-details', params: { id: item.Series.Id } }">{{ item.Series.Title }}</router-link>

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
          <router-link :to="{name: 'authors-details', params: { id: author.Id } }">
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
          <router-link :to="{name: 'genres-details', params: { id: (genre.Id.replace(/\./g, '_')) } }">
            {{ genre.Name }}
          </router-link>
        </li>
      </ul>
    </template>
    <p class="text-right">
      <small class="text-muted">
        {{ new Date(item.UpdateDate).toLocaleDateString() }},
        {{ item.Lang.toUpperCase() }}
      </small>
    </p>
  </li>
</template>

<script>
export default {
  name: "app-book-list-item",
  props: {
    item: {
      type: Object,
      required: true,
    },
  },
};
</script>
