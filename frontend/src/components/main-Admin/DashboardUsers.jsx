import { useEffect, useState } from "react";
import axios from "axios";

const DashboardUsers = () => {
  const [userCount, setUserCount] = useState(0);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchUsers = async () => {
      try {
        const response = await axios.get("https://localhost:44331/api/v1/user/getAllUser"); // API call
        setUserCount(response.data.users.length); // Store count
      } catch (err) {
        setError(err.response?.data?.message || err.message);
      } finally {
        setLoading(false);
      }
    };

    fetchUsers();
  }, []);

  if (loading) return <div className="text-center py-10">Loading users count...</div>;
  if (error) return <div className="text-center py-10 text-red-500">{error}</div>;

  return (
    <div className="p-6 border rounded-lg shadow-md bg-white text-center">
      <h2 className="text-xl font-semibold">Total Users</h2>
      <p className="text-3xl font-bold text-purple-600 mt-2">{userCount}</p>
    </div>
  );
};

export default DashboardUsers;
