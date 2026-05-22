import { createSlice } from "@reduxjs/toolkit";

// Safe parse - auto clears bad data
const stored = localStorage.getItem("user");
let user = null;
try {
    user = stored ? JSON.parse(stored) : null;
} catch (e) {
    user = null;
    localStorage.removeItem("user"); // auto clear corrupted data
}
const initialState = {
    loading: false,
    user: user,
};

const authSlice = createSlice({
    name: "auth",
    initialState,
    reducers: {
        setLoading: (state, action) => {
            state.loading = action.payload;
        },
        setUser: (state, action) => {
            state.user = action.payload;
            if (action.payload) {
                localStorage.setItem("user", JSON.stringify(action.payload));
                // ↑ save student + recruiter + admin 
            } else {
                localStorage.removeItem("user");
            }
        },
        logout: (state) => {
            state.user = null;
            localStorage.removeItem("user");
            localStorage.removeItem("token");
        },
    },
});

export const { setLoading, setUser, logout } = authSlice.actions;
export default authSlice.reducer;