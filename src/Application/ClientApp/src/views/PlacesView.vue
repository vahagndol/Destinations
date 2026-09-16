<script setup lang="ts">
import { useJson } from '@/composables/useJson'
import type { Place } from '@/types'

const props = defineProps<{
  locationId: string
  locationName: string
}>()

// A getter, so moving from one city's places to another's re-fetches.
const { data: places, error } = useJson<Place[]>(
  () => `${import.meta.env.BASE_URL}api/places/GetAll/${encodeURIComponent(props.locationId)}`,
)
</script>

<template>
  <h1>Top places to see in {{ locationName }}</h1>
  <p>This component demonstrates Must see from the server.</p>

  <p v-if="error" class="text-danger">Couldn't load places: {{ error }}</p>
  <p v-else-if="!places"><em>Loading...</em></p>

  <table v-else class="table">
    <thead>
      <tr>
        <th>Name</th>
        <th>Summary</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="place in places" :key="place.id">
        <td>{{ place.name }}</td>
        <td>{{ place.summary }}</td>
        <td><img width="100" height="100" :src="place.imageUri ?? undefined" :alt="place.name" /></td>
      </tr>
    </tbody>
  </table>
</template>

<style scoped src="../assets/page.css"></style>
