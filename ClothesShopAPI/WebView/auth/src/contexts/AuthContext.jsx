import {createContext, useEffect, useReducer} from "react"
import {AuthReducer, INITIAL_STATE} from "../redux/AuthReducer";

export const AuthContext = createContext(INITIAL_STATE)

export const AuthContextProvider = ({children}) => {
    const [state, dispatch] = useReducer(AuthReducer, INITIAL_STATE)

    useEffect(() => {
        localStorage.setItem("id", JSON.stringify(state.id))
    }, [state.id])

    return (
        <AuthContext.Provider
            value={{
                id:state.id,
                loading: state.loading,
                error:state.error,
                dispatch}
            }>
            {children}
        </AuthContext.Provider>
    )
}