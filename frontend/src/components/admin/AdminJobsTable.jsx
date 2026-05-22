import  { useEffect, useState } from 'react';
import { Table, TableBody, TableCaption, TableCell, TableHead, TableHeader, TableRow } from '../ui/table';
import { Popover, PopoverContent, PopoverTrigger } from '../ui/popover';
import { Edit2, Eye, MoreHorizontal, Trash2 } from 'lucide-react';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { toast } from 'sonner';

const AdminJobsTable = () => { 
    const { allAdminJobs, searchJobByText } = useSelector(store => store.job);
    const [filterJobs, setFilterJobs] = useState(allAdminJobs);
    const navigate = useNavigate();

    useEffect(() => { 
        const filteredJobs = allAdminJobs.filter((job) => {
            if (!searchJobByText) return true;
            return job?.title?.toLowerCase().includes(searchJobByText.toLowerCase()) || 
                   job?.company?.name.toLowerCase().includes(searchJobByText.toLowerCase());
        });
        setFilterJobs(filteredJobs);
    }, [allAdminJobs, searchJobByText]);

    // Soft delete job
    const handleDelete = async (jobId) => {
        if (!window.confirm("Are you sure you want to delete this job?")) return;

        try {
            const response = await fetch(`https://localhost:44331/api/v1/job/deleteJob/${jobId}`, {
                method: "DELETE",
                headers: {
                        Authorization: `Bearer ${localStorage.getItem("token")}`,
                        "Content-Type": "application/json"
                    },
                credentials: "include",
                body: JSON.stringify({ isDeleted: true }),
            });

            const data = await response.json();
            if (!response.ok) {
                throw new Error(data.message || "Failed to delete job.");
            }

            // Remove job from UI after deletion
            setFilterJobs((prevJobs) => prevJobs.filter((job) => job.id !== jobId));

            toast.success("Job deleted successfully!");
        } catch (error) {
            console.error("Error deleting job:", error);
            toast.error(error.message);
        }
    };

    return (
        <div>
            <Table>
                <TableCaption>A list of your recently posted jobs</TableCaption>
                <TableHeader>
                    <TableRow>
                        <TableHead>Company Name</TableHead>
                        <TableHead>Role</TableHead>
                        <TableHead>Date</TableHead>
                        <TableHead className="text-right">Action</TableHead>
                    </TableRow>
                </TableHeader>
                <TableBody>
                    {filterJobs?.map((job) => (
                        <TableRow key={job.id}>
                            <TableCell>{job?.company?.name}</TableCell>
                            <TableCell>{job?.title}</TableCell>
                            <TableCell>{job?.createdAt.split("T")[0]}</TableCell>
                            <TableCell className="text-right cursor-pointer">
                                <Popover>
                                    <PopoverTrigger><MoreHorizontal /></PopoverTrigger>
                                    <PopoverContent className="w-32">
                                        <div onClick={() => navigate(`/recruiter/jobs/edit/${job.id}`)} className='flex items-center gap-2 w-fit cursor-pointer'>
                                            <Edit2 className='w-4' />
                                            <span>Edit</span>
                                        </div>
                                        <div onClick={() => navigate(`/recruiter/jobs/${job.id}/applicants`)} className='flex items-center w-fit gap-2 cursor-pointer mt-2'>
                                            <Eye className='w-4'/>
                                            <span>Applicants</span>
                                        </div>
                                        <div onClick={() => handleDelete(job.id)} className='flex items-center w-fit gap-2 cursor-pointer text-red-500 mt-2'>
                                            <Trash2 className='w-4'/>
                                            <span>Delete</span>
                                        </div>
                                    </PopoverContent>
                                </Popover>
                            </TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </div>
    );
};

export default AdminJobsTable;
