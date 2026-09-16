<script setup lang="ts">
import { useJson } from '@/composables/useJson'
import type { Location } from '@/types'

const { data: locations, error } = useJson<Location[]>(
  `${import.meta.env.BASE_URL}api/locations/GetAll`,
)
</script>

<template>
  <h1>Top locations to visit in The Netherlands.</h1>

  <p>This component demonstrates usage of the Locations API.</p>

  <p v-if="error" class="text-danger">Couldn't load locations: {{ error }}</p>
  <p v-else-if="!locations"><em>Loading...</em></p>

  <table v-else class="table">
    <thead>
      <tr>
        <th>Name</th>
        <th>Summary</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="location in locations" :key="location.id">
        <td>
          <RouterLink
            :to="{ name: 'places', params: { locationId: location.id, locationName: location.name } }"
          >
            {{ location.name }}
          </RouterLink>
        </td>
        <td>{{ location.summary }}</td>
        <td><img width="100" height="100" :src="location.imageUri ?? undefined" :alt="location.name" /></td>
      </tr>
    </tbody>
  </table>
</template>

<style scoped src="../assets/page.css"></style>
