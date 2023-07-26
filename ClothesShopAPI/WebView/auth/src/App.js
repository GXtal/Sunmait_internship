import React from 'react';
import { BrowserRouter, Route, Routes } from "react-router-dom";
import LoginPage from './pages/LoginPage';
import ProductCount from './pages/ProductCount';
import ProductList from './pages/ProductList';
import ProductPage from './pages/ProductPage';
import CartPage from './pages/CartPage';
import Header from './partials/Header';
import './styles.css'

function App() {

  return <div className="App">
    <BrowserRouter>
      <Header />
      <Routes>
        <Route path="/" element={<ProductList />} />
        <Route path="/products/:productId" element={<ProductPage />} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/cart" element={<CartPage />} />
        <Route path="/test" element={<ProductCount />} />
      </Routes>
    </BrowserRouter>
  </div>
}

export default App;
