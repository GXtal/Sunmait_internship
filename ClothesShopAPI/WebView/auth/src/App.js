import React, { useState } from 'react';
import axios from 'axios'

function App() {

  const [credentials, setCredentials] = useState({
    email: undefined,
    password: undefined,
    name: undefined,
    surname: undefined
  })

  const handleChange = (e) => {
    setCredentials(prev => ({ ...prev, [e.target.id]: e.target.value }))
  }

  const handleLoginClick = async (e) => {
    e.preventDefault()    
    
    var req =
    {
      Email: credentials.email,
      Password: credentials.password
    }
    console.log(req);
    axios.post("http://localhost:5233/api/Users/login", req).then(r => console.log(r.data));
  }

  const handleSignupClick = async (e) => {
    e.preventDefault()    
    var req =
    {
      Email: credentials.email,
      Password: credentials.password,
      Name: credentials.name,
      Surname: credentials.surname
    }
    axios.post("http://localhost:5233/api/Users/register", req).then(r => console.log(r.data));
  }

  return (
    <div className="App">
      <div>
        <span>Email</span>
        <input type="text" id="email" onChange={handleChange} />
      </div>
      <div >
        <span className="input-group-text">Password</span>
        <input type="password" id="password" onChange={handleChange} />
      </div>
      <div >
        <span className="input-group-text">Name</span>
        <input type="text" id="name" onChange={handleChange} />
      </div>
      <div >
        <span className="input-group-text">Surname</span>
        <input type="text" id="surname" onChange={handleChange} />
      </div>
      <button onClick={handleLoginClick}>Login</button>
      <button onClick={handleSignupClick}>Register</button>
    </div>
  );
}

export default App;
