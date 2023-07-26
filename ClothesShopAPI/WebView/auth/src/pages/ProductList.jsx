import React, { useEffect, useState } from 'react';
import { Link } from "react-router-dom";
import api from "../api/axios";
import ProductItem from '../partials/ProductItem';

function ProductList() {

    const [products, setProducts] = useState([]);

    useEffect(() => {
        api.get("Products").then(res => {
            setProducts(res.data);
        });
    },[])

    return (<div className="ProductList">

        {products.map((product) =>
            <a className='nice-link' href={`/products/${product.id}`} key={product.id}>
                <ProductItem  product={product}/>
            </a>           
        )}

    </div>);
}

export default ProductList;