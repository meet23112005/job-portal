import { useEffect, useState } from "react";
import { Trash } from "lucide-react"; // Import trash icon

const RecruiterList = () => {
    const [recruiters, setRecruiters] = useState([]);

    useEffect(() => {
        fetchRecruiters();
    }, []);

    const fetchRecruiters = async () => {
        try {
            const response = await fetch("https://localhost:44331/api/v1/user/getAllRecruiter",
                {
                    headers: {
                        Authorization: `Bearer ${localStorage.getItem("token")}`
                    }
                });
            const data = await response.json();
            setRecruiters(data.users);
        } catch (error) {
            console.error("Error fetching recruiters:", error);
        }
    };

    const handleDelete = async (id) => {
        try {
            const response = await fetch(`https://localhost:44331/api/v1/user/removeUser/${id}`, {
                method: "DELETE",
                headers:{
                    Authorization:`Bearer ${localStorage.getItem("token")}`
                }
            });
            const data = await response.json();
            if (data.success) {
                setRecruiters(recruiters.filter(recruiter => recruiter.id !== id));
            }
        } catch (error) {
            console.error("Error deleting recruiter:", error);
        }
    };

    return (
        <div className="p-6">
            <h2 className="text-2xl font-bold mb-4 text-center">Recruiters List</h2>
            <table className="w-full border-collapse border border-gray-300">
                <thead>
                    <tr className="bg-gray-100">
                        <th className="border p-2">Actions</th>
                        <th className="border p-2">Name</th>
                        <th className="border p-2">Email</th>
                        <th className="border p-2">Phone</th>
                    </tr>
                </thead>
                <tbody>
                    {recruiters.length > 0 ? (
                        recruiters.map((recruiter) => (
                            <tr key={recruiter.id} className="text-center">

                                <td className="border p-2">{recruiter.fullname}</td>
                                <td className="border p-2">{recruiter.email}</td>
                                <td className="border p-2">{recruiter.phoneNumber}</td>
                                <td className="border p-2"> <Trash
                                    className="text-red-500 cursor-pointer hover:scale-110 transition-transform mx-auto"
                                    onClick={() => handleDelete(recruiter.id)}
                                /></td>
                            </tr>
                        ))
                    ) : (
                        <tr>
                            <td colSpan="4" className="border p-2 text-center">
                                No recruiters found.
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>
        </div>
    );
};

export default RecruiterList;
