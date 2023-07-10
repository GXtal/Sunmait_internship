import React, { useState, useEffect } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';

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

          connection.send('JoinProductGroup', parseInt(props.product.id));
        })
        .catch(e => console.log('Connection failed: ', e));          
    }
  }, [connection, props.product.id]);

  useEffect(()=>
  {

  },[allQuantity])

  return <div className="CartItem">
    {props.product.productName} <br/>
    In cart: 
    {props.product.count}
    {allQuantity &&
      <div>
        available:{allQuantity.availableQuantity} <br/>
        reserved:{allQuantity.reservedQuantity}
      </div>}
      
  </div>
}

export default CartItem;