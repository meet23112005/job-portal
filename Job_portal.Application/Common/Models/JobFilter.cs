using System;
using System.Collections.Generic;
using System.Text;

namespace Job_portal.Application.Common.Models
{
    public class JobFilter
    {
        public string? Keyword { get; set; }  // search by title
        public string? Location { get; set; }  // filter by city
        public string? JobType { get; set; }  // remote / on-site
        public string? ExperienceLevel { get; set; }  // fresher / senior
        public decimal? MinSalary { get; set; }  // minimum salary
        public decimal? MaxSalary { get; set; }  // maximum salary
        public Guid? CompanyId { get; set; }  // filter by company
    }
}
