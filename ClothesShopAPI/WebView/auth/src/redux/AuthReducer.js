export const INITIAL_STATE = {
    id: localStorage.getItem("id") !== "undefined"?
        JSON.parse(localStorage.getItem("id")) || null
        : null,
    loading: false,
    error: null
}

export const AuthReducer = (state, action) => {
    switch(action.type){
        case "LOGIN_START":
            return {
                id: null,
                loading: true,
                error: null
            }
        case "LOGIN_SUCCESS":
            console.log(action);
            return {
                id: action.payload.id,
                loading: false,
                error: null
            }
        case "LOGIN_FAILURE":
            return {
                id: null,
                loading: false,
                error: action.payload
            }
        case "LOGOUT":
            return {
                id: null,
                loading: false,
                error: null
            }
        default:
            return state
    }
}