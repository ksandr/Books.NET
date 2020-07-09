<template>
  <div>
    <app-nav-bar></app-nav-bar>
    <div class="container">
      <transition name="fade"
                  mode="out-in">
        <router-view></router-view>
      </transition>
    </div>
  </div>
</template>

<script>
export default {
  name: "books-app",
};
</script>
