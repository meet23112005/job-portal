import { useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { motion } from 'framer-motion';
import Navbar from './shared/Navbar';
import FilterCard from './FilterCard';
import Job from './Job';
import Pagination from './shared/Pagination';
import { setCurrentPage } from '@/redux/jobSlice';
import useGetAllJobs from '@/hooks/useGetAllJobs';

const Jobs = () => {
    useGetAllJobs();

    const dispatch = useDispatch();
    const {
        allJobs,
        searchedQuery,
        currentPage,
        totalPages,
        totalJobs,
        pageSize
    } = useSelector(store => store.job);

    const [filterJobs, setFilterJobs] = useState(allJobs);

    useEffect(() => {
        if (searchedQuery) {
            const filteredJobs = allJobs.filter((job) => {
                return (
                    job.title.toLowerCase().includes(searchedQuery.toLowerCase()) ||
                    job.description.toLowerCase().includes(searchedQuery.toLowerCase()) ||
                    job.location.toLowerCase().includes(searchedQuery.toLowerCase())
                );
            });
            setFilterJobs(filteredJobs);
        } else {
            setFilterJobs(allJobs);
        }
    }, [allJobs, searchedQuery]);

    // HANDLE PAGE CHANGE
    const handlePageChange = (page) => {
        dispatch(setCurrentPage(page));
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    };

    return (
        <div>
            <Navbar />

            <div className='max-w-7xl mx-auto mt-5'>
                <div className='flex gap-5'>
                    {/* FILTER SIDEBAR */}
                    <div className='w-[20%]'>
                        <FilterCard />
                    </div>

                    {/* MAIN CONTENT AREA */}
                    <div
                        className='flex-1 overflow-y-auto pb-5'
                        style={{ height: 'calc(100vh - 100px)' }}
                    >
                        {/* JOB COUNT */}
                        <div className='flex justify-between items-center px-4 mb-4'>
                            <p className='text-sm text-gray-500'>
                                Showing{" "}
                                <span className='font-semibold text-gray-800'>
                                    {((currentPage - 1) * pageSize) + 1}
                                    {" - "}
                                    {currentPage * pageSize > totalJobs ? totalJobs : currentPage * pageSize}
                                </span>
                                {" "}of{" "}
                                <span className='font-semibold text-gray-800'>
                                    {totalJobs}
                                </span>
                                {" "}jobs
                            </p>
                        </div>

                        {/* JOBS DISPLAY CONTAINER */}
                        {filterJobs.length <= 0 ? (
                            <div className='text-center py-10'>
                                <span className='text-gray-500 text-lg'>
                                    Job not found
                                </span>
                            </div>
                        ) : (
                            <>
                                {/* JOB GRID */}
                                <div className='grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4 p-4'>
                                    {filterJobs
                                        .filter(job => !job.isRemoved)
                                        .map((job) => (
                                            <motion.div
                                                key={job?.id}
                                                initial={{ opacity: 0, x: 100 }}
                                                animate={{ opacity: 1, x: 0 }}
                                                exit={{ opacity: 0, x: -100 }}
                                                transition={{ duration: 0.3 }}
                                            >
                                                <Job job={job} />
                                            </motion.div>
                                        ))
                                    }
                                </div>

                                {/* PAGINATION */}
                                <Pagination
                                    currentPage={currentPage}
                                    totalPages={totalPages}
                                    onPageChange={handlePageChange}
                                />
                            </>
                        )}
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Jobs;