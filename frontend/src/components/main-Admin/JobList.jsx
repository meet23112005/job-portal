import { Trash, ArrowUpDown } from "lucide-react";
import { useEffect, useState } from "react";

const JobList = () => {
    const [jobs, setJobs] = useState([]);
    const [loading, setLoading] = useState(true);
    const [sortConfig, setSortConfig] = useState({ key: "createdAt", direction: "desc" });

    // Fetch jobs from API
    const fetchJobs = async () => {
        try {
            const response = await fetch("https://localhost:44331/api/v1/admin/getAllJobs", {
                headers: {
                    Authorization: `Bearer ${localStorage.getItem("token")}`,
                    "Content-Type": "application/json"
                }
            });
            const data = await response.json();
            console.log("API Response:", data);

            if (data && data.jobs) {
                setJobs(data.jobs);
            } else {
                setJobs([]);
            }
        } catch (error) {
            console.error("Error fetching jobs:", error);
            setJobs([]);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchJobs();

        window.addEventListener("focus", fetchJobs);

        return () => window.removeEventListener("focus", fetchJobs);
    }, []);

    const handleSort = (key) => {
        let direction = "asc";
        if (sortConfig.key === key && sortConfig.direction === "asc") {
            direction = "desc";
        }
        setSortConfig({ key, direction });
    }
    

    // Handle delete action
    const handleDelete = async (id) => {
        console.log("Attempting to delete job with ID:", id);

        try {
            const response = await fetch(`https://localhost:44331/api/v1/job/deleteJob/${id}`, {
                method: "DELETE",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${localStorage.getItem("token")}`

                },
            });

            if (response.ok) {
                console.log("Job deleted successfully!");

                // Update state to remove the deleted job
                setJobs(prevJobs => prevJobs.filter(job => job.id !== id));
            } else {
                console.error("Failed to delete job");
            }
        } catch (error) {
            console.error("Error deleting job:", error);
        }
    };

    const sortedJobs = [...jobs].sort((a, b) => {
        const { key, direction } = sortConfig;

        let avalue = a[key];
        let bvalue = b[key];

        if (key === "company") {
            avalue = a.company?.name || "";
            bvalue = b.company?.name || "";
        }

        if (key === "createdAt") {
            avalue = new Date(a.createdAt);
            bvalue = new Date(b.createdAt);
        }

        if (avalue < bvalue) {
            return direction === "asc" ? -1 : 1;
        }
        if (avalue > bvalue) {
            return direction === "asc" ? 1 : -1;
        }

        return 0;
    });
    return (
        <div className="p-6 bg-white shadow-lg rounded-lg">
            <h2 className="text-xl font-semibold mb-4">Job List</h2>

            {loading ? (
                <p>Loading...</p>
            ) : jobs.length === 0 ? (
                <p>No jobs found.</p>
            ) : (
                <table className="min-w-full border border-gray-300">
                    <thead>
                        <tr className="bg-gray-100">
                            <th
                                className="p-2 border cursor-pointer"
                                onClick={() => handleSort("title")}
                            >
                                <div className="flex items-center justify-center gap-1">
                                    Title
                                    <ArrowUpDown size={16} />
                                </div>
                            </th>
                            <th
                                className="p-2 border cursor-pointer"
                                onClick={() => handleSort("company")}
                            >
                                <div className="flex items-center justify-center gap-1">
                                    Company
                                    <ArrowUpDown size={16} />
                                </div>
                            </th>
                            <th
                                className="p-2 border cursor-pointer"
                                onClick={() => handleSort("location")}
                            >
                                <div className="flex items-center justify-center gap-1">
                                    Location
                                    <ArrowUpDown size={16} />
                                </div>
                            </th>
                            <th
                                className="p-2 border cursor-pointer"
                                onClick={() => handleSort("salary")}
                            >
                                <div className="flex items-center justify-center gap-1">
                                    Salary
                                    <ArrowUpDown size={16} />
                                </div>
                            </th>
                            <th
                                className="p-2 border cursor-pointer"
                                onClick={() => handleSort("createdAt")}
                            >
                                <div className="flex items-center justify-center gap-1">
                                    Posted On
                                    <ArrowUpDown size={16} />
                                </div>
                            </th>
                            <th className="p-2 border">IsActive</th>
                            <th className="p-2 border">Action</th>
                        </tr>
                    </thead>
                    <tbody>
                        {sortedJobs.map((job) => (
                            <tr key={job.id} className="border">
                                <td className="p-2 border">{job.title}</td>
                                <td className="p-2 border">{job.company?.name || "N/A"}</td>
                                <td className="p-2 border">{job.location || "N/A"}</td>
                                <td className="p-2 border">
                                    {job.salary ? `${job.salary} LPA` : "Not specified"}
                                </td>
                                <td className="p-2 border">
                                    {new Date(job.createdAt).toLocaleDateString()}
                                </td>
                                <td className="p-2 border">
                                    {job.isRemoved === false ? "Active" : "NotActive"}
                                </td>
                                <td className="p-2 border">
                                    <Trash
                                        className="text-red-500 cursor-pointer hover:scale-110 transition-transform mx-auto"
                                        onClick={() => handleDelete(job.id)}
                                    />
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            )}
        </div>
    );
};

export default JobList;
