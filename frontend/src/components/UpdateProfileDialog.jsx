import { useState } from 'react'
import { useLocation } from 'react-router-dom'
import { Dialog, DialogContent, DialogFooter, DialogHeader, DialogTitle } from './ui/dialog'
import { Label } from './ui/label'
import { Input } from './ui/input'
import { Button } from './ui/button'
import { Avatar, AvatarImage } from './ui/avatar'
import { Loader2 } from 'lucide-react'
import { useDispatch, useSelector } from 'react-redux'
import axios from 'axios'
import { API_BASE_URL, USER_API_END_POINT } from '@/utils/constant'
import { setUser } from '@/redux/authSlice'
import { toast } from 'sonner'

// eslint-disable-next-line react/prop-types
const UpdateProfileDialog = ({ open, setOpen }) => {
    const [loading, setLoading] = useState(false);
    const { user } = useSelector(store => store.auth);
    const dispatch = useDispatch();
    const location = useLocation();

    const isRecruiterProfile = location.pathname === "/recruiter/profile";

    const [input, setInput] = useState({
        fullname: user?.fullname || "",
        email: user?.email || "",
        phoneNumber: user?.phoneNumber || "",
        bio: user?.profile?.bio || "",
        skills: user?.profile?.skills?.join(", ") || "",
        profilePhoto: `${API_BASE_URL}`+user?.profile?.profilePhoto || null,
        resume: user?.profile?.resume || null
    });

    // Handle text input changes
    const changeEventHandler = (e) => {
        const { name, value } = e.target;
        setInput(prev => ({
            ...prev,
            [name]: name === "phoneNumber" ? value.replace(/\D/g, "").slice(0, 10) : value
        }));
    };

    // Handle file uploads
    const fileChangeHandler = (e) => {
        const file = e.target.files?.[0];

        // If profile photo is updated, show preview immediately
        if (e.target.name === "profilePhoto") {
            setInput(prev => ({
                ...prev,
                profilePhoto: URL.createObjectURL(file),
                profilePhotoFile: file
            }));
        } else if (e.target.name === "resume") {  // handle resume separately
            setInput(prev => ({ ...prev, resumeFile: file })); // store as resumeFile
        }
    };

    // Handle form submission
    const submitHandler = async (e) => {
        e.preventDefault();
        const formData = new FormData();
        formData.append("fullname", input.fullname);
        formData.append("email", input.email);
        formData.append("phoneNumber", input.phoneNumber);

        if (!isRecruiterProfile) {
            formData.append("bio", input.bio);
            formData.append("skills", input.skills);
            if (input.resumeFile) {
                formData.append("resume", input.resumeFile);
            }
        }

        // Append profile photo if updated
        if (input.profilePhotoFile) {
            formData.append("file", input.profilePhotoFile);
        }

        try {
            setLoading(true);
            const res = await axios.put(`${USER_API_END_POINT}/profile/update`, formData, {
                headers: { 'Content-Type': 'multipart/form-data' },
                withCredentials: true
            });

            if (res.data.success) {
                dispatch(setUser(res.data.user));
                toast.success(res.data.message);
            }
        } catch (error) {
            console.error(error);
            toast.error(error.response?.data?.message || "Something went wrong!");
        } finally {
            setLoading(false);
            setOpen(false);
        }
    };

    return (
        <Dialog open={open} onOpenChange={setOpen}>
            <DialogContent className="sm:max-w-[425px]">
                <DialogHeader>
                    <DialogTitle>Update Profile</DialogTitle>
                </DialogHeader>
                <form onSubmit={submitHandler}>
                    <div className='grid gap-4 py-4'>

                        {/* Profile Photo */}
                        <div className="flex flex-col items-center">
                            <Avatar className="h-24 w-24">
                                <AvatarImage src={input.profilePhoto} alt="Profile" />
                            </Avatar>
                            <Label htmlFor="profilePhoto" className="mt-2 text-blue-500 cursor-pointer">Change Photo</Label>
                            <Input
                                id="profilePhoto"
                                name="profilePhoto"
                                type="file"
                                accept="image/*"
                                className="hidden"
                                onChange={fileChangeHandler}
                            />
                        </div>

                        {/* Name */}
                        <div className='grid grid-cols-4 items-center gap-4'>
                            <Label htmlFor="fullname" className="text-right">Name</Label>
                            <Input
                                id="fullname"
                                name="fullname"
                                type="text"
                                value={input.fullname}
                                onChange={changeEventHandler}
                                className="col-span-3"
                            />
                        </div>

                        {/* Email */}
                        <div className='grid grid-cols-4 items-center gap-4'>
                            <Label htmlFor="email" className="text-right">Email</Label>
                            <Input
                                id="email"
                                name="email"
                                type="email"
                                value={input.email}
                                onChange={changeEventHandler}
                                className="col-span-3"
                            />
                        </div>

                        {/* Phone Number */}
                        <div className='grid grid-cols-4 items-center gap-4'>
                            <Label htmlFor="phoneNumber" className="text-right">Number</Label>
                            <Input
                                id="phoneNumber"
                                name="phoneNumber"
                                type="text"
                                value={input.phoneNumber}
                                onChange={changeEventHandler}
                                maxLength={10}
                                className="col-span-3"
                            />
                        </div>

                        {/* Only show these fields if NOT on Recruiter Profile */}
                        {!isRecruiterProfile && (
                            <>
                                {/* Bio */}
                                <div className='grid grid-cols-4 items-center gap-4'>
                                    <Label htmlFor="bio" className="text-right">Bio</Label>
                                    <Input
                                        id="bio"
                                        name="bio"
                                        value={input.bio}
                                        onChange={changeEventHandler}
                                        className="col-span-3"
                                    />
                                </div>

                                {/* Skills */}
                                <div className='grid grid-cols-4 items-center gap-4'>
                                    <Label htmlFor="skills" className="text-right">Skills</Label>
                                    <Input
                                        id="skills"
                                        name="skills"
                                        value={input.skills}
                                        onChange={changeEventHandler}
                                        className="col-span-3"
                                    />
                                </div>

                                {/* Resume Upload */}
                                <div className='grid grid-cols-4 items-center gap-4'>
                                    <Label htmlFor="file" className="text-right">Resume</Label>
                                    <Input
                                        id="resume"
                                        name="resume"
                                        type="file"
                                        accept="application/pdf"
                                        onChange={fileChangeHandler}
                                        className="col-span-3"
                                    />
                                </div>
                            </>
                        )}
                    </div>

                    {/* Submit Button */}
                    <DialogFooter>
                        {loading ? (
                            <Button className="w-full my-4" disabled>
                                <Loader2 className='mr-2 h-4 w-4 animate-spin' /> Please wait
                            </Button>
                        ) : (
                            <Button type="submit" className="w-full my-4">Update</Button>
                        )}
                    </DialogFooter>
                </form>
            </DialogContent>
        </Dialog>
    );
};

export default UpdateProfileDialog;
