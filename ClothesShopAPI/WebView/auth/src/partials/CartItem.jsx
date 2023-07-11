import React, { useState, useEffect } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import api from '../api/axios';

function CartItem(props) {

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

          connection.send('JoinProductGroup', parseInt(props.product.productId));
        })
        .catch(e => console.log('Connection failed: ', e));
    }
  }, [connection, props.product.productId]);

  useEffect(() => {

  }, [allQuantity])

  const removeFromCartHandle = () => {
    api.delete(`Cart/${props.product.productId}`);
    window.location.reload(false);
    
  }

  return <div className="CartItem">
    <div className='column-text'>
      <span className='normal-text'>{props.product.productName}</span>
      <span className='normal-text'>in cart: {props.product.count}</span>
      <button onClick={removeFromCartHandle}>Remove</button>
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

export default CartItem;