import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import vueDevTools from 'vite-plugin-vue-devtools'

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    vue(),
    vueDevTools(),
  ],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url)),
    },
  },
  build: {
    // The Application host serves the SPA from its wwwroot. emptyOutDir must be explicit
    // because the output directory is outside this project root.
    outDir: '../wwwroot',
    emptyOutDir: true,
  },
  server: {
    // `npm run dev` forwards API calls to the Application host (dotnet run, Development profile).
    proxy: {
      '/api': { target: 'https://localhost:65136', secure: false },
    },
  },
})
