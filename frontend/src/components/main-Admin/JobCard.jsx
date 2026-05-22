import React from "react";

const Card = ({ title, value, percentage, color }) => {
  return (
    <div className={`p-5 rounded-lg shadow-lg ${color} text-white`}>
      <div className="flex justify-between items-center">
        <span className="text-sm">{title}</span>
        <span className="text-xs">{percentage}</span>
      </div>
      <h2 className="text-2xl font-bold">{value}</h2>
      <div className="w-full h-10 mt-2 bg-white bg-opacity-20 rounded-md"></div>
    </div>
  );
};

export default Card;
