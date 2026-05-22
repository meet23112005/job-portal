import React, { useState } from 'react';
import Navbar from '../shared/Navbar';
import { Label } from '../ui/label';
import { Input } from '../ui/input';
import { Button } from '../ui/button';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import { COMPANY_API_END_POINT } from '@/utils/constant';
import { toast } from 'sonner';
import { useDispatch } from 'react-redux';
import { setSingleCompany } from '@/redux/companySlice';

const CompanyCreate = () => {
    const navigate = useNavigate();
    const dispatch = useDispatch();

    const [companyName, setCompanyName] = useState('');
    const [error, setError] = useState(''); // Track validation error

    const registerNewCompany = async () => {
        if (!companyName || typeof companyName !== "string" || !companyName.trim()) {
            setError("Company name is required.");
            return;
        }

        try {
            console.log("Sending data:", { companyName: companyName.toLowerCase() });
            console.log("API Endpoint:", `${COMPANY_API_END_POINT}/register`);

            const res = await axios.post(
                `${COMPANY_API_END_POINT}/register`,
                { companyName: companyName.toLowerCase() }, // Convert to lowercase before sending
                {
                    headers: { 'Content-Type': 'application/json' },
                    withCredentials: true
                }
            );

            if (res?.data?.success) {
                dispatch(setSingleCompany(res.data.company));
                toast.success(res.data.message);
                navigate(`/recruiter/companies/${res.data.company.id}`);
            }
        } catch (error) {
            console.error("Error Response:", error.response?.data);
            toast.error(error.response?.data?.message || "Failed to create company. Please try again.");
        }
    };

    return (
        <div>
            <Navbar />
            <div className='max-w-4xl mx-auto'>
                <div className='my-10'>
                    <h1 className='font-bold text-2xl'>Your Company Name</h1>
                    <p className='text-gray-500'>What would you like to give your company name? You can change this later.</p>
                </div>

                <Label>Company Name</Label>
                <Input
                    type="text"
                    className="my-2"
                    placeholder="Google, Microsoft etc."
                    value={companyName}
                    onChange={(e) => {
                        setCompanyName(e.target.value);
                        if (error) setError(""); // Clear error when typing
                    }}
                    required
                />

                {error && <p className="text-red-500 text-sm mt-1">{error}</p>} {/* 🔹 Display error message */}

                <div className='flex items-center gap-2 my-10'>
                    <Button variant="outline" onClick={() => navigate("/recruiter/companies")}>Cancel</Button>
                    <Button onClick={registerNewCompany}>Continue</Button>
                </div>
            </div>
        </div>
    );
};

export default CompanyCreate;
