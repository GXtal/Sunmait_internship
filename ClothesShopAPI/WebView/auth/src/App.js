import React from 'react';
import {BrowserRouter, Route, Routes} from "react-router-dom";
import LoginPage from './pages/LoginPage';
import ProductCount from './pages/ProductCount';

function App() {

  return <div className="App">
      <BrowserRouter>
          <Routes>
            <Route path="/" element={<LoginPage/>}/>
            <Route path="/test" element={<ProductCount/>}/>
          </Routes>
      </BrowserRouter>
    </div>
}

export default App;
