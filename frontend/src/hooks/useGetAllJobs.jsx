import { setAllJobs, setPagination, setCurrentPage } from '@/redux/jobSlice'
import { JOB_API_END_POINT } from '@/utils/constant'
import axios from 'axios'
import { useEffect, useRef } from 'react'
import { useDispatch, useSelector } from 'react-redux'

const useGetAllJobs = () => {
    const dispatch = useDispatch();

    const {
        searchedQuery,
        filters: { keyword, location },
        currentPage,
        pageSize
    } = useSelector(store => store.job);

    const previousQueryRef = useRef({
        searchedQuery,
        keyword,
        location
    });

    useEffect(() => {
        if (!currentPage || !pageSize) return;

        const filterChanged =
            previousQueryRef.current.searchedQuery !== searchedQuery ||
            previousQueryRef.current.keyword !== keyword ||
            previousQueryRef.current.location !== location;

        if (filterChanged && currentPage !== 1) {
            previousQueryRef.current = { searchedQuery, keyword, location };
            dispatch(setCurrentPage(1));
            return;
        }

        previousQueryRef.current = { searchedQuery, keyword, location };

        const effectiveKeyword = [searchedQuery, keyword]
            .filter(Boolean)
            .join(' ');

        const fetchAllJobs = async () => {
            try {

                const params = new URLSearchParams();
                if (effectiveKeyword) params.set('keyword', effectiveKeyword);
                if (location) params.set('location', location);
                params.set('pageNumber', currentPage);
                params.set('pageSize', pageSize);
                
                const res = await axios.get(
                    `${JOB_API_END_POINT}/get?${params.toString()}`,
                    { withCredentials: true }
                );
                console.log("API RESPONSE:", res.data);
                console.log(
                    "Fetching jobs with params:",
                    params.toString()
                );
                if (res.data.success) {

                    dispatch(setAllJobs(res.data.jobs));

                    dispatch(setPagination({
                        currentPage: res.data.pageNumber,
                        totalJobs: res.data.total,
                        totalPages: res.data.totalPages
                    }));
                    console.log("total Jobs " + res.data.total);
                }


            } catch (error) {
                console.log(error);
            }
        }
        fetchAllJobs();
    }, [
        currentPage,
        searchedQuery,
        keyword,
        location,
        pageSize,
        dispatch
    ]);
};

export default useGetAllJobs