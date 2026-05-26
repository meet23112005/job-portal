import { createBrowserRouter, RouterProvider } from 'react-router-dom'
import Login from './components/auth/Login'
import Signup from './components/auth/Signup'
import Home from './components/Home'
import Jobs from './components/Jobs'
import Browse from './components/Browse'
import Profile from './components/Profile'
import JobDescription from './components/JobDescription'
import Companies from './components/admin/Companies'
import CompanyCreate from './components/admin/CompanyCreate'
import CompanySetup from './components/admin/CompanySetup'
import AdminJobs from "./components/admin/AdminJobs";
import PostJob from './components/admin/PostJob'
import Applicants from './components/admin/Applicants'
import ProtectedRoute from './components/admin/ProtectedRoute'
import Dashboard from './components/main-Admin/dashboard'
import AdminLogin from './components/main-Admin/AdminLogin'
import EditJob from './components/admin/EditJob'
import AdminProtectedRoute from './components/main-Admin/AdminProtectedRoute'
import EmailConfirmationPending from './components/EmailConfirmationPending'
import ConfirmEmailFailed from './components/auth/ConfirmEmailFailed'
import ForgotPassword from './components/auth/ForgotPassword'
import ResetPassword from './components/auth/ResetPassword'
import axios from 'axios'
import store from './redux/store'
import { logout } from './redux/authSlice'
import RootRedirect from './components/RootRedirect'

const appRouter = createBrowserRouter([  
  {
    path: "/",
    element: <RootRedirect />  // ← smart redirect based on role ✅
  },
  {
    path: '/home',
    element:<Home />
  },
  {
    path: '/login',
    element: <Login />
  },
  {
    path: '/signup',
    element: <Signup />
  },
  {
    path: "/jobs",
    element: <Jobs />
  },
  {
    path: "/description/:id",
    element: <JobDescription />
  },
  {
    path: "/browse",
    element: <Browse />
  },
  {
    path: "/profile",
    element: <Profile />
  },
  {
    path: "/recruiter/profile",
    element: <Profile />
  },
  // admin routes start from here
  {
    path: "/recruiter/companies",
    element: <ProtectedRoute><Companies /></ProtectedRoute>
  },
  {
    path: "/recruiter/companies/create",
    element: <ProtectedRoute><CompanyCreate /></ProtectedRoute>
  },
  {
    path: "/recruiter/companies/:id",
    element: <ProtectedRoute><CompanySetup /></ProtectedRoute>
  },
  {
    path: "/recruiter/jobs",
    element: <ProtectedRoute><AdminJobs /></ProtectedRoute>
  },
  {
    path: "/recruiter/jobs/create",
    element: <ProtectedRoute><PostJob /></ProtectedRoute>
  },
  {
    path: "/recruiter/jobs/edit/:id",
    element: <ProtectedRoute><EditJob /></ProtectedRoute>
  },
  {
    path: "/recruiter/jobs/:id/applicants",
    element: <ProtectedRoute><Applicants /></ProtectedRoute>
  },
  {
    path: "/admin/login",
    element: <AdminLogin />
  },
  {
    path: "/admin/dashboard",
    element: <AdminProtectedRoute><Dashboard /></AdminProtectedRoute>
  },
  {
    path: "/email-confirmation-pending",
    element: <EmailConfirmationPending />
  },
  {
    path: "/confirm-email-failed",
    element: <ConfirmEmailFailed />
  },
  // Router file
  {
    path: "/forgot-password",
    element: <ForgotPassword />
  },
  {
    path: "/reset-password",
    element: <ResetPassword />
  },

])

axios.defaults.withCredentials = true

// Add request interceptor
axios.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("token");
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

//Response Interceptor — handle 401
axios.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error?.response?.status === 401) {
      //token invalid or expired

      //clear redux state and remove token and user from localstorage in logout reducer
      store.dispatch(logout());

      window.location.href = "/login";
    }
    return Promise.reject(error);
  }
);

function App() {

  return (
    <div>
      <RouterProvider router={appRouter} />
    </div>
  )
}

export default App
