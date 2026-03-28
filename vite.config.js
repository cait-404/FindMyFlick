import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import tailwindcss from '@tailwindcss/vite'
// https://vite.dev/config/
export default defineConfig({
  plugins: [ tailwindcss(), react()],
  define: {
    'import.meta.env.VITE_API_URL': JSON.stringify('https://findmyflick-api-ecargjhzf4acffec.canadacentral-01.azurewebsites.net')
  },
build: {
  rollupOptions: {
    output: {
      entryFileNames: 'assets/[name]-[hash]-${Date.now()}.js',
      chunkFileNames: 'assets/[name]-[hash]-${Date.now()}.js',
      assetFileNames: 'assets/[name]-[hash]-${Date.now()}.[ext]'
    }
  }
}
})
