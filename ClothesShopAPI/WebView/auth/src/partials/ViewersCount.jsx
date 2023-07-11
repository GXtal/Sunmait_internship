import React, { useState, useEffect, useContext } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { AuthContext } from '../contexts/AuthContext';

function ViewersCount(props) {

    const productId = props.productId;
    const [connection, setConnection] = useState(null);
    const [productViewersCount, setProductViewersCount] = useState(null);

    const { id } = useContext(AuthContext);

    useEffect(() => {
        let newConnection;
        if (id) {
            const token = localStorage.getItem("accessToken");
            newConnection = new HubConnectionBuilder()
                .withUrl('http://localhost:5233/hubs/ProductViewersCount', { accessTokenFactory: () => token })
                .withAutomaticReconnect()
                .build();
            console.log("with token");
        }
        else {
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
                        setProductViewersCount(data);
                    });

                    window.addEventListener('beforeunload', () => {
                        connection.send('LeaveProductGroup', parseInt(productId));
                    });

                    connection.send('JoinProductGroup', parseInt(productId));
                })
                .catch(e => console.log('Connection failed: ', e));

            return () => {
                console.log(connection);
                connection.send('LeaveProductGroup', parseInt(productId));
            };
        }

    }, [connection, productId]);

    return <div className="ViewersCount">
        {productViewersCount &&
            <div>
                current viewers: {productViewersCount.viewersCount}
            </div>}
    </div>
}

export default ViewersCount;