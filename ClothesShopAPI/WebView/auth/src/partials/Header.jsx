import React, { useEffect, useState, useContext } from 'react';
import { Link, useNavigate } from "react-router-dom";
import AuthService from "../services/AuthService";
import { AuthContext } from '../contexts/AuthContext';

function Header(props) {

    const [token, setToken] = useState(null);
    const navigate = useNavigate();
    const {id, dispatch} = useContext(AuthContext);

    const handleClick = async (e) => {
        e.preventDefault()
        try {
            dispatch({type: 'LOGOUT'})
            await AuthService.logout()            
        } catch (e) {
            console.log(e)
        } finally {
            navigate("/login")
        }
    }

    useEffect(()=>
    {
        var t = localStorage.getItem("accessToken");
        setToken(t);
    })

    return (
        <div className="navbar">
            <header>
                <div className='header-full'>

                    <Link to="/" className="navigate">Home</Link>

                    {token ?
                        (
                            <div className="nabigate-group">                                
                                <button onClick={handleClick} className="nice-button">Logout</button>
                                <Link to="/cart" className="navigate">Cart</Link>
                            </div>)
                        :
                        (
                            <div className="navigate-group">
                                <Link to="/login" className="navigate">Login</Link>
                            </div>
                        )
                    }

                </div>
            </header>
        </div>
    );
}

export default Header;