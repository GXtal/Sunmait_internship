import React, { useEffect, useState, useContext } from 'react';
import { Link } from 'react-router-dom';
import api from "../api/axios";
import { AuthContext } from "../contexts/AuthContext";
import CartItem from "../partials/CartItem"

function CartPage() {

  const [cartProducts, setCartProducts] = useState([]);
  const { id } = useContext(AuthContext)

  useEffect(() => {
    api.get(`Products/Cart/${id}`).then(res => {
      setCartProducts(res.data);
      console.log(res);
    });
  }, [id])

  const handleClick = (e) => {
    e.preventDefault();
    api.post("Orders");
    window.location.reload(false);
  }
  return <div className="CartPage">
    {cartProducts.map((product) =>
      <CartItem key={product.productId} product={product} />
    )}
    <button onClick={handleClick}>Create order</button>
  </div>
}

export default CartPage;