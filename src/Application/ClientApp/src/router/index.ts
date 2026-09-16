import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import AccountView from '../views/AccountView.vue'
import LocationsView from '../views/LocationsView.vue'
import PlacesView from '../views/PlacesView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  linkActiveClass: 'active',
  routes: [
    { path: '/', name: 'home', component: HomeView },
    // The host serves index.html as a real file too; don't render an empty page for it.
    { path: '/index.html', redirect: '/' },
    { path: '/account', name: 'account', component: AccountView },
    { path: '/locations', name: 'locations', component: LocationsView },
    {
      path: '/places/:locationId/:locationName',
      name: 'places',
      component: PlacesView,
      props: true,
    },
  ],
})

export default router
