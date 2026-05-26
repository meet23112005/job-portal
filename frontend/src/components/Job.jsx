/* eslint-disable react/prop-types */
import { useEffect, useState } from 'react';
import { Button } from './ui/button';
import { Bookmark, BookmarkCheck } from 'lucide-react';
import { Avatar, AvatarImage } from './ui/avatar';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import { toast } from 'sonner';                          // ✅ sonner not react-toastify
import { useSelector } from 'react-redux';
import { JOB_API_END_POINT } from '@/utils/constant';   // ✅ use constant
import { API_BASE_URL } from '@/utils/constant';     // ✅ use constant

const Job = ({ job }) => {
    const navigate = useNavigate();
    const [isSaved, setIsSaved] = useState(false);
    const { user } = useSelector(store => store.auth);  // ✅ Redux not localStorage

    useEffect(() => {
        const fetchSaveStatus = async () => {
            if (!job?.id || !user) return;              // ✅ guard
            try {
                const res = await axios.get(
                    `${JOB_API_END_POINT}/${job.id}/save-status`,  // ✅ correct URL
                    { withCredentials: true }                        // ✅ send cookie
                );
                setIsSaved(res.data.isSaved);
            } catch (error) {
                console.error('Error fetching save status:', error);
            }
        };
        fetchSaveStatus();
    }, [job?.id, user]);

   

    const handleSaveJob = async () => {
        if (!user) { navigate("/login"); return; }      // ✅ Redux check

        try {
            if (isSaved) {
                await axios.delete(
                    `${JOB_API_END_POINT}/unsave/${job.id}`,       // ✅ correct URL
                    { withCredentials: true }
                );
                setIsSaved(false);
                toast.success("Job removed from saved.");
            } else {
                await axios.post(
                    `${JOB_API_END_POINT}/saved-jobs/${job.id}`,         // ✅ correct URL
                    {},
                    { withCredentials: true }
                );
                setIsSaved(true);
                toast.success("Job saved successfully.");
            }
        } catch (error) {
            toast.error(error.response?.data?.message || "Failed to update save status.");
        }
    };

    const getTimeAgo = (dateString) => {
        if (!dateString) return "Unknown";

        const posted = new Date(dateString);
        const now = new Date();
        const diffMs = now - posted;               
        const diffDays = Math.floor(diffMs / (1000 * 60 * 60 * 24));       
        const diffMonths = Math.floor(diffDays / 30);                        
        const diffYears = Math.floor(diffDays / 365);                        

        if (diffDays === 0) return "Today";
        if (diffDays === 1) return "Yesterday";
        if (diffDays < 30) return `${diffDays} days ago`;
        if (diffMonths < 12) return diffMonths === 1
            ? "1 month ago"
            : `${diffMonths} months ago`;
        return diffYears === 1 ? "1 year ago" : `${diffYears} years ago`;
    };

    var logo = job?.company?.logo ?? "";
    logo = logo.includes("cloudinary") ? logo : `${API_BASE_URL}/${logo}`;  // ✅ handle relative URL

    return (
        <div className='p-5 rounded-md shadow-xl bg-white border border-gray-100 w-full max-w-md mx-auto md:max-w-lg lg:max-w-xl h-[350px] flex flex-col justify-between'>
            <div className='flex items-center justify-between'>
                <p className='text-sm text-gray-500'>
                    {getTimeAgo(job?.createdAt)}
                </p>
                <Button variant="outline" className="rounded-full" size="icon" onClick={handleSaveJob}>
                    {isSaved ? <BookmarkCheck /> : <Bookmark />}
                </Button>
            </div>

            <div className='flex items-center gap-3 my-2 flex-wrap'>

                <Avatar className="w-10 h-10">
                    <AvatarImage src={logo} />
                </Avatar>
                <div>
                    <h1 className='font-medium text-lg'>{job?.company?.name}</h1>
                    <p className='text-sm text-gray-500'>India</p>
                </div>
            </div>

            <h1 className='font-bold text-lg my-2'>{job?.title}</h1>
            <p className='text-sm text-gray-600'>{job?.description}</p>

            <div className='flex gap-4 mt-4'>
                <Button onClick={() => navigate(`/description/${job?.id}`)} variant="outline">
                    Details
                </Button>
                <Button
                    className={`w-full ${isSaved ? "bg-red-500" : "bg-[#7209b7]"}`}
                    onClick={handleSaveJob}
                >
                    {isSaved ? "Unsave" : "Save For Later"}
                </Button>
            </div>
        </div>
    );
};

export default Job;