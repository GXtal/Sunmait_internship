import React, { useState, useEffect } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';

const ProductCount = (props) => {
    const [ProductId, setProductId] = useState('');
    const [ connection, setConnection ] = useState(null);

    const onSubmit = async (e) => {
        e.preventDefault();

        console.log(connection._connectionStarted);
        if (connection._connectionStarted) {
            try {
                await connection.send('JoinProductGroup', parseInt(ProductId));
            }
            catch(e) {
                console.log(e);
            }
        }
        else {
            alert('No connection to server yet.');
        }
    }

    const onProductIdUpdate = (e) => {
        setProductId(e.target.value);
    }

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
                        console.log(data);
                    });
                })
                .catch(e => console.log('Connection failed: ', e));
        }
    }, [connection]);

    return (
        <form 
            onSubmit={onSubmit}>
            <label htmlFor="ProductId">ProductId:</label>
            <br />
            <input 
                id="ProductId" 
                name="ProductId" 
                value={ProductId}
                onChange={onProductIdUpdate} />
            <br/>            
            <button>Submit</button>
        </form>
    )
};

export default ProductCount;