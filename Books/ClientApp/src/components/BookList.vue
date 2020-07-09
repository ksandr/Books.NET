<template>
  <ul class="list">
    <book-list-item v-for="book in items"
                    :key="book.Id"
                    :item="book"
                    class="list__item">
    </book-list-item>
  </ul>
</template>

<script>
import BookListItem from "./BookListItem";

export default {
  name: "app-book-list",
  components: {
    BookListItem,
  },
  props: {
    items: {
      type: Array,
      required: true,
    },
  },
};
</script>
