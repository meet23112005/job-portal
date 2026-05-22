import React from "react";
import { Home, Users, Building2, BriefcaseBusiness } from "lucide-react";

const Sidebar = ({ setActiveSection }) => {
  return (
    <div className="w-64 h-screen bg-white shadow-lg p-5 flex flex-col">
      <h2 className="text-2xl font-bold text-blue-600">Dashboard</h2>
      <ul className="mt-6 space-y-4">
        <li
          className="flex items-center gap-3 text-gray-600 hover:text-blue-600 cursor-pointer"
          onClick={() => setActiveSection("dashboard")}
        >
          <Home size={20} /> Dashboard
        </li>
        <li
          className="flex items-center gap-3 text-gray-600 hover:text-blue-600 cursor-pointer"
          onClick={() => setActiveSection("recruiters")}
        >
          <Users size={20} /> Recruiters
        </li>
        <li
          className="flex items-center gap-3 text-gray-600 hover:text-blue-600 cursor-pointer"
          onClick={() => setActiveSection("jobSeekers")}
        >
          <Users size={20} /> Job Seekers
        </li>
        <li className="flex items-center gap-3 text-gray-600 hover:text-blue-600 cursor-pointer" onClick={() => setActiveSection("companies")}>
          <Building2 size={20} /> Companies
        </li>
        <li className="flex items-center gap-3 text-gray-600 hover:text-blue-600 cursor-pointer" onClick={() => setActiveSection("jobs")}>
          <BriefcaseBusiness size={20} /> Jobs
        </li>
      </ul>
    </div>
  );
};

export default Sidebar;
