import { useState } from "react";
import Sidebar from "./Sidebar";
import DashboardJobs from "./DashboardJobs";
import Navbar from "../shared/Navbar";
import DashboardCompanies from "./DashboardCompanies";
import DashboardUsers from "./DashboardUsers";
import RecruiterList from "./RecruiterList";
import JobSeekerList from "./JobSeekerList";
import CompanyList from "./CompanyList";
import JobList from "./JobList";

const Dashboard = () => {
  const [activeSection, setActiveSection] = useState("dashboard");

  return (
    <div className="flex">
      <Sidebar setActiveSection={setActiveSection} />
      <div className="flex-1 flex flex-col">
        <Navbar />
        <h1 className="text-2xl font-bold p-4">Admin Dashboard</h1>

        {/* Dynamic Content Rendering */}
        {activeSection === "dashboard" && (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 p-4">
            <DashboardJobs />
            <DashboardCompanies />
            <DashboardUsers />
          </div>
        )}
        {activeSection === "recruiters" && <RecruiterList />}
        {activeSection === "jobSeekers" && <JobSeekerList />}
        {activeSection === "companies" && <CompanyList />}
        {activeSection === "jobs" && <JobList />}
      </div>
    </div>
  );
};

export default Dashboard;
