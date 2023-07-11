import React, { useState, useEffect, useContext } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { AuthContext } from '../contexts/AuthContext';

const ProductCount = (props) => {
    const [productId, setProductId] = useState('');
    const [connection, setConnection] = useState(null);

    const {id} = useContext(AuthContext);

    const onSubmit = async (e) => {
        e.preventDefault();

        console.log(connection._connectionStarted);
        if (connection._connectionStarted) {
            try {
                await connection.send('JoinProductGroup', parseInt(productId));
            }
            catch (e) {
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
        let newConnection;
        if (id)
        {
            const token=localStorage.getItem("accessToken");
            newConnection = new HubConnectionBuilder()
            .withUrl('http://localhost:5233/hubs/ProductViewersCount', { accessTokenFactory: () => token })
            .withAutomaticReconnect()
            .build();
            console.log("with token");
        }
        else
        {
            newConnection = new HubConnectionBuilder()
            .withUrl('http://localhost:5233/hubs/ProductViewersCount')
            .withAutomaticReconnect()
            .build();
        }
        

        setConnection(newConnection);
    }, []);

    useEffect(() => {
        if (connection) {
            connection.start()
                .then(result => {
                    console.log('Connected!');

                    connection.on('GetViewersCount', data => {
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
                value={productId}
                onChange={onProductIdUpdate} />
            <br />
            <button>Submit</button>
        </form>
    )
};

export default ProductCount;