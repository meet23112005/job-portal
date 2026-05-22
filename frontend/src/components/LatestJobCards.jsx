import { Badge } from './ui/badge'
import { useNavigate } from 'react-router-dom'

const LatestJobCards = ({ job }) => {
    const navigate = useNavigate();
    
    return (
        <div 
            onClick={() => navigate(`/description/${job.id}`)} 
            className="p-5 rounded-md shadow-xl bg-white border border-gray-100 cursor-pointer flex flex-col justify-between h-full"
        >
            <div>
                <h1 className="font-medium text-lg">{job?.company?.name}</h1>
                <p className="text-sm text-gray-500">India</p>
            </div>
            <div>
                <h1 className="font-bold text-lg my-2">{job?.title}</h1>
                <p className="text-sm text-gray-600">{job?.description}</p>
            </div>
            
            {/* Properly aligned badges (Stacked Bottom) */}
            <div className="mt-8 flex  gap-4 ">
                <Badge className="text-blue-700 font-bold w-fit" variant="ghost">
                    {job?.position} Positions
                </Badge>
                <Badge className="text-[#F83002] font-bold w-fit" variant="ghost">
                    {job?.jobType}
                </Badge>
                <Badge className="text-[#7209b7] font-bold w-fit" variant="ghost">
                    {job?.salary} LPA
                </Badge>
            </div>
        </div>
    )
}

export default LatestJobCards
