import React, { useEffect, useState } from 'react';
import Navbar from '../shared/Navbar';
import { Label } from '../ui/label';
import { Input } from '../ui/input';
import { RadioGroup } from '../ui/radio-group';
import { Button } from '../ui/button';
import { Link, useNavigate } from 'react-router-dom';
import axios from 'axios';
import { USER_API_END_POINT } from '@/utils/constant';
import { toast } from 'sonner';
import { useDispatch, useSelector } from 'react-redux';
import { setLoading } from '@/redux/authSlice';
import { Loader2 } from 'lucide-react';

const Signup = () => {
    const [input, setInput] = useState({
        fullname: "",
        email: "",
        phoneNumber: "",
        password: "",
        role: "",
        file: ""
    });

    const { loading, user } = useSelector(store => store.auth);
    const dispatch = useDispatch();
    const navigate = useNavigate();

    const changeEventHandler = (e) => {
        setInput({ ...input, [e.target.name]: e.target.value });
    };

    const changeFileHandler = (e) => {
        setInput({ ...input, file: e.target.files?.[0] });
    };

   const submitHandler = async (e) => {
    e.preventDefault();

    const normalizedInput = {
        ...input,
        fullname: input.fullname.toLowerCase().trim(),
        email: input.email.toLowerCase().trim(),
        phoneNumber: input.phoneNumber.trim()
    };

    const phoneRegex = /^[6-9]\d{9}$/;
    if (!phoneRegex.test(normalizedInput.phoneNumber)) {
        toast.error("Please enter a valid 10-digit Indian phone number.");
        return;
    }

    const emailRegex = /^[a-zA-Z0-9._%+-]+@(gmail\.com|yahoo\.com)$/;
    if (!emailRegex.test(normalizedInput.email)) {
        toast.error("Please enter a valid Gmail or Yahoo email address.");
        return;
    }

    // formData declared FIRST
    const formData = new FormData();
    formData.append("fullname", normalizedInput.fullname);
    formData.append("email", normalizedInput.email);
    formData.append("phoneNumber", normalizedInput.phoneNumber);
    formData.append("password", normalizedInput.password);
    formData.append("role", normalizedInput.role);

    // photo is optional - only add if selected
    if (input.file) {
        formData.append("file", input.file);
    }

    try {
        dispatch(setLoading(true));
        const res = await axios.post(`${USER_API_END_POINT}/register`, formData, {
            headers: { 'Content-Type': "multipart/form-data" },
            withCredentials: true,
        });
        if (res.data.success) {
            navigate("/email-confirmation-pending", {
            state: { email: input.email }  // ← pass email to show on page
    });
        }
    } catch (error) {
        console.log(error);
        toast.error(error.response?.data?.message || "Something went wrong");
    } finally {
        dispatch(setLoading(false));
    }
};

    useEffect(() => {
        if (user) {
            navigate("/");
        }
    }, [user, navigate]);

    return (
        <div>
            <Navbar />
            <div className='flex items-center justify-center max-w-7xl mx-auto'>
                <form onSubmit={submitHandler} className='w-1/2 border border-gray-200 rounded-md p-4 my-10'>
                    <h1 className='font-bold text-xl mb-5'>Sign Up</h1>

                    <div className='my-2'>
                        <Label>Full Name</Label>
                        <Input
                            type="text"
                            value={input.fullname}
                            name="fullname"
                            onChange={(e) => changeEventHandler({
                                target: { name: 'fullname', value: e.target.value.toLowerCase() }
                            })}
                        />
                    </div>

                    <div className='my-2'>
                        <Label>Email</Label>
                        <Input
                            type="email"
                            value={input.email}
                            name="email"
                            onChange={(e) => changeEventHandler({
                                target: { name: 'email', value: e.target.value.toLowerCase() }
                            })}
                            placeholder="Enter Gmail or Yahoo email"
                        />
                    </div>

                    <div className='my-2'>
                        <Label>Phone Number</Label>
                        <Input
                            type="text"
                            value={input.phoneNumber}
                            name="phoneNumber"
                            onChange={changeEventHandler}
                            onInput={(e) => e.target.value = e.target.value.replace(/\D/g, '')}
                            maxLength="10"
                            placeholder="Enter 10-digit phone number"
                        />
                    </div>

                    <div className='my-2'>
                        <Label>Password</Label>
                        <Input type="password" value={input.password} name="password" onChange={changeEventHandler} />
                    </div>

                    <div className='flex items-center gap-4 my-5'>
                        <RadioGroup className="flex items-center gap-4 my-5">
                            <div className="flex items-center space-x-2">
                                <Input
                                    type="radio"
                                    name="role"
                                    value="student"
                                    checked={input.role === 'student'}
                                    onChange={changeEventHandler}
                                    className="w-4 h-4 cursor-pointer"
                                />
                                <Label htmlFor="r1">Job Seeker</Label>
                            </div>
                            <div className="flex items-center space-x-2">
                                <Input
                                    type="radio"
                                    name="role"
                                    value="recruiter"
                                    checked={input.role === 'recruiter'}
                                    onChange={changeEventHandler}
                                    className="w-4 h-4 cursor-pointer"
                                />
                                <Label htmlFor="r2">Recruiter</Label>
                            </div>
                        </RadioGroup>

                        <div className='flex items-center gap-2'>
                            <Label>Profile</Label>
                            <Input accept="image/*" type="file" onChange={changeFileHandler} className="cursor-pointer" />
                        </div>
                    </div>

                    {
                        loading ?
                            <Button className="w-full my-4">
                                <Loader2 className='mr-2 h-4 w-4 animate-spin' /> Please wait
                            </Button>
                            :
                            <Button type="submit" className="w-full my-4">Signup</Button>
                    }

                    <span className='text-sm'>
                        Already have an account? <Link to="/login" className='text-blue-600'>Login</Link>
                    </span>
                </form>
            </div>
        </div>
    );
};

export default Signup;
