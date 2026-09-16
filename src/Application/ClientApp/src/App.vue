<script setup lang="ts">
import NavMenu from './components/NavMenu.vue'
</script>

<template>
  <div class="container-fluid">
    <div class="row">
      <div class="col-md-3">
        <NavMenu />
      </div>
      <div class="col-md-9 body-content">
        <RouterView />
      </div>
    </div>
  </div>
</template>

<style scoped>
/* Bootstrap 5 dropped the top margin Bootstrap 3 gave headings; keep the old spacing. */
.body-content {
  padding-top: 20px;
}

@media (max-width: 767px) {
  /* On small screens, the nav menu spans the full width of the screen (56px tall). Leave a space for it. */
  .body-content {
    padding-top: 76px;
  }
}
</style>
