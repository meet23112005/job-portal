import { useEffect } from "react";
import { useSelector, useDispatch } from "react-redux";
import { useNavigate } from "react-router-dom";
import { setUser } from "../../redux/authSlice";

const AdminProtectedRoute = ({ children }) => {
    const { user } = useSelector(store => store.auth);
    const dispatch = useDispatch();
    const navigate = useNavigate();

    useEffect(() => {
        const storedAdmin = localStorage.getItem("user");
        
        // Only update Redux state if user is not already set
        if (!user && storedAdmin) {
            dispatch(setUser(JSON.parse(storedAdmin))); 
        }

        // Redirect if no admin user is found
        if (!storedAdmin) {
            navigate("/admin/login");
        }
    }, [dispatch, navigate, user]); // Only update when these dependencies change

    return user?.role === "admin" ? children : null;
};

export default AdminProtectedRoute;
