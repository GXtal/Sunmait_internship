import React, { useState, useEffect } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import ImagePlace from "./ImagePlace";

function ProductItem(props) {

  const [connection, setConnection] = useState(null);
  const [allQuantity, setAllQuantity] = useState(null);

  useEffect(() => {
    const newConnection = new HubConnectionBuilder()
      .withUrl('http://localhost:5233/hubs/ProductCount')
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);
  }, []);

  useEffect(() => {
    if (connection) {
      connection.start()
        .then(result => {
          console.log('Connected!');

          connection.on('GetProductCount', data => {
            setAllQuantity(data);
            console.log(data);
          });

          connection.send('JoinProductGroup', parseInt(props.product.id));
        })
        .catch(e => console.log('Connection failed: ', e));
    }
  }, [connection, props.product.id]);

  useEffect(() => {

  }, [allQuantity])

  return <div className="ProductItem">
  <div className='column-text'>
    <ImagePlace productId={props.product.id}/>
    <span className='normal-text'>{props.product.name}</span>
    <span className='normal-text'>Price : ${props.product.price}</span>
    <span className='small-text'>{props.product.categoryName}</span>
    <span className='small-text'>{props.product.brandName}</span>
    {allQuantity && (
      <span className='normal-text'>
        <span className='available-text'>available: {allQuantity.availableQuantity}</span>
        <br />
        <span className='reserved-text'>reserved: {allQuantity.reservedQuantity}</span>
      </span>
    )}
  </div>    
</div>
}

export default ProductItem;