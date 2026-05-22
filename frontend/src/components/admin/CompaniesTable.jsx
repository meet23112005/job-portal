import { useEffect, useState } from 'react';
import { Table, TableBody, TableCaption, TableCell, TableHead, TableHeader, TableRow } from '../ui/table';
import { Avatar, AvatarImage } from '../ui/avatar';
import { Popover, PopoverContent, PopoverTrigger } from '../ui/popover';
import { Edit2, MoreHorizontal, Trash2 } from 'lucide-react';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import { COMPANY_API_END_POINT } from '@/utils/constant';
import { toast } from 'sonner';

const CompaniesTable = () => {
    const { companies, searchCompanyByText } = useSelector(store => store.company);
    const [filterCompany, setFilterCompany] = useState(companies);
    const navigate = useNavigate();

    useEffect(() => {
        console.log(companies);
        const filteredCompany = companies.length >= 0 && companies.filter((company) => {
            if (!searchCompanyByText) {
                return true;
            }
            
            return company?.name?.toLowerCase().includes(searchCompanyByText.toLowerCase());
        });
        setFilterCompany(filteredCompany);
    }, [companies, searchCompanyByText]);

    
    //  Delete Company Function (Soft Delete)
    const deleteCompany = async (companyId) => {
        try {
            const res = await axios.put(`${COMPANY_API_END_POINT}/deleteCompany/${companyId}`, {}, {
                withCredentials: true
            });

            if (res.data.success) {
                toast.success("Company marked as removed.");
                setFilterCompany(filterCompany.filter((company) => company.id !== companyId)); // Remove from UI
            }
        } catch (error) {
            console.error(error);
            toast.error(error.response?.data?.message || "Failed to remove company.");
        }
    };

    return (
        <div>
            <Table>
                <TableCaption>A list of your recent registered companies</TableCaption>
                <TableHeader>
                    <TableRow>
                        <TableHead>Logo</TableHead>
                        <TableHead>Name</TableHead>
                        <TableHead>Date</TableHead>
                        <TableHead className="text-right">Action</TableHead>
                    </TableRow>
                </TableHeader>
                <TableBody>
                    {filterCompany?.map((company) => (
                        <TableRow key={company.id}>
                            <TableCell>
                                <Avatar>
                                    {/* here some logo's are get from cloudinary or some get from local so we put this condition */}
                                    <AvatarImage 
                                            src={company?.logo?.includes("https://res.cloudinary.com")
                                                                    ?company.logo
                                                                    :"https://localhost:44331/"+company.logo} />
                                </Avatar>
                            </TableCell>
                            <TableCell>{company.name}</TableCell>
                            <TableCell>{company.createdAt.split("T")[0]}</TableCell>
                            <TableCell className="text-right cursor-pointer">
                                <Popover>
                                    <PopoverTrigger><MoreHorizontal /></PopoverTrigger>
                                    <PopoverContent className="w-32">
                                        <div
                                            onClick={() => navigate(`/recruiter/companies/${company.id}`, { state: company })}
                                            className='flex items-center gap-2 w-full mt-2  rounded-md text-black hover:bg-gray-100'
                                        >
                                            <Edit2 className='w-4' />
                                            <span>Edit</span>
                                        </div>
                                        <button
                                            onClick={() => deleteCompany(company.id)}
                                            className="flex items-center gap-2 w-full mt-2  rounded-md text-black hover:bg-gray-100"
                                        >
                                            <Trash2 className="w-4" />
                                            Delete
                                        </button>
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

export default CompaniesTable;
