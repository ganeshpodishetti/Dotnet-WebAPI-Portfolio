import 'bootstrap/dist/css/bootstrap.min.css'
import 'bootstrap/dist/js/bootstrap.bundle.min.js'
import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App'
import './index.css'; // You might need to create this file if it doesn't exist

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <App />
  </React.StrictMode>,
)
