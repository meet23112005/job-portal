import { useState, useEffect } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import axios from "axios";
import { toast } from "sonner";
import { USER_API_END_POINT } from "@/utils/constant";
import { Button } from "../ui/button";
import { Input } from "../ui/input";
import { Label } from "../ui/label";
import { Loader2, Eye, EyeOff } from "lucide-react";
import Navbar from "../shared/Navbar";

const ResetPassword = () => {
    const [searchParams]              = useSearchParams();
    const token                       = searchParams.get("token");
    const navigate                    = useNavigate();

    const [newPassword,     setNewPassword]     = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [loading,         setLoading]         = useState(false);
    const [showNew,         setShowNew]         = useState(false);
    const [showConfirm,     setShowConfirm]     = useState(false);

    // redirect if no token in URL
    useEffect(() => {
        if (!token) {
            toast.error("Invalid reset link.");
            navigate("/login");
        }
    }, [token, navigate]);

    const submitHandler = async (e) => {
        e.preventDefault();

        // frontend validation
        if (newPassword !== confirmPassword) {
            toast.error("Passwords do not match.");
            return;
        }
        if (newPassword.length < 8) {
            toast.error("Password must be at least 8 characters.");
            return;
        }

        setLoading(true);

        try {
            const res = await axios.post(
                `${USER_API_END_POINT}/reset-password`,
                {
                    token,
                    newPassword,
                    confirmPassword
                },
                { withCredentials: true }
            );

            if (res.data.success) {
                toast.success("Password reset successfully!");
                navigate("/login");
            }
        } catch (error) {
            toast.error(
                error?.response?.data?.message
                || "Reset link is invalid or expired."
            );
        } finally {
            setLoading(false);
        }
    };

    return (
        <div>
            <Navbar />
            <div className="flex items-center justify-center min-h-[80vh]">
                <div className="max-w-md w-full border border-gray-200 rounded-xl p-8 mx-4">
                    <h1 className="text-2xl font-bold text-gray-900 mb-2">
                        Reset Password
                    </h1>
                    <p className="text-gray-500 text-sm mb-6">
                        Enter your new password below.
                    </p>

                    <form onSubmit={submitHandler} className="space-y-4">

                        {/* New Password */}
                        <div>
                            <Label>New Password</Label>
                            <div className="relative mt-1">
                                <Input
                                    type={showNew ? "text" : "password"}
                                    value={newPassword}
                                    onChange={e => setNewPassword(e.target.value)}
                                    placeholder="Min 8 characters"
                                    required
                                />
                                <button
                                    type="button"
                                    onClick={() => setShowNew(!showNew)}
                                    className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400">
                                    {showNew
                                        ? <EyeOff className="h-4 w-4" />
                                        : <Eye    className="h-4 w-4" />}
                                </button>
                            </div>
                        </div>

                        {/* Confirm Password */}
                        <div>
                            <Label>Confirm Password</Label>
                            <div className="relative mt-1">
                                <Input
                                    type={showConfirm ? "text" : "password"}
                                    value={confirmPassword}
                                    onChange={e => setConfirmPassword(e.target.value)}
                                    placeholder="Repeat password"
                                    required
                                />
                                <button
                                    type="button"
                                    onClick={() => setShowConfirm(!showConfirm)}
                                    className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400">
                                    {showConfirm
                                        ? <EyeOff className="h-4 w-4" />
                                        : <Eye    className="h-4 w-4" />}
                                </button>
                            </div>
                        </div>

                        {/* Password match indicator */}
                        {confirmPassword && (
                            <p className={`text-xs ${
                                newPassword === confirmPassword
                                    ? "text-green-500"
                                    : "text-red-500"
                            }`}>
                                {newPassword === confirmPassword
                                    ? "✓ Passwords match"
                                    : "✗ Passwords do not match"}
                            </p>
                        )}

                        {loading ? (
                            <Button className="w-full" disabled>
                                <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                                Resetting...
                            </Button>
                        ) : (
                            <Button type="submit" className="w-full">
                                Reset Password
                            </Button>
                        )}
                    </form>
                </div>
            </div>
        </div>
    );
};

export default ResetPassword;