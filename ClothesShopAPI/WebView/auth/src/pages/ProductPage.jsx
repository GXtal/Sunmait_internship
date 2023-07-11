import React, { useEffect, useState, useContext } from 'react';
import { useParams } from "react-router-dom";
import { AuthContext } from '../contexts/AuthContext'
import api from '../api/axios';
import ProductItem from '../partials/ProductItem'
import ViewersCount from '../partials/ViewersCount';

function ProductPage() {

  const { productId } = useParams()
  const { id } = useContext(AuthContext)

  const [product, setProduct] = useState();
  const [cartCount, setCartCount] = useState(0);

  const handleCartCount = (e) => {
    setCartCount(e.target.value);
  }

  const handleClick = (e) => {
    e.preventDefault();
    api.post("Cart", { productId: productId, count: cartCount });
  }
  useEffect(() => {
    api.get(`Products/${productId}`).then(res => {
      setProduct(res.data);
    })
  }, [productId]);

  return product ? (<div className="ProductPage">
    <ProductItem product={product} />
    <ViewersCount productId={productId} />
    {id &&
      <div><span> Add to cart count</span>
        <input onChange={handleCartCount}></input>
        <button onClick={handleClick}>Add</button>
      </div>}

  </div>) : <></>
}

export default ProductPage;