import { setAllJobs, setPagination } from '@/redux/jobSlice'
import { JOB_API_END_POINT } from '@/utils/constant'
import axios from 'axios'
import { useEffect } from 'react'
import { useDispatch, useSelector } from 'react-redux'

const useGetAllJobs = () => {

    const dispatch = useDispatch();

    const {
        searchedQuery,
        currentPage,
        pageSize
    } = useSelector(store => store.job);

    useEffect(() => {
        if (!currentPage || !pageSize) return;

        const fetchAllJobs = async () => {

            try {

                const params = new URLSearchParams({
                    keyword: searchedQuery,
                    pageNumber: currentPage,
                    pageSize: pageSize
                });
                

                const res = await axios.get(
                    `${JOB_API_END_POINT}/get?${params.toString()}`,
                    { withCredentials: true }
                );
                console.log("API RESPONSE:", res.data);
                console.log(
                    "Fetching jobs with params:",
                    params.toString()
                );
                console.log({
                    searchedQuery,
                    currentPage,
                    pageSize
                });
                if (res.data.success) {

                    dispatch(setAllJobs(res.data.jobs));

                    dispatch(setPagination({
                        currentPage: res.data.pageNumber,
                        totalJobs: res.data.total,
                        totalPages: res.data.totalPages
                    }));
                    console.log("total Jobs "+res.data.total);
                }

            } catch (error) {

                console.log(error);
            }
        };

        fetchAllJobs();

    }, [
        currentPage,
        searchedQuery,
        pageSize,
        dispatch
    ]);
};

export default useGetAllJobs;