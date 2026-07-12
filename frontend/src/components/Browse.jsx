import { useEffect } from 'react';
import Navbar from './shared/Navbar';
import Job from './Job';
import Pagination from './shared/Pagination';

import { useDispatch, useSelector } from 'react-redux';

import {
    setCurrentPage,
    setSearchedQuery
} from '@/redux/jobSlice';

import useGetAllJobs from '@/hooks/useGetAllJobs';

const Browse = () => {

    // FETCH JOBS
    useGetAllJobs();

    const dispatch = useDispatch();

    const {
        allJobs,
        currentPage,
        totalPages,
        totalJobs,
        pageSize
    } = useSelector(store => store.job);

    // RESET SEARCH WHEN COMPONENT UNMOUNT
    useEffect(() => {

        return () => {

            dispatch(setSearchedQuery(""));
            dispatch(setCurrentPage(1));
        };

    }, [dispatch,currentPage,pageSize   ]);

    // HANDLE PAGE CHANGE
    const handlePageChange = (page) => {

        dispatch(setCurrentPage(page));

        window.scrollTo({
            top: 0,
            behavior: "smooth"
        });
    };

    return (
        <div>

            <Navbar />

            <div className="max-w-7xl mx-auto my-10 px-4">

                {/* HEADER */}
                <div className="flex justify-between items-center mb-6">

                    <h1 className="font-bold text-2xl">
                        Search Results
                    </h1>

                    <p className="text-sm text-gray-500">

                        Showing{" "}

                        <span className="font-semibold text-black">

                            {allJobs.length > 0
                                ? ((currentPage - 1) * pageSize) + 1
                                : 0}

                            -

                            {Math.min(currentPage * pageSize, totalJobs)}

                        </span>

                        {" "}of{" "}

                        <span className="font-semibold text-black">

                            {totalJobs}

                        </span>

                        {" "}jobs
                    </p>

                </div>

                {/* NO JOBS */}
                {allJobs.length <= 0 ? (

                    <div className="text-center py-20">

                        <h2 className="text-xl font-semibold text-gray-600">
                            No Jobs Found
                        </h2>

                    </div>

                ) : (

                    <>
                        {/* JOB GRID */}
                        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">

                            {allJobs.map((job) => (

                                <Job
                                    key={job.id}
                                    job={job}
                                />
                            ))}

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
    );
};

export default Browse;  