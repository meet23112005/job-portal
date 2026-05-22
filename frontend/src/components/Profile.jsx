import { useState, useEffect } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { useSelector } from "react-redux";
import Navbar from "./shared/Navbar";
import { Avatar, AvatarImage } from "./ui/avatar";
import { Button } from "./ui/button";
import { Contact, Mail, Pen } from "lucide-react";
import { Badge } from "./ui/badge";
import { Label } from "./ui/label";
import AppliedJobTable from "./AppliedJobTable";
import UpdateProfileDialog from "./UpdateProfileDialog";
import axios from "axios";
import { API_BASE_URL } from "@/utils/constant";

const Profile = () => {
  const [open, setOpen] = useState(false);
  const { user } = useSelector((store) => store.auth);
  const location = useLocation();
  const navigate = useNavigate();
  const isRecruiterProfile = location.pathname === "/recruiter/profile";
  const [savedJobs, setSavedJobs] = useState([]);

  useEffect(() => {
    const fetchSavedJobs = async () => {
      try {
        const response = await axios.get(
          "https://localhost:44331/api/v1/job/saved",
          {
            headers: {
              Authorization: `Bearer ${localStorage.getItem("token")}`,
            },
          }
        );
        setSavedJobs(response.data.savedJobs);
      } catch (error) {
        console.error("Error fetching saved jobs:", error);
      }
    };

    if (!isRecruiterProfile) fetchSavedJobs();
  }, [isRecruiterProfile]);

  return (
       

    <div>
      <Navbar />
      <div className="max-w-4xl mx-auto bg-white border border-gray-200 rounded-2xl my-5 p-8">
        <div className="flex justify-between">
          <div className="flex items-center gap-4">
            <Avatar className="h-24 w-24">
              <AvatarImage src={`${API_BASE_URL}`+user?.profile?.profilePhoto} alt="profile" />
            </Avatar>
            <div>
              <h1 className="font-medium text-xl">{user?.fullname}</h1>
            </div>
          </div>
          <Button onClick={() => setOpen(true)} variant="outline">
            <Pen /> Edit Profile
          </Button>
        </div>

        <div className="my-5">
          <div className="flex items-center gap-3 my-2">
            <Mail />
            <span>{user?.email}</span>
          </div>
          <div className="flex items-center gap-3 my-2">
            <Contact />
            <span>{user?.phoneNumber}</span>
          </div>
        </div>

        {!isRecruiterProfile && (
          <>
            <h1 className="font-bold text-lg my-5">Skills</h1>
            <div className="flex flex-wrap gap-2">
              {user?.profile?.skills?.length ? (
                user?.profile?.skills.map((item, index) => (
                  <Badge key={index}>{item}</Badge>
                ))
              ) : (
                <span>NA</span>
              )}
            </div>

            <div className="my-5">
              <Label className="text-md font-bold">
                Resume </Label>
              {user?.profile?.resume ?(
                
                <a
                  href={`https://localhost:44331${user.profile.resume}`}
                  target="_blank"
                  rel="noopener noreferrer"
                  className="text-blue-500 hover:underline"
                >
                  {user.profile.resumeOriginalName}
                </a>
              ) : (
                <span>NA</span>
              )}
            </div>

            <div className="max-w-4xl mx-auto bg-white rounded-2xl">
              <h1 className="font-bold text-lg my-5">Applied Jobs</h1>
              <AppliedJobTable />
            </div>

            <div className="max-w-4xl mx-auto bg-white rounded-2xl my-5 p-8">
              <h1 className="font-bold text-lg mb-5">Saved Jobs</h1>
              {savedJobs.length > 0 ? (
                savedJobs.map((job, index) => (
                  <div
                    key={index}
                    className="bg-gray-100 p-4 rounded-lg mb-4 flex justify-between items-center"
                  >
                    <div>
                      <h2 className="font-semibold">{job.title}</h2>
                      <p>{job.company?.name}</p>
                      <div className="flex gap-2 my-2">
                        <Badge className="text-blue-700 bg-gray-200">{job.jobType}</Badge>
                        <Badge className="text-[#7209b7] bg-gray-200">
                          {job.salary} LPA
                        </Badge>
                      </div>
                    </div>
                    <Button
                      onClick={() => navigate(`/description/${job.id}`)}
                      variant="default"
                      className="bg-gray-800"
                    >
                      View Job
                    </Button>
                  </div>
                ))
              ) : (
                <p>No saved jobs found.</p>
              )}
            </div>
          </>
        )}

        <UpdateProfileDialog open={open} setOpen={setOpen} />
      </div>
    </div>
  );
};

export default Profile;
