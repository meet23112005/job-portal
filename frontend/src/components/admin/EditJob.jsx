import  { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import axios from 'axios';
import { JOB_API_END_POINT } from '@/utils/constant';
import { toast } from 'sonner';
import { Input } from '../ui/input';
import { Label } from '../ui/label';
import { Button } from '../ui/button';
import Navbar from '../shared/Navbar';
import { Loader2 } from 'lucide-react';

const EditJob = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const [jobData, setJobData] = useState(null);
    const [loading, setLoading] = useState(false);

    // Fetch job details
    useEffect(() => {
        const fetchJobDetails = async () => {
            try {
                setLoading(true);
                console.log(`Fetching job details from: ${JOB_API_END_POINT}/job/${id}`);

                const res = await axios.get(`https://localhost:44331/api/v1/job/get/${id}`, {
                    withCredentials: true,
                });

                setJobData(res.data.job);
                console.log("Job data received:", res.data.job);
            } catch (error) {
                console.error("Error fetching job details:", error);
                toast.error('Failed to load job details.');
            } finally {
                setLoading(false);
            }
        };

        fetchJobDetails();
    }, [id]);

    // Handle input changes
    const handleChange = (e) => {
        const { name, value } = e.target;
        setJobData((prev) => ({
            ...prev,
            [name]: value,
        }));
    };

    // Handle form submission
    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            setLoading(true);
            console.log("Updating job with data:", jobData);

            const response = await fetch(`https://localhost:44331/api/v1/job/edit/${id}`, {
                method: "PUT",
                headers: {
                        Authorization: `Bearer ${localStorage.getItem("token")}`,
                        "Content-Type": "application/json"
                    },
                credentials: "include",
                body: JSON.stringify(jobData),
            });

            const data = await response.json();

            if (!response.ok) {
                throw new Error(data.message || "Failed to update job.");
            }

            toast.success("Job updated successfully!");
            navigate("/recruiter/jobs"); // Redirect after success
        } catch (error) {
            console.error("Error updating job:", error);
            toast.error(error.message);
        } finally {
            setLoading(false);
        }
    };

    if (loading) {
        return <div className="text-center mt-5"><Loader2 className="animate-spin w-8 h-8" /></div>;
    }

    if (!jobData) return <p className="text-center text-red-500">Job not found</p>;

    return (
        <div>
            <Navbar />
            <div className='flex items-center justify-center w-screen my-5'>
                <form onSubmit={handleSubmit} className='p-8 max-w-4xl border border-gray-200 shadow-lg rounded-md'>
                    <h1 className='text-2xl font-semibold underline mb-6'>Edit Job</h1>
                    <div className='grid grid-cols-2 gap-2'>
                        <div>
                            <Label>Title</Label>
                            <Input type="text" name="title" value={jobData.title} onChange={handleChange} />
                        </div>
                        <div>
                            <Label>Description</Label>
                            <Input type="text" name="description" value={jobData.description} onChange={handleChange} />
                        </div>
                        <div>
                            <Label>Requirements</Label>
                            <Input type="text" name="requirements" value={jobData.requirements} onChange={handleChange} />
                        </div>
                        <div>
                            <Label>Salary</Label>
                            <Input type="number" name="salary" value={jobData.salary} onChange={handleChange} />
                        </div>
                        <div>
                            <Label>Location</Label>
                            <Input type="text" name="location" value={jobData.location} onChange={handleChange} />
                        </div>
                        <div>
                            <Label>Job Type</Label>
                            <Input type="text" name="jobType" value={jobData.jobType} onChange={handleChange} />
                        </div>
                        <div>
                            <Label>Experience Level</Label>
                            <Input type="text" name="experienceLevel" value={jobData.experienceLevel} onChange={handleChange} />
                        </div>
                        <div>
                            <Label>No of Position</Label>
                            <Input type="text" name="location" value={jobData.position} onChange={handleChange} />
                        </div>
                    </div>
                    <Button type="submit" className="w-full mt-4">Update Job</Button>
                </form>
            </div>
        </div>
    );
};

export default EditJob;
