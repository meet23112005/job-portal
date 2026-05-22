import  { useEffect, useState } from "react";
import axios from "axios";

const DashboardCompanies = () => {
  const [companyCount, setCompanyCount] = useState(0);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchCompanies = async () => {
      try {
        const response = await axios.get("https://localhost:44331/api/v1/admin/getAllCompanies",
          {
            headers: {
              Authorization: `Bearer ${localStorage.getItem("token")}`
            }
          });
        setCompanyCount(response.data.companies.length);
      } catch (err) {
        setError(err.response?.data?.message || err.message);
      } finally {
        setLoading(false);
      }
    };

    fetchCompanies();
  }, []);

  if (loading) return <div className="text-center py-10">Loading company count...</div>;
  if (error) return <div className="text-center py-10 text-red-500">{error}</div>;

  return (
    <div className="p-6 border rounded-lg shadow-md bg-white text-center">
      <h2 className="text-xl font-semibold">Total Companies</h2>
      <p className="text-3xl font-bold text-green-600 mt-2">{companyCount}</p>
    </div>
  );
};

export default DashboardCompanies;
