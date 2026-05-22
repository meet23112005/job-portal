import  { useState } from "react";
import { Popover, PopoverContent, PopoverTrigger } from "../ui/popover";
import { Button } from "../ui/button";
import { Avatar, AvatarImage, AvatarFallback } from "../ui/avatar";
import { LogOut, User2, Menu, X } from "lucide-react";
import { Link, useNavigate, useLocation } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";
import axios from "axios";
import { API_BASE_URL, USER_API_END_POINT } from "@/utils/constant";
import { setUser } from "@/redux/authSlice";
import { toast } from "sonner";

const Navbar = () => {
  const { user } = useSelector((store) => store.auth);
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const location = useLocation();
  const [menuOpen, setMenuOpen] = useState(false);
  
  const logoutHandler = async () => {
    try {
      const res = await axios.get(`${USER_API_END_POINT}/logout`, {
        withCredentials: true,
      });
      if (res.data.success) {
        localStorage.removeItem("user");
        localStorage.removeItem("token");
        dispatch(setUser(null));
        navigate("/");
        toast.success(res.data.message);
      }
    } catch (error) {
      console.log(error);
      toast.error(error.response?.data?.message || "Something went wrong!");
    }
  };

  const isAdminDashboard = location.pathname.startsWith("/admin/dashboard");

  // Define a static default profile picture
  const defaultProfilePhoto = "../../public/admin.png"; // Add this image to your public folder
  //  console.log(user)
  // Function to get profile picture
  const getProfilePhoto = () => {
    if (window.location.href === "http://localhost:5173/admin/dashboard" &&
      (!user?.profile?.profilePhoto || user?.profile?.profilePhoto === "")) {
      return defaultProfilePhoto;
    }
    if(user?.profile?.profilePhoto){
      return `${API_BASE_URL}${user.profile.profilePhoto}`;
    }else{
      return "https://ui-avatars.com/api/?name=" + user?.fullname + "&background=6A38C2&color=fff";
    }
    //return user?.profile?.profilePhoto || "https://ui-avatars.com/api/?name=" + user?.fullname + "&background=6A38C2&color=fff";
  };

  return (
    <div className="bg-white shadow-md">
      <div className="flex items-center justify-between mx-auto max-w-7xl px-4 py-3">
        {/* Logo */}
        <div>
          <h1 className="text-2xl font-bold">
            Dream<span className="text-purple-700">Job</span>
          </h1>
        </div>

        {/* Desktop Menu */}
        <div className="hidden md:flex items-center gap-10">
          <ul className="flex font-medium items-center gap-5">
            {!isAdminDashboard && (
              user && user.role === "recruiter" ? (
                <>
                <Link to="/" className="hover:text-[#F83002]">
                      Home
                    </Link>
                  <li>
                    <Link to="/recruiter/companies" className="hover:text-[#F83002]">
                      Companies
                    </Link>
                  </li>
                  <li>
                    <Link to="/recruiter/jobs" className="hover:text-[#F83002]">
                      Jobs
                    </Link>
                  </li>
                </>
              ) : (
                <>
                  <li>
                    <Link to="/" className="hover:text-[#F83002]">
                      Home
                    </Link>
                  </li>
                  <li>
                    <Link to="/jobs" className="hover:text-[#F83002]">
                      Jobs
                    </Link>
                  </li>
                  <li>
                    <Link to="/browse" className="hover:text-[#F83002]">
                      Browse
                    </Link>
                  </li>
                </>
              )
            )}
          </ul>

          {/* Auth Buttons / Profile */}
          {!user ? (
            <div className="flex items-center gap-2">
              <Link to="/login">
                <Button variant="outline">Login</Button>
              </Link>
              <Link to="/signup">
                <Button className="bg-[#6A38C2] hover:bg-[#5b30a6]">
                  Signup
                </Button>
              </Link>
            </div>
          ) : (
            <Popover>
              <PopoverTrigger asChild>
                <Avatar className="cursor-pointer">
                  <AvatarImage src={getProfilePhoto()} alt="Profile" />
                  {<AvatarFallback className="bg-purple-600 text-white cursor-pointer">
                    {user?.fullname?.charAt(0).toUpperCase()}
                  </AvatarFallback> }
                </Avatar>
              </PopoverTrigger>
              <PopoverContent className="w-80">
                <div className="">
                  <div className="flex gap-2 space-y-2">
                    <Avatar className="cursor-pointer">
                      <AvatarImage src={getProfilePhoto()} alt="Profile" />

                    </Avatar>
                    <div>
                      <h4 className="font-medium">{user?.fullname}</h4>
                      <p className="text-sm text-gray-500">{user?.profile?.bio}</p>
                    </div>
                  </div>
                  <div className="flex flex-col my-2 text-gray-600">
                    {user?.role === "student" && (
                      <div className="flex w-fit items-center gap-2 cursor-pointer">
                        <User2 />
                        <Button variant="link">
                          <Link to="/profile">View Profile</Link>
                        </Button>
                      </div>
                    )}
                    {user?.role === "recruiter" && (
                      <div className="flex w-fit items-center gap-2 cursor-pointer">
                        <User2 />
                        <Button variant="link">
                          <Link to="/recruiter/profile">View Profile</Link>
                        </Button>
                      </div>
                    )}
                    <div className="flex w-fit items-center gap-2 cursor-pointer">
                      <LogOut />
                      <Button onClick={logoutHandler} variant="link">
                        Logout
                      </Button>
                    </div>
                  </div>
                </div>
              </PopoverContent>
            </Popover>
          )}
        </div>

        {/* Mobile Menu Button */}
        <button className="md:hidden p-2" onClick={() => setMenuOpen(!menuOpen)}>
          {menuOpen ? <X size={28} /> : <Menu size={28} />}
        </button>
      </div>

      {/* Mobile Menu */}
      {menuOpen && (
        <div className="md:hidden bg-white py-4 px-6 shadow-md">
          <ul className="flex flex-col space-y-3">
            {!isAdminDashboard && (
              user && user.role === "recruiter" ? (
                <>
                  <li>
                    <Link to="/recruiter/companies" className="hover:text-[#F83002]">
                      Companies
                    </Link>
                  </li>
                  <li>
                    <Link to="/recruiter/jobs" className="hover:text-[#F83002]">
                      Jobs
                    </Link>
                  </li>
                </>
              ) : (
                <>
                  <li>
                    <Link to="/" className="hover:text-[#F83002]">
                      Home
                    </Link>
                  </li>
                  <li>
                    <Link to="/jobs" className="hover:text-[#F83002]">
                      Jobs
                    </Link>
                  </li>
                  <li>
                    <Link to="/browse" className="hover:text-[#F83002]">
                      Browse
                    </Link>
                  </li>
                </>
              )
            )}
          </ul>

          {/* Mobile Auth Buttons / Profile */}
          <div className="mt-4">
            {!user ? (
              <div className="flex flex-col space-y-2">
                <Link to="/login">
                  <Button variant="outline" className="w-full">
                    Login
                  </Button>
                </Link>
                <Link to="/signup">
                  <Button className="w-full bg-[#6A38C2] hover:bg-[#5b30a6]">
                    Signup
                  </Button>
                </Link>
              </div>
            ) : (
              <div className="mt-4">
                <div className="flex gap-2 items-center">
                  <Avatar className="cursor-pointer">
                    <AvatarImage src={getProfilePhoto()} alt="Profile" />
                  </Avatar>
                  <div>
                    <h4 className="font-medium">{user?.fullname}</h4>
                    <p className="text-sm text-gray-500">{user?.profile?.bio}</p>
                  </div>
                </div>
                <div className="mt-2">
                  <Button onClick={logoutHandler} variant="outline" className="w-full">
                    Logout
                  </Button>
                </div>
              </div>
            )}
          </div>
        </div>
      )}
    </div>
  );
};

export default Navbar;