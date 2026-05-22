import { Trash } from "lucide-react";
import { useEffect, useState } from "react";
import { toast } from "sonner";

const CompanyList = () => {
    const [companies, setCompanies] = useState([]);
    const [loading, setLoading] = useState(true);

    // Fetch companies function
    const fetchCompanies = async () => {
        try {
            const response = await fetch("https://localhost:44331/api/v1/admin/getAllCompanies",
                {
                    headers: {
                        Authorization: `Bearer ${localStorage.getItem("token")}`
                    }
                }
            );
            const data = await response.json();

            if (data && data.companies) {
                setCompanies(data.companies);
            } else {
                setCompanies([]);
            }
        } catch (error) {
            console.error("Error fetching companies:", error);
            setCompanies([]);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchCompanies();
    }, []);

    // Handle delete action
    const handleDelete = async (id) => {
        console.log("Attempting to delete company with ID:", id);

        try {
            const response = await fetch(`https://localhost:44331/api/v1/company/deleteCompany/${id}`, {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${localStorage.getItem("token")}`
                },
                body: JSON.stringify({ isRemove: true })
            });

            const responseData = await response.json();
            console.log("API Response:", responseData);

            if (response.ok) {
                console.log("Company deleted successfully!");

                // Update state immediately without refresh
                setCompanies(prevCompanies => prevCompanies.filter(company => company.id !== id));
            } else {
                toast.error(responseData.message,{duration:1500});
                console.error("Failed to delete company");
            }
        } catch (error) {
            console.error("Error deleting company:", error);
        }
    };

    return (
        <div className="p-6 bg-white shadow-lg rounded-lg">
            <h2 className="text-xl font-semibold mb-4">Company List</h2>

            {loading ? (
                <p>Loading...</p>
            ) : companies.length === 0 ? (
                <p>No companies found.</p>
            ) : (
                <table className="min-w-full border border-gray-300">
                    <thead>
                        <tr className="bg-gray-100">
                            <th className="p-2 border">Company Name</th>
                            <th className="p-2 border">Location</th>
                            <th className="p-2 border">Website</th>
                            <th className="p-2 border">Action</th>
                        </tr>
                    </thead>
                    <tbody>
                        {companies.map((company) => (
                            <tr key={company.id} className="border">

                                <td className="p-2 border">{company.name}</td>
                                <td className="p-2 border">{company.location || "N/A"}</td>
                                <td className="p-2 border">
                                    {company.website ? (
                                        <a href={company.website} target="_blank" rel="noopener noreferrer" className="text-blue-500">
                                            Visit Website
                                        </a>
                                    ) : (
                                        "No Website"
                                    )}
                                </td>
                                <td className="p-2 border">
                                    
                                        <Trash
                                            className="text-red-500 cursor-pointer hover:scale-110 transition-transform mx-auto"
                                            onClick={() => {
                                                const confirmDelete = window.confirm(
                                                    "Are you sure you want to delete this company?"
                                                );

                                                if (confirmDelete) {
                                                    handleDelete(company.id);
                                                }
                                            }}

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

export default CompanyList;
