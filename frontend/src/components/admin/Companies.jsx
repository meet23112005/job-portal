import React, { useEffect, useState } from 'react';
import Navbar from '../shared/Navbar';
import { Input } from '../ui/input';
import { Button } from '../ui/button';
import CompaniesTable from './CompaniesTable';
import { useNavigate } from 'react-router-dom';
import useGetAllCompanies from '@/hooks/useGetAllCompanies';
import { useDispatch } from 'react-redux';
import { setSearchCompanyByText } from '@/redux/companySlice';
import axios from 'axios';
import { COMPANY_API_END_POINT } from '@/utils/constant';
import { toast } from 'sonner';

const Companies = () => {
    useGetAllCompanies();
    const [input, setInput] = useState("");
    const navigate = useNavigate();
    const dispatch = useDispatch();

    useEffect(() => {
        dispatch(setSearchCompanyByText(input));
    }, [input]);

    // ✅ Delete Company Function (Soft Delete)
    const deleteCompany = async (companyId) => {
        try {
            const res = await axios.put(`${COMPANY_API_END_POINT}/deleteCompany/${companyId}`, {}, {
                withCredentials: true
            });

            if (res.data.success) {
                toast.success("Company marked as removed.");
            }
        } catch (error) {
            console.error(error);
            toast.error(error.response?.data?.message || "Failed to remove company.");
        }
    };

    return (
        <div>
            <Navbar />
            <div className='max-w-6xl mx-auto my-10'>
                <div className='flex items-center justify-between my-5'>
                    <Input
                        className="w-fit"
                        placeholder="Filter by name"
                        onChange={(e) => setInput(e.target.value)}
                    />
                    <Button onClick={() => navigate("/recruiter/companies/create")}>New Company</Button>
                </div>
                {/* ✅ Pass delete function to CompaniesTable */}
                <CompaniesTable deleteCompany={deleteCompany} />
            </div>
        </div>
    );
};

export default Companies;
