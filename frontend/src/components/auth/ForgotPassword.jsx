import { useState } from "react";
import { Link } from "react-router-dom";
import axios from "axios";
import { toast } from "sonner";
import { USER_API_END_POINT } from "@/utils/constant";
import { Button } from "../ui/button";
import { Input } from "../ui/input";
import { Label } from "../ui/label";
import { Loader2, Mail } from "lucide-react";
import Navbar from "../shared/Navbar";

const ForgotPassword = () => {
    const [email,   setEmail]   = useState("");
    const [loading, setLoading] = useState(false);
    const [sent,    setSent]    = useState(false);

    const submitHandler = async (e) => {
        e.preventDefault();
        setLoading(true);

        try {
            const res = await axios.post(
                `${USER_API_END_POINT}/forgot-password`,
                { email },
                { withCredentials: true }
            );

            if (res.data.success) {
                setSent(true); // ← show "check email" UI
            }
        } catch (error) {
            toast.error(
                error?.response?.data?.message
                || "Something went wrong."
            );
        } finally {
            setLoading(false);
        }
    };

    // ── After email sent ───────────────────────────────
    if (sent) {
        return (
            <div>
                <Navbar />
                <div className="flex items-center justify-center min-h-[80vh]">
                    <div className="max-w-md w-full bg-white rounded-xl shadow-md p-8 text-center mx-4">
                        <div className="flex justify-center mb-4">
                            <div className="bg-blue-100 p-4 rounded-full">
                                <Mail className="h-10 w-10 text-blue-600" />
                            </div>
                        </div>
                        <h2 className="text-xl font-bold text-gray-900 mb-2">
                            Check your inbox
                        </h2>
                        <p className="text-gray-500 mb-2">
                            We sent a password reset link to
                        </p>
                        <p className="font-semibold text-gray-800 mb-6">
                            {email}
                        </p>
                        <p className="text-sm text-gray-400 mb-6">
                            The link expires in 1 hour.
                            Check your spam folder if you don't see it.
                        </p>
                        <Link
                            to="/login"
                            className="text-blue-600 text-sm hover:underline">
                            Back to Login
                        </Link>
                    </div>
                </div>
            </div>
        );
    }

    // ── Enter email form ───────────────────────────────
    return (
        <div>
            <Navbar />
            <div className="flex items-center justify-center min-h-[80vh]">
                <div className="max-w-md w-full border border-gray-200 rounded-xl p-8 mx-4">
                    <h1 className="text-2xl font-bold text-gray-900 mb-2">
                        Forgot Password
                    </h1>
                    <p className="text-gray-500 text-sm mb-6">
                        Enter your email and we'll send you a reset link.
                    </p>

                    <form onSubmit={submitHandler}>
                        <div className="mb-4">
                            <Label>Email address</Label>
                            <Input
                                type="email"
                                value={email}
                                onChange={e => setEmail(e.target.value)}
                                placeholder="john@gmail.com"
                                required
                                className="mt-1"
                            />
                        </div>

                        {loading ? (
                            <Button className="w-full" disabled>
                                <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                                Sending...
                            </Button>
                        ) : (
                            <Button type="submit" className="w-full">
                                Send Reset Link
                            </Button>
                        )}
                    </form>

                    <p className="text-center text-sm text-gray-500 mt-4">
                        Remember your password?{" "}
                        <Link to="/login" className="text-blue-600 hover:underline">
                            Login
                        </Link>
                    </p>
                </div>
            </div>
        </div>
    );
};

export default ForgotPassword;