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
            <Link to={`/products/${product.id}`} key={product.id}>
                <ProductItem  product={product}/>
            </Link>           
        )}

    </div>);
}

export default ProductList;