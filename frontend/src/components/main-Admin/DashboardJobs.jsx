import  { useEffect, useState } from "react";
import axios from "axios";

const DashboardJobs = () => {
  const [jobCount, setJobCount] = useState(0);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    axios
      .get("https://localhost:44331/api/v1/admin/getAllJobs",{
        headers:{
          Authorization: `Bearer ${localStorage.getItem("token")}`
        }
      })
      .then((response) => {
        setJobCount(response.data.jobs.length); // Get job count
        setLoading(false);
      })
      .catch((err) => {
        setError(err.message);
        setLoading(false);
      });
  }, []);

  if (loading) return <div className="text-center py-10">Loading job count...</div>;
  if (error) return <div className="text-center py-10 text-red-500">{error}</div>;

  return (
    <div className="p-6 border rounded-lg shadow-md bg-white text-center">
      <h2 className="text-xl font-semibold">Total Jobs Available</h2>
      <p className="text-3xl font-bold text-blue-600 mt-2">{jobCount}</p>
    </div>
  );
};

export default DashboardJobs;
