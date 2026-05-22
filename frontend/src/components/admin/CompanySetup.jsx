import React, { useEffect, useState } from 'react';
import Navbar from '../shared/Navbar';
import { Button } from '../ui/button';
import { ArrowLeft, Loader2 } from 'lucide-react';
import { Label } from '../ui/label';
import { Input } from '../ui/input';
import axios from 'axios';
import { COMPANY_API_END_POINT } from '@/utils/constant';
import { useNavigate, useParams } from 'react-router-dom';
import { toast } from 'sonner';
import { useSelector } from 'react-redux';
import useGetCompanyById from '@/hooks/useGetCompanyById';

const CompanySetup = () => {
    const params = useParams();
    useGetCompanyById(params.id);

    const [input, setInput] = useState({
        name: "",
        description: "",
        website: "",
        location: "",
        file: null
    });

    const [logoUrl, setLogoUrl] = useState(""); // Store old logo URL
    const { singleCompany } = useSelector(store => store.company);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(""); 
    const navigate = useNavigate();

    // Handle text inputs
    const changeEventHandler = (e) => {
        setInput({ ...input, [e.target.name]: e.target.value });
    };

    // Handle file input
    const changeFileHandler = (e) => {
        const file = e.target.files?.[0];

        if (!file) {
            setError("Company logo is required.");
        } else {
            setError(""); 
            setInput({ ...input, file });
        }
    };

    const submitHandler = async (e) => {
        e.preventDefault();

        const formData = new FormData();
        formData.append("name", input.name);
        formData.append("description", input.description);
        formData.append("website", input.website);
        formData.append("location", input.location);

        if (input.file) {
            formData.append("file", input.file); // Upload new file
        } else if (logoUrl) {
            formData.append("logo", logoUrl); // Keep old logo
        }

        try {
            setLoading(true);
            const res = await axios.put(`${COMPANY_API_END_POINT}/update/${params.id}`, formData, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                },
                withCredentials: true
            });

            if (res.data.success) {
                toast.success(res.data.message);
                navigate("/recruiter/companies");
            }
        } catch (error) {
            console.error(error);
            toast.error(error.response?.data?.message || "Failed to update company.");
        } finally {
            setLoading(false);
        }
    };

    // Populate form with existing company data
    useEffect(() => {
        setInput({
            name: singleCompany.name || "",
            description: singleCompany.description || "",
            website: singleCompany.website || "",
            location: singleCompany.location || "",
            file: null
        });

        if (singleCompany.logo) {
            setLogoUrl(singleCompany.logo); // Store old logo URL
        }
    }, [singleCompany]);

    return (
        <div>
            <Navbar />
            <div className='max-w-xl mx-auto my-10'>
                <form onSubmit={submitHandler}>
                    <div className='flex items-center gap-5 p-8'>
                        <Button onClick={() => navigate("/recruiter/companies")} variant="outline" className="flex items-center gap-2 text-gray-500 font-semibold">
                            <ArrowLeft />
                            <span>Back</span>
                        </Button>
                        <h1 className='font-bold text-xl'>Company Setup</h1>
                    </div>
                    <div className='grid grid-cols-2 gap-4'>
                        <div>
                            <Label>Company Name</Label>
                            <Input
                                type="text"
                                name="name"
                                value={input.name}
                                onChange={changeEventHandler}
                            />
                        </div>
                        <div>
                            <Label>Description</Label>
                            <Input
                                type="text"
                                name="description"
                                value={input.description}
                                onChange={changeEventHandler}
                            />
                        </div>
                        <div>
                            <Label>Website</Label>
                            <Input
                                type="text"
                                name="website"
                                value={input.website}
                                onChange={changeEventHandler}
                            />
                        </div>
                        <div>
                            <Label>Location</Label>
                            <Input
                                type="text"
                                name="location"
                                value={input.location}
                                onChange={changeEventHandler}
                            />
                        </div>
                        <div>
                            <Label>Logo</Label>
                            <Input
                                type="file"
                                accept="image/*"
                                onChange={changeFileHandler}
                            />
                            {/* Show old logo if available and no new file is selected */}
                            {logoUrl && !input.file && (
                                <img src={logoUrl} alt="Company Logo" className="mt-2 w-20 h-20 object-cover rounded" />
                            )}
                            {error && <p className="text-red-500 text-sm mt-1">{error}</p>}
                        </div>
                    </div>
                    {loading ? (
                        <Button className="w-full my-4">
                            <Loader2 className='mr-2 h-4 w-4 animate-spin' /> Please wait
                        </Button>
                    ) : (
                        <Button type="submit" className="w-full my-4">Update</Button>
                    )}
                </form> 
            </div>
        </div>
    );
};

export default CompanySetup;
