// import React, { useEffect, useState } from 'react';
// import { Badge } from './ui/badge';
// import { Button } from './ui/button';
// import { useParams } from 'react-router-dom';
// import axios from 'axios';
// import { APPLICATION_API_END_POINT, JOB_API_END_POINT } from '@/utils/constant';
// import { setSingleJob } from '@/redux/jobSlice';
// import { useDispatch, useSelector } from 'react-redux';
// import { toast } from 'sonner';
// import Navbar from './shared/Navbar';

// const JobDescription = () => {
//     const { singleJob } = useSelector(store => store.job);
//     const { user } = useSelector(store => store.auth);
//     const isInitiallyApplied = singleJob?.applications?.some(application => application.applicant === user?._id) || false;
//     const [isApplied, setIsApplied] = useState(isInitiallyApplied);

//     const params = useParams();
//     const jobId = params.id;
//     const dispatch = useDispatch();

//     const applyJobHandler = async () => {
//         try {
//             const res = await axios.get(`${APPLICATION_API_END_POINT}/apply/${jobId}`, { withCredentials: true });
//             if (res.data.success) {
//                 setIsApplied(true);
//                 const updatedSingleJob = { ...singleJob, applications: [...singleJob.applications, { applicant: user?._id }] };
//                 dispatch(setSingleJob(updatedSingleJob));
//                 toast.success(res.data.message);
//             }
//         } catch (error) {
//             console.log(error);
//             toast.error(error.response.data.message);
//         }
//     };

//     useEffect(() => {
//         const fetchSingleJob = async () => {
//             try {
//                 const res = await axios.get(`${JOB_API_END_POINT}/get/${jobId}`, { withCredentials: true });
//                 if (res.data.success) {
//                     dispatch(setSingleJob(res.data.job));
//                     setIsApplied(res.data.job.applications.some(application => application.applicant === user?._id));
//                 }
//             } catch (error) {
//                 console.log(error);
//             }
//         };
//         fetchSingleJob();
//     }, [jobId, dispatch, user?._id]);

//     return (
//         <div>
//             <Navbar />

//         <div className='max-w-4xl mx-auto my-10 p-6 bg-white shadow-lg rounded-lg'>
//             <div className='flex items-center justify-between border-b pb-4'>
//                 <div>
//                     <h1 className='font-bold text-2xl text-gray-900'>{singleJob?.title}</h1>
//                     <div className='flex items-center gap-3 mt-2'>
//                         <Badge className='bg-blue-100 text-blue-700 font-medium'>{singleJob?.position} Positions</Badge>
//                         <Badge className='bg-red-100 text-red-700 font-medium'>{singleJob?.jobType}</Badge>
//                         <Badge className='bg-purple-100 text-purple-700 font-medium'>{singleJob?.salary} LPA</Badge>
//                     </div>
//                 </div>
//                 <Button
//                     onClick={isApplied ? null : applyJobHandler}
//                     disabled={isApplied}
//                     className={`px-6 py-2 text-white font-medium rounded-lg transition ${isApplied ? 'bg-gray-500 cursor-not-allowed' : 'bg-purple-600 hover:bg-purple-500'}`}>
//                     {isApplied ? 'Already Applied' : 'Apply Now'}
//                 </Button>
//             </div>
//             <div className='mt-6 space-y-4'>
//                 <div className='text-lg text-gray-800'><span className='font-semibold'>Role:</span> {singleJob?.title}</div>
//                 <div className='text-lg text-gray-800'><span className='font-semibold'>Location:</span> {singleJob?.location}</div>
//                 <div className='text-lg text-gray-800'><span className='font-semibold'>Description:</span> {singleJob?.description}</div>
//                 <div className='text-lg text-gray-800'><span className='font-semibold'>Experience:</span> {singleJob?.experience} yrs</div>
//                 <div className='text-lg text-gray-800'><span className='font-semibold'>Salary:</span> {singleJob?.salary} LPA</div>
//                 <div className='text-lg text-gray-800'><span className='font-semibold'>Total Applicants:</span> {singleJob?.applications?.length}</div>
//                 <div className='text-lg text-gray-800'><span className='font-semibold'>Posted Date:</span> {singleJob?.createdAt.split('T')[0]}</div>
//             </div>
//         </div>
//         </div>
//     );
// };

// export default JobDescription;


import { useEffect, useState } from 'react';
import { Badge } from './ui/badge';
import { Button } from './ui/button';
import { useParams } from 'react-router-dom';
import axios from 'axios';
import { APPLICATION_API_END_POINT, JOB_API_END_POINT } from '@/utils/constant';
import { setSingleJob } from '@/redux/jobSlice';
import { useDispatch, useSelector } from 'react-redux';
import { toast } from 'sonner';
import Navbar from './shared/Navbar';

const JobDescription = () => {
  const { singleJob } = useSelector(store => store.job);
  const { user } = useSelector(store => store.auth);
  //const isInitiallyApplied = singleJob?.applications?.some(application => application.applicant === user?.id) || false;
  const params = useParams();
  const jobId = params.id;
  // const { allAppliedJobs } = useSelector(store => store.job);
  // const isInitiallyApplied = allAppliedJobs?.some(
  //   applied => applied?.job?.id === jobId
  // ) || false;
  const [isApplied, setIsApplied] = useState(false);
  const dispatch = useDispatch();

  const applyJobHandler = async () => {
    try {
      const res = await axios.post(`${APPLICATION_API_END_POINT}/apply/${jobId}`,
        {},
        {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("token")}`
          },
          withCredentials: true
        });
      if (res.data.success) {
        setIsApplied(true);
        console.log("Apply response:", res.data);
        const updatedSingleJob = {
          ...singleJob,
          applicationCount: (singleJob?.applicationCount ?? 0) + 1,
          applications: [
            ...(singleJob?.applications || []),
            { applicant: user?.id }
          ]
        };
        dispatch(setSingleJob(updatedSingleJob));
        toast.success(res.data.message);
      }
    } catch (error) {
      if(error.response.data.isRemoved === true)
          toast.error("This Job Is no longer avilable to apply");
      toast.error(error?.response?.data?.message || 'Failed to apply. Please try again later.');
    }
  };

  useEffect(() => {
    const fetchSingleJob = async () => {
      try {
        const res = await axios.get(`${JOB_API_END_POINT}/get/${jobId}`, { withCredentials: true });
        if (res.data.success) {
          console.log(res.data.job );
          dispatch(setSingleJob(res.data.job));
          //setIsApplied(res.data.job.applications?.some(application => application.applicantId?.toString() === user?.id) ?? false);
          setIsApplied(res.data.job.isApplied);
        }
      } catch (error) {
        toast.error('Error fetching job details. Please refresh the page.');
      }
    };
    fetchSingleJob();
  }, [jobId, dispatch, user?.id]);

  return (
    <div>
      <Navbar />
      <div className='max-w-4xl mx-auto my-10 p-6 bg-gradient-to-br from-blue-300 via-purple-300 to-pink-300 shadow-2xl rounded-xl border border-purple-400 transition-transform transform hover:scale-105 duration-300'>
        <div className='flex items-center justify-between border-b pb-4'>
          <div>
            <h1 className='font-bold text-3xl text-gray-900'>{singleJob?.title || 'N/A'}</h1>
            <div className='flex items-center gap-3 mt-2'>
              <Badge className='bg-blue-500 text-white font-medium px-3 py-1 rounded-full'>{singleJob?.position ?? '0'} Positions</Badge>
              <Badge className='bg-red-500 text-white font-medium px-3 py-1 rounded-full'>{singleJob?.jobType || 'N/A'}</Badge>
              <Badge className='bg-purple-500 text-white font-medium px-3 py-1 rounded-full'>{singleJob?.salary ? `${singleJob.salary} LPA` : 'N/A'}</Badge>
            </div>
          </div>{
            //  user.role != "recruiter"?

            <Button
              onClick={isApplied ? null : applyJobHandler}
              disabled={isApplied}
              aria-disabled={isApplied}
              className={`px-6 py-2 text-white font-medium rounded-lg transition ${isApplied
                ? 'bg-gray-500 cursor-not-allowed'
                : 'bg-purple-600 hover:bg-purple-500'
                }`}
            >
              {isApplied ? 'Already Applied' : 'Apply Now'}
            </Button>//: ""
          }
        </div>
        <div className='mt-6 space-y-4'>
          <div className='text-lg text-gray-800'><span className='font-semibold'>Role:</span> {singleJob?.title || 'N/A'}</div>
          <div className='text-lg text-gray-800'><span className='font-semibold'>Location:</span> {singleJob?.location || 'N/A'}</div>
          <div className='text-lg text-gray-800'><span className='font-semibold'>Description:</span> {singleJob?.description || 'No description available'}</div>
          <div className='text-lg text-gray-800'><span className='font-semibold'>Experience:</span> {singleJob?.experienceLevel ?? 'N/A'} yrs</div>
          <div className='text-lg text-gray-800'><span className='font-semibold'>Salary:</span> {singleJob?.salary ? `${singleJob.salary} LPA` : 'N/A'}</div>
          <div className='text-lg text-gray-800'><span className='font-semibold'>Total Applicants:</span> {singleJob?.applicationCount}</div>
          <div className='text-lg text-gray-800'><span className='font-semibold'>Posted Date:</span> {singleJob?.createdAt ? singleJob.createdAt.split('T')[0] : 'N/A'}</div>
        </div>
      </div>
    </div>
  );
};

export default JobDescription;
