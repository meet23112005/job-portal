import { API_BASE_URL } from "@/utils/constant";
import axios from "axios";
import { Trash } from "lucide-react";
import { useEffect, useState } from "react";

const JobSeekerList = () => {
  const [jobSeekers, setJobSeekers] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchJobSeekers = async () => {
      try {
        const response = await axios.get(
          "https://localhost:44331/api/v1/user/getAllJobSeeker",
          {
            headers:{
              Authorization : `Bearer ${localStorage.getItem("token")}`
            }
          }
        );

        setJobSeekers(response.data.users || []);
        
      } catch (error) {
        console.error("Error fetching job seekers:", error);
        setJobSeekers([]);
      } finally {
        setLoading(false);
      }
    };

    fetchJobSeekers();
  }, []);

  const handleDelete = async (id) => {
    try {
      const response = await fetch(
        `https://localhost:44331/api/v1/user/removeUser/${id}`,
        {
          method: "DELETE",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${localStorage.getItem("token")}`
          },
        }
      );

      if (response.ok) {
        setJobSeekers((prevSeekers) =>
          prevSeekers.filter((seeker) => seeker.id !== id)
        );
      } else {
        console.error("Failed to delete job seeker");
      }
    } catch (error) {
      console.error("Error deleting job seeker:", error);
    }
  };

  return (
    <div className="p-6 bg-white shadow-lg rounded-lg">
      <h2 className="text-xl font-semibold mb-4">Job Seeker List</h2>

      {loading ? (
        <p>Loading...</p>
      ) : jobSeekers.length === 0 ? (
        <p>No job seekers found.</p>
      ) : (
        <table className="min-w-full border border-gray-300">
          <thead>
            <tr className="bg-gray-100">
              <th className="p-2 border">Profile</th>
              <th className="p-2 border">Name</th>
              <th className="p-2 border">Email</th>
              <th className="p-2 border">Phone</th>
              <th className="p-2 border">Skills</th>
              <th className="p-2 border">Resume</th>
              <th className="p-2 border">Action</th>
            </tr>
          </thead>
          <tbody>
            {jobSeekers.map((seeker) => (
              <tr key={seeker.id} className="border">
                <td className="p-2 border">
                  <img
                    src={
                      `${API_BASE_URL}`+seeker.profile?.profilePhoto ||
                      "https://via.placeholder.com/50"
                    }
                    alt="Profile"
                    className="w-10 h-10 rounded-full"
                  />
                </td>
                <td className="p-2 border">{seeker.fullname}</td>
                <td className="p-2 border">{seeker.email}</td>
                <td className="p-2 border">{seeker.phoneNumber || "N/A"}</td>
                <td className="p-2 border">
                  {seeker.profile?.skills?.join(", ") || "No skills listed"}
                </td>
                <td className="p-2 border">
                  {seeker.profile?.resume ? (
                    <a
                      href={`https://localhost:44331${seeker.profile.resume}`}
                      target="_blank"
                      rel="noopener noreferrer"
                      className="text-blue-500 hover:underline"
                    >
                      View Resume
                    </a>
                  ) : (
                    "No Resume"
                  )}
                </td>

                <td className="p-2 border">
                  {!seeker.isRemove && (
                    <Trash
                      className="text-red-500 cursor-pointer hover:scale-110 transition-transform mx-auto"
                      onClick={() => handleDelete(seeker.id)}
                    />
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
};

export default JobSeekerList;
