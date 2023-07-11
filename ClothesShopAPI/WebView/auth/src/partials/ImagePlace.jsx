import React, { useState, useEffect } from 'react';
import api from '../api/axios';

function ProductItem(props) {

    const [image, setImage] = useState(null);
    useEffect(() => {
        api.get(`Images/Products/${props.productId}`).then( res =>
            {
                if (res.data[0])
                {
                    setImage(`http://localhost:5233/api/Images/${res.data[0]}`);
                }
            })
        
    }, []);


    return <div className="ImagePlace">
        <img className='kittenImage' src={image}></img>
    </div>
}

export default ProductItem;