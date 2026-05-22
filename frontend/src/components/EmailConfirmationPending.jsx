import { Mail } from "lucide-react";
import { Button } from "./ui/button";
import { useNavigate, useLocation } from "react-router-dom";

const EmailConfirmationPending = () => {
    const navigate  = useNavigate();
    const location  = useLocation();
    const email     = location.state?.email || "your email";

    return (
        <div className="flex items-center justify-center min-h-screen bg-gray-50">
            <div className="max-w-md w-full bg-white rounded-xl shadow-md p-8 text-center">

                {/* Icon */}
                <div className="flex justify-center mb-6">
                    <div className="bg-blue-100 p-4 rounded-full">
                        <Mail className="h-12 w-12 text-blue-600" />
                    </div>
                </div>

                {/* Title */}
                <h1 className="text-2xl font-bold text-gray-900 mb-2">
                    Check your email
                </h1>

                {/* Message */}
                <p className="text-gray-500 mb-2">
                    We sent a confirmation link to
                </p>
                <p className="font-semibold text-gray-800 mb-6">
                    {email}
                </p>

                <p className="text-sm text-gray-500 mb-8">
                    Click the link in the email to confirm your account.
                    Once confirmed you can login.
                </p>

                {/* Divider */}
                <div className="border-t border-gray-100 pt-6">
                    <p className="text-sm text-gray-500 mb-3">
                        Already confirmed?
                    </p>
                    <Button
                        onClick={() => navigate("/login")}
                        className="w-full">
                        Go to Login
                    </Button>
                </div>

                {/* Wrong email */}
                <p className="text-xs text-gray-400 mt-4">
                    Wrong email?{" "}
                    <span
                        onClick={() => navigate("/signup")}
                        className="text-blue-500 cursor-pointer hover:underline">
                        Register again
                    </span>
                </p>
            </div>
        </div>
    );
};

export default EmailConfirmationPending;