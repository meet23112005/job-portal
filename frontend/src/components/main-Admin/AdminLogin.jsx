import { useState, useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";
import { setUser } from "../../redux/authSlice";
import axios from "axios";
import { toast } from "sonner";

const AdminLogin = () => {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const { user } = useSelector((store) => store.auth);

  const [emailId, setEmailId] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);
  // const adminEmail = "admin@gmail.com";
  // const adminPassword = "admin123";

  useEffect(() => {
    // Redirect if already logged in
    if (user?.role === "admin") {
      navigate("/admin/dashboard");
    }
  }, [user, navigate]);

  const handleLogin = async (e) => {
    e.preventDefault();
    setLoading(true);

    // if (emailId === adminEmail && password === adminPassword) {
    //     const adminData = { role: "admin", email: emailId };

    //     // Save admin session in Redux and localStorage
    //     dispatch(setUser(adminData));
    //     localStorage.setItem("user", JSON.stringify(adminData));
    // } else {
    //     setError("Invalid email or password");
    // }

    try {
      const res = await axios.post(`https://localhost:44331/api/v1/admin/login`,
        { email: emailId, password },
        { withCredentials: true });

      if (res.data.success) {
        //token in localstorage
        localStorage.setItem("token", res.data.token);

        //set admin user in redux
        dispatch(setUser({
          role: "admin",
          email: emailId
        }));
        toast.success("Admin login successful!");
        navigate("/admin/dashboard");
      }
    } catch (error) {
      toast.error(error?.response?.data?.message || "Invalid Credentials.");
    } finally {
      setLoading(false);
    }
  };

  return (
    // <div className="flex justify-center items-center h-screen bg-gray-100">
    //   <div className="bg-white p-8 rounded-lg shadow-lg w-96">
    //     <h2 className="text-2xl font-semibold text-center mb-4">Admin Login</h2>
    //     {error && <p className="text-red-500 text-center">{error}</p>}
    //     <form onSubmit={handleLogin}>
    //       <div className="mb-4">
    //         <label className="block text-gray-700">Email ID</label>
    //         <input
    //           type="email"
    //           value={emailId}
    //           onChange={(e) => setEmailId(e.target.value)}
    //           className="w-full px-3 py-2 border rounded-lg focus:outline-none focus:ring focus:border-blue-300"
    //           required
    //         />
    //       </div>
    //       <div className="mb-4">
    //         <label className="block text-gray-700">Password</label>
    //         <input
    //           type="password"
    //           value={password}
    //           onChange={(e) => setPassword(e.target.value)}
    //           className="w-full px-3 py-2 border rounded-lg focus:outline-none focus:ring focus:border-blue-300"
    //           required
    //         />
    //       </div>
    //       <button
    //         type="submit"
    //         className="w-full bg-blue-500 text-white py-2 rounded-lg hover:bg-blue-600"
    //       >
    //         Login
    //       </button>
    //     </form>
    //   </div>
    // </div>
    <div className="flex justify-center items-center h-screen bg-gray-100">
            <div className="bg-white p-8 rounded-lg shadow-lg w-96">
                <h2 className="text-2xl font-semibold text-center mb-4">
                    Admin Login
                </h2>
                <form onSubmit={handleLogin}>
                    <div className="mb-4">
                        <label className="block text-gray-700">Email</label>
                        <input
                            type="email"
                            value={emailId}
                            onChange={e => setEmailId(e.target.value)}
                            className="w-full px-3 py-2 border rounded-lg"
                            required
                        />
                    </div>
                    <div className="mb-4">
                        <label className="block text-gray-700">Password</label>
                        <input
                            type="password"
                            value={password}
                            onChange={e => setPassword(e.target.value)}
                            className="w-full px-3 py-2 border rounded-lg"
                            required
                        />
                    </div>
                    <button
                        type="submit"
                        disabled={loading}
                        className="w-full bg-blue-500 text-white py-2 rounded-lg hover:bg-blue-600 disabled:opacity-50"
                    >
                        {loading ? "Logging in..." : "Login"}
                    </button>
                </form>
            </div>
        </div>
  );
};

export default AdminLogin;
