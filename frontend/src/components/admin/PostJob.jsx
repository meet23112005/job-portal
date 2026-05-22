import React, { useState } from 'react';
import Navbar from '../shared/Navbar';
import { Label } from '../ui/label';
import { Input } from '../ui/input';
import { Button } from '../ui/button';
import { useSelector } from 'react-redux';
import { Select, SelectContent, SelectGroup, SelectItem, SelectTrigger, SelectValue } from '../ui/select';
import axios from 'axios';
import { JOB_API_END_POINT } from '@/utils/constant';
import { toast } from 'sonner';
import { useNavigate } from 'react-router-dom';
import { Loader2 } from 'lucide-react';

const PostJob = () => {
    const [input, setInput] = useState({
        title: "",
        description: "",
        requirements: "",
        salary: 0,
        location: "",
        jobType: "",
        experienceLevel: "" ,
        position: 0,
        companyId: ""
    });

    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();
    const { companies } = useSelector(store => store.company);

    const changeEventHandler = (e) => {
        const { name, value } = e.target;

        if (["position", "salary"].includes(name)) {
            const newValue = Math.max(0, parseInt(value,10) || 0); // 🔹 Ensure value is never negative
            setInput({ ...input, [name]: newValue });
        } else {
            setInput({ ...input, [name]: value });
        }
    };

    const selectChangeHandler = (value) => {
        const selectedCompany = companies.find(company => company.name.toLowerCase() === value);
        setInput({ ...input, companyId: selectedCompany.id });
    };

    const submitHandler = async (e) => {
        e.preventDefault();
        if (input.position < 0 || input.salary < 0 || input.experienceLevel < 0) {
            toast.error("Salary, Experience, and Position cannot be negative.");
            return;
        }

        try {
            setLoading(true);
            console.log("Payload:", input);
            const res = await axios.post(`${JOB_API_END_POINT}/post`, input, {
                headers: {
                    'Content-Type': 'application/json'
                },
                withCredentials: true
            });

            if (res.data.success) {
                console.log(res.data.message);
                toast.success(res.data.message);
                navigate("/recruiter/jobs");
            }
        } catch (error) {
             console.log("Full error response:", error.response?.data);
            toast.error(error.response?.data?.message || "Failed to post job.");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div>
            <Navbar />
            <div className='flex items-center justify-center w-screen my-5'>
                <form onSubmit={submitHandler} className='p-8 max-w-4xl border border-gray-200 shadow-lg rounded-md'>
                    <div className='grid grid-cols-2 gap-2'>
                        <div>
                            <Label>Title</Label>
                            <Input type="text" name="title" value={input.title} onChange={changeEventHandler} className="focus-visible:ring-offset-0 focus-visible:ring-0 my-1" />
                        </div>
                        <div>
                            <Label>Description</Label>
                            <Input type="text" name="description" value={input.description} onChange={changeEventHandler} className="focus-visible:ring-offset-0 focus-visible:ring-0 my-1" />
                        </div>
                        <div>
                            <Label>Requirements</Label>
                            <Input type="text" name="requirements" value={input.requirements} onChange={changeEventHandler} className="focus-visible:ring-offset-0 focus-visible:ring-0 my-1" />
                        </div>
                        <div>
                            <Label>Salary (lakh per year)</Label>
                            <Input 
                                type="number" 
                                name="salary" 
                                value={input.salary} 
                                onChange={changeEventHandler} 
                                className="focus-visible:ring-offset-0 focus-visible:ring-0 my-1"
                                min="0" // 🔹 Prevents negative input
                            />
                        </div>
                        <div>
                            <Label>Location</Label>
                            <Input type="text" name="location" value={input.location} onChange={changeEventHandler} className="focus-visible:ring-offset-0 focus-visible:ring-0 my-1" />
                        </div>
                        <div>
                            <Label>Job Type</Label>
                            <Select onValueChange={(value) => setInput({ ...input, jobType: value })}>
                                <SelectTrigger className="w-full my-1">
                                    <SelectValue placeholder="Select Job Type" />
                                </SelectTrigger>
                                <SelectContent>
                                    <SelectGroup>
                                        <SelectItem value="remote">Remote</SelectItem>
                                        <SelectItem value="on-site">On-Site</SelectItem>
                                    </SelectGroup>
                                </SelectContent>
                            </Select>
                        </div>
                        <div>
                            <Label>Experience Level</Label>
                            <Input 
                                type="text" 
                                name="experienceLevel" 
                                value={input.experienceLevel} 
                                onChange={changeEventHandler} 
                                className="focus-visible:ring-offset-0 focus-visible:ring-0 my-1"
                               
                            />
                        </div>
                        <div>
                            <Label>No of Position</Label>
                            <Input 
                                type="number" 
                                name="position" 
                                value={input.position} 
                                onChange={changeEventHandler} 
                                className="focus-visible:ring-offset-0 focus-visible:ring-0 my-1"
                                min="0" // 🔹 Prevents negative input
                            />
                        </div>
                        {
                            companies.length > 0 && (
                                <Select onValueChange={selectChangeHandler}>
                                    <SelectTrigger className="w-[180px]">
                                        <SelectValue placeholder="Select a Company" />
                                    </SelectTrigger>
                                    <SelectContent>
                                        <SelectGroup>
                                            {
                                                companies.map(company => (
                                                    <SelectItem key={company.id} value={company.name.toLowerCase()}>
                                                        {company.name}
                                                    </SelectItem>
                                                ))
                                            }
                                        </SelectGroup>
                                    </SelectContent>
                                </Select>
                            )
                        }
                    </div>
                    {
                        loading ? <Button className="w-full my-4"> <Loader2 className='mr-2 h-4 w-4 animate-spin' /> Please wait </Button> : <Button type="submit" className="w-full my-4">Post New Job</Button>
                    }
                    {
                        companies.length === 0 && <p className='text-xs text-red-600 font-bold text-center my-3'>*Please register a company first, before posting a job</p>
                    }
                </form>
            </div>
        </div>
    );
};

export default PostJob;
