// import React from 'react'
// import { Table, TableBody, TableCaption, TableCell, TableHead, TableHeader, TableRow } from './ui/table'
// import { Badge } from './ui/badge'
// import { useSelector } from 'react-redux'

// const AppliedJobTable = () => {
//     const {allAppliedJobs} = useSelector(store=>store.job);
//     console.log("====allAppliedJobs=====>",allAppliedJobs);

//     return (
//         <div>
//             <Table>
//                 <TableCaption>A list of your applied jobs</TableCaption>
//                 <TableHeader>
//                     <TableRow>
//                         <TableHead>Date</TableHead>
//                         <TableHead>Job Role</TableHead>
//                         <TableHead>Company</TableHead>
//                         <TableHead className="text-right">Status</TableHead>
//                     </TableRow>
//                 </TableHeader>
//                 <TableBody>
//                     {
//                         allAppliedJobs.length <= 0 ? <span>You haven't applied any job yet.</span> : allAppliedJobs.map((appliedJob) => (

//                             <TableRow key={appliedJob._id}>
//                                 <TableCell>{appliedJob?.createdAt?.split("T")[0]}</TableCell>
//                                 <TableCell>{appliedJob.job?.title}</TableCell>
//                                 <TableCell>{appliedJob.job?.company?.name}</TableCell>
//                                 <TableCell className="text-right"><Badge className={`${appliedJob?.status === "rejected" ? 'bg-red-400' : appliedJob.status === 'pending' ? 'bg-gray-400' : 'bg-green-400'}`}>{appliedJob.status.toUpperCase()}</Badge></TableCell>
//                             </TableRow>
//                         ))
//                     }
//                 </TableBody>
//             </Table>
//         </div>
//     )
// }

// export default AppliedJobTable

import { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import {
  Table,
  TableBody,
  TableCaption,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "./ui/table";
import { Badge } from "./ui/badge";
import axios from "axios";
import { APPLICATION_API_END_POINT } from '@/utils/constant';


const AppliedJobTable = () => {
  const [allAppliedJobs, setAllAppliedJobs] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  // useEffect(() => {
  //   const fetchAppliedJobs = async () => {
  //     try {
  //       const response = await axios.get(
  //         "http://localhost:3000/api/v1/application/applied/:userId"
  //       );

  //       console.log("API Response:", response.data);

  //       // ✅ Get the actual job array
  //       const jobs = response.data.applications || [];

  //       setAllAppliedJobs(jobs);
  //     } catch (err) {
  //       console.error("Error fetching applied jobs:", err);
  //       setError("Failed to load applied jobs.");
  //     } finally {
  //       setLoading(false);
  //     }
  //   };

  //   fetchAppliedJobs();
  // }, []);
  const { user } = useSelector((store) => store.auth);

  useEffect(() => {
    const fetchAppliedJobs = async () => {
      try {
        //const userId = localStorage.getItem("user"); // it's already the ID, no need to parse
        const userId = user?.id;
        if (!userId) {
          throw new Error("User ID not found");
        }

        const response = await axios.get(
          `${APPLICATION_API_END_POINT}/get`,
          { withCredentials: true }
        );

        console.log("API Response:", response.data);
        const jobs = response.data.application || [];

        setAllAppliedJobs(jobs);
      } catch (err) {
        console.error("Error fetching applied jobs:", err);
        // If 404 means no applications found - not an error!
        if (err?.response?.status === 404) {
          setAllAppliedJobs([]); // just set empty array
        } else {
          setError("Failed to load applied jobs.");
        }
      } finally {
        setLoading(false);
      }
    };

    fetchAppliedJobs();
  }, []);

  if (loading) return <div>Loading applied jobs...</div>;
  if (error) return <div className="text-red-500">{error}</div>;

  return (
    <div className="p-4">
      <Table>
        <TableCaption>A list of your applied jobs</TableCaption>
        <TableHeader>
          <TableRow>
            <TableHead>Job Role</TableHead>
            <TableHead>Company</TableHead>
            <TableHead>Location</TableHead>
            <TableHead>Salary</TableHead>
            <TableHead className="text-right">Status</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {Array.isArray(allAppliedJobs) && allAppliedJobs.length > 0 ? (
            allAppliedJobs.map((appliedJob, index) => (
              <TableRow key={index}>
                <TableCell>{appliedJob?.job?.title || "N/A"}</TableCell>
                <TableCell>{appliedJob?.job?.company?.name || "N/A"}</TableCell>
                <TableCell>{appliedJob?.job?.location || "N/A"}</TableCell>
                <TableCell>
                  ₹{appliedJob?.job?.salary?.toLocaleString() + "LPA" || "N/A"}
                </TableCell>
                <TableCell className="text-right">
                  <Badge
                    className={
                      appliedJob?.status === "rejected"
                        ? "bg-red-400"
                        : appliedJob?.status === "pending"
                          ? "bg-gray-400"
                          : "bg-green-400"
                    }
                  >
                    {appliedJob?.status?.toUpperCase() || "UNKNOWN"}
                  </Badge>
                </TableCell>
              </TableRow>
            ))
          ) : (
            <TableRow>
              <TableCell colSpan={5} className="text-center text-gray-500">
                {"You haven't applied for any jobs yet."}
              </TableCell>
            </TableRow>
          )}
        </TableBody>
      </Table>
    </div>
  );
};

export default AppliedJobTable;
