import { XCircle } from "lucide-react";
import { Button } from "../ui/button";
import { useNavigate } from "react-router-dom";

const ConfirmEmailFailed = () => {
    const navigate = useNavigate();

    return (
        <div className="flex items-center justify-center min-h-screen bg-gray-50">
            <div className="max-w-md w-full bg-white rounded-xl shadow-md p-8 text-center">

                {/* Icon */}
                <div className="flex justify-center mb-6">
                    <div className="bg-red-100 p-4 rounded-full">
                        <XCircle className="h-12 w-12 text-red-500" />
                    </div>
                </div>

                <h1 className="text-2xl font-bold text-gray-900 mb-2">
                    Confirmation Failed
                </h1>

                <p className="text-gray-500 mb-8">
                    The confirmation link is invalid or has already been used.
                    Please register again to get a new link.
                </p>

                <div className="flex flex-col gap-3">
                    <Button
                        onClick={() => navigate("/signup")}
                        className="w-full">
                        Register Again
                    </Button>
                    <Button
                        variant="outline"
                        onClick={() => navigate("/login")}
                        className="w-full">
                        Back to Login
                    </Button>
                </div>
            </div>
        </div>
    );
};

export default ConfirmEmailFailed;