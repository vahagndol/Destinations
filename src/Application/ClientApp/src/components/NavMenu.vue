<script setup lang="ts">
import { ref } from 'vue'

const isExpanded = ref(false)

function collapse() {
  isExpanded.value = false
}

function toggle() {
  isExpanded.value = !isExpanded.value
}
</script>

<template>
  <div class="main-nav">
    <nav class="navbar navbar-expand-md navbar-dark bg-dark px-3">
      <RouterLink class="navbar-brand" to="/">Destinations</RouterLink>
      <button type="button" class="navbar-toggler" :aria-expanded="isExpanded" @click="toggle">
        <span class="visually-hidden">Toggle navigation</span>
        <span class="navbar-toggler-icon"></span>
      </button>
      <!-- Collapse is driven by isExpanded; Bootstrap's JavaScript is intentionally not loaded. -->
      <div class="navbar-collapse collapse" :class="{ show: isExpanded }">
        <ul class="navbar-nav">
          <li class="nav-item">
            <RouterLink class="nav-link" to="/" @click="collapse">
              <span class="bi bi-house"></span> Home
            </RouterLink>
          </li>
          <li class="nav-item">
            <RouterLink class="nav-link" to="/account" @click="collapse">
              <span class="bi bi-person"></span> Account
            </RouterLink>
          </li>
          <li class="nav-item">
            <RouterLink class="nav-link" to="/locations" @click="collapse">
              <span class="bi bi-geo-alt"></span> Locations
            </RouterLink>
          </li>
        </ul>
      </div>
    </nav>
  </div>
</template>

<style scoped>
.main-nav .bi {
  margin-right: 10px;
}

/* Highlighting rules for nav menu items */
.main-nav .nav-link.active,
.main-nav .nav-link.active:hover,
.main-nav .nav-link.active:focus {
  background-color: #4189c7;
  color: white;
}

/* Keep the nav menu independent of scrolling and on top of other items */
.main-nav {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  z-index: 1;
}

@media (min-width: 768px) {
  /* On larger screens, convert the nav menu to a vertical sidebar */
  .main-nav {
    height: 100%;
    width: calc(25% - 20px);
  }

  /* Bootstrap 5 lays an expanded navbar out as a row; stack it instead */
  .main-nav .navbar {
    height: 100%;
    flex-direction: column;
    align-items: stretch;
    justify-content: flex-start;
  }

  .main-nav .navbar-collapse {
    flex-direction: column;
    align-items: stretch;
    flex-grow: 0;
    border-top: 1px solid #444;
  }

  .main-nav .navbar-nav {
    flex-direction: column;
  }

  .main-nav .nav-item {
    font-size: 15px;
    margin: 6px;
  }

  .main-nav .navbar-nav .nav-link {
    padding: 10px 16px;
    border-radius: 4px;
  }

  .main-nav a {
    /* If a menu item's text is too long, truncate it */
    width: 100%;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
  }
}
</style>
