import { createSlice } from "@reduxjs/toolkit";

const jobSlice = createSlice({
    name:"job",
    initialState:{
        allJobs:[],
        allAdminJobs:[],
        singleJob:null, 
        searchJobByText:"",
        allAppliedJobs:[],
        searchedQuery:"",
        filters: {
            keyword: "",
            location: ""
        },

        currentPage : 1,
        totalJobs: 0,
        totalPages: 1,
        pageSize: 9
    },
    reducers:{
        // actions
        setAllJobs:(state,action) => {
            state.allJobs = action.payload;
        },
        setSingleJob:(state,action) => {
            state.singleJob = action.payload;
        },
        setAllAdminJobs:(state,action) => {
            state.allAdminJobs = action.payload;
        },
        setSearchJobByText:(state,action) => {
            state.searchJobByText = action.payload;
        },
        setAllAppliedJobs:(state,action) => {
            state.allAppliedJobs = action.payload;
        },
        setSearchedQuery:(state,action) => {
            state.searchedQuery = action.payload;
        },
        setFilters:(state,action) => {
            state.filters = {
                ...state.filters,
                ...action.payload
            };
        },
        setFilterKeyword:(state, action) => {
            state.filters.keyword = action.payload;
            state.filters.location = '';
        },
        setFilterLocation:(state, action) => {
            state.filters.location = action.payload;
            state.filters.keyword = '';
        },
        setPagination:(state,action) => {
            state.currentPage = action.payload.currentPage;
            state.totalJobs = action.payload.totalJobs;
            state.totalPages = action.payload.totalPages;
        },
        setCurrentPage:(state,action) => {
            state.currentPage = action.payload;
        }
    }
});
export const {
    setAllJobs, 
    setSingleJob, 
    setAllAdminJobs,
    setSearchJobByText, 
    setAllAppliedJobs,
    setSearchedQuery,
    setFilters,
    setFilterKeyword,
    setFilterLocation,
    setPagination,
    setCurrentPage
} = jobSlice.actions;
export default jobSlice.reducer;