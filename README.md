# Job Portal — Full Stack Web Application

A full-stack job portal built with **ASP.NET Core** (.NET 10) on the backend and **React + Vite** on the frontend. The backend follows Clean Architecture with CQRS pattern, and the frontend connects to it via a RESTful API with JWT-based authentication.

## What This Project Does
Students can browse jobs, apply with a cover letter, save jobs for later, and track application statuses. Recruiters can post jobs under their companies, view applicants, and accept or reject them. An admin panel gives full visibility into users, companies, and jobs.


## Tech Stack

**Backend**
- ASP.NET Core (.NET 10)
- Entity Framework Core + SQL Server
- MediatR (CQRS pattern)
- AutoMapper
- FluentValidation
- JWT Bearer Authentication
- Cloudinary / Local file storage
- Swashbuckle (Swagger)
- BCrypt.Net for password hashing

**Frontend**
- React 18 + Vite
- Redux Toolkit
- React Router v6
- Axios
- Tailwind CSS + shadcn/ui
- Sonner (toasts)

---

## Project Structure
Job_portal/
├── Domain/                  → Entities, Enums (no dependencies)
├── Application/             → Business logic, CQRS handlers, DTOs, Interfaces
│   ├── Common/
│   │   ├── Behaviors/       → ValidationBehavior (MediatR pipeline)
│   │   ├── Interfaces/      → IFileService, IJwtService, IRepositories
│   │   ├── Mappings/        → AutoMapper profiles
│   │   ├── Models/          → PaginatedList, JobFilter
│   │   └── Settings/        → JwtSettings, AdminSettings, CloudinarySettings
│   ├── DTOs/                → Response shapes for frontend
│   └── Features/            → Commands and Queries per feature
│       ├── Auth/
│       ├── Companies/
│       ├── Jobs/
│       ├── JobApplications/
│       ├── SavedJobs/
│       └── Admin/
├── Infrastructure/          → EF Core, JWT, File services, Repositories
│   ├── Persistence/
│   │   └── Repositories/
│   └── Services/            → JwtService, LocalFileService, CloudinaryFileService
└── API/                     → Controllers, Middleware, Program.cs
    ├── Controllers/
    └── Middleware/          → ExceptionHandlingMiddleware

# API Endpoints

## Auth — `/api/v1/user`
| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| POST | `/register` | ❌ | Register student or recruiter |
| POST | `/login` | ❌ | Login and receive JWT cookie |
| GET | `/logout` | ❌ | Clear JWT cookie |
| PUT | `/profile/update` | ✅ Any | Update profile, photo, resume |
| GET | `/getAllUser` | 🔐 Admin | All users |
| GET | `/getAllRecruiter` | 🔐 Admin | All recruiters |
| GET | `/getAllJobSeeker` | 🔐 Admin | All students |
| DELETE | `/removeUser/{id}` | 🔐 Admin | Hard delete user |

## Companies — `/api/v1/company`
| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| GET | `/get` | 🏢 Recruiter | Get own companies |
| GET | `/get/{id}` | ✅ Any | Single company detail |
| POST | `/register` | 🏢 Recruiter | Register new company |
| PUT | `/update/{id}` | 🏢 Recruiter | Update company info + logo |
| PUT | `/deleteCompany/{id}` | 🏢 Recruiter | Soft delete company |
| GET | `/getAllCompany` | 🔐 Admin | All companies |

## Jobs — `/api/v1/job`
| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| GET | `/get` | ❌ | Browse jobs with filters + pagination |
| GET | `/get/{id}` | ❌ | Single job detail |
| GET | `/getadminjobs` | 🏢 Recruiter | Recruiter's posted jobs |
| POST | `/post` | 🏢 Recruiter | Post new job |
| PUT | `/job/{id}` | 🏢 Recruiter | Update job |
| DELETE | `/deleteJob/{id}` | 🏢 Recruiter | Soft delete job |
| GET | `/saved` | 👤 Student | Saved jobs list |
| POST | `/save/{jobId}` | 👤 Student | Save a job |
| DELETE | `/unsave/{jobId}` | 👤 Student | Remove saved job |

## Applications — `/api/v1/application`
| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| GET | `/get` | 👤 Student | Student's applied jobs |
| POST | `/apply/{jobId}` | 👤 Student | Apply for a job |
| GET | `/{jobId}/applicants` | 🏢 Recruiter | View job applicants |
| POST | `/status/{id}/update` | 🏢 Recruiter | Accept or reject applicant |

## Admin — `/api/v1/admin`
| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| POST | `/login` | ❌ | Admin login |
| GET | `/{jobId}/applicants` | 🔐 Admin | View any job's applicants |

---

API runs at `https://localhost:44331`
Swagger UI at `https://localhost:44331/swagger`

