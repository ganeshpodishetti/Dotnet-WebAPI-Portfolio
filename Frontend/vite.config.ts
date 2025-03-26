import react from '@vitejs/plugin-react'
import { resolve } from 'path'
import { defineConfig } from 'vite'

export default defineConfig({
  plugins: [react()],
  server: {
    port: 3000,
    proxy: {
      '/api': {
        target: 'http://localhost:5067',
        changeOrigin: true,
        secure: false,
        rewrite: (path) => path
      }
    }
  },
  build: {
    sourcemap: true,
    outDir: resolve(__dirname, '../wwwroot'),
    emptyOutDir: true,
    rollupOptions: {
      input: {
        main: resolve(__dirname, 'index.html')  // This is the correct path to your index.html
      }
    }
  },
  optimizeDeps: {
    include: ['react', 'react-dom', 'react-router-dom', 'bootstrap', 'axios']
  },
  resolve: {
    alias: {
      '@': resolve(__dirname, 'src')  // Add alias for easier imports
    }
  }
})
