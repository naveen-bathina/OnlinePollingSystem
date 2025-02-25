import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import 'bootstrap/dist/css/bootstrap.min.css';
import { PollProvider } from "./contexts/PollingContext";

createRoot(document.getElementById('root')!).render(
  <PollProvider>
    <App />
  </PollProvider>
)
