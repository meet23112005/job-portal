import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.jsx'
import './index.css'
import { Toaster } from './components/ui/sonner.jsx'
import { Provider } from 'react-redux'
import store from './redux/store.js'
import { persistStore } from 'redux-persist'
import { PersistGate } from 'redux-persist/integration/react'

const persistor = persistStore(store);

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <Provider store={store}>
      {/* Delays rendering until the saved data is loaded from LocalStorage */}
      <PersistGate loading={null} persistor={persistor}>
        <App />
        <Toaster duration={1500}/>
      </PersistGate>
    </Provider>
  </React.StrictMode>,
)
