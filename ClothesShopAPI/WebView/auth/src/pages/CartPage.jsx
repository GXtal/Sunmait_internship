import React, { useEffect, useState, useContext } from 'react';
import api from "../api/axios";
import { AuthContext } from "../contexts/AuthContext";
import CartItem from "../partials/CartItem"

function CartPage() {

  const [cartProducts, setCartProducts] = useState([]);
  const { id } = useContext(AuthContext)

  useEffect(() => {
    api.get(`Products/Cart/${id}`).then(res => {
      setCartProducts(res.data);
    });
  }, [id])

  const handleClick = (e) => {
    e.preventDefault();
    api.post("Orders").then( res => window.location.reload(false)).catch(err => alert(err.detail));
    
  }

  return <div className="CartPage">
    {cartProducts.map((product) =>
      <CartItem key={product.productId} product={product} />
    )}
    <button className="navigate" onClick={handleClick}>Create order</button>
  </div>
}

export default CartPage;