import { useEffect } from "react";
import { useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";

const RootRedirect = () => {
    const {user} = useSelector((state) => state.auth);
    const navigate = useNavigate();
    useEffect(() => {
        if(!user){
            navigate("/login");
            return;
        }

        if(user.role === "admin"){
            navigate("/admin/dashboard");
        }else {
            navigate("/home");
        }
    }, [user, navigate]);
  return null;
}

export default RootRedirect