using PMS.APP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMS.APP.Helper
{
    public static class Helpers
    {

        #region //=> Methods

        public static List<ProjectMilestones> ProjectMilestonesList(List<Projects> _projectsList, string userType = "")
        {
            List<ProjectMilestones> _projectMilestonesList = new List<ProjectMilestones>();
            try
            {
                foreach (var project in _projectsList)
                {
                    var _projectMilestones = new ProjectMilestones();
                    _projectMilestones.ProjectId = project.ProjectId;
                    _projectMilestones.Name = project.Name ?? "";
                    _projectMilestones.Description = project.Description ?? "";
                    _projectMilestones.Client = project.Client ?? "";
                    _projectMilestones.EstimatedCost = project.EstimatedCost;
                    _projectMilestones.PaymentReceived = project.PaymentReceived;
                    _projectMilestones.DatePaymentReceived = project.DatePaymentReceived;
                    _projectMilestones.DateAssigned = project.DateAssigned;
                    _projectMilestones.DateCompleted = project.DateCompleted;
                    _projectMilestones.IsActive = project.IsActive;
                    _projectMilestones.TechnologyStack = project.TechnologyStack ?? "";
                    _projectMilestones.ProjectType = project.ProjectType ?? "";
                    _projectMilestones.IsCompleted = project.IsCompleted;
                    _projectMilestones.IsDeleted = project.IsDeleted;
                    _projectMilestones.Status = project.Status ?? "";
                    _projectMilestones.Notes = project.Notes ?? "";

                    //total
                    _projectMilestones.TotalMilestones = project.Milestones.Count;
                    if (userType.ToLower() == "director")
                        _projectMilestones.TotalMilestoneCost = project.Milestones.Sum(s => s.Amount);
                    else
                        _projectMilestones.TotalMilestoneCost = (project.Milestones.Sum(s => s.Amount) > project.EstimatedCost ? project.Milestones.Sum(s => s.Amount) : project.EstimatedCost);
                    //  _projectMilestones.TotalMilestoneCost = (project.Milestones.Count>0?project.Milestones.Sum(s => s.Amount): project.EstimatedCost);
                    /*Manjeet*/
                    // _projectMilestones.TotalMilestoneCost = project.EstimatedCost;
                    _projectMilestones.TotalMilestoneCostReceived = project.Milestones.Where(w => w.PaymentReceived == true).Sum(s => s.RecievedAmount);

                    _projectMilestones.Milestones = project.Milestones;

                    _projectMilestonesList.Add(_projectMilestones);
                }
            }
            catch (Exception ex)
            {
            }
            return _projectMilestonesList;
        }

        public static List<Tech> ProjectsByTech(List<Projects> _projectsList, string _techBy)
        {
            List<Tech> _techList = new List<Tech>();
            try
            {
                Tech tech = new Tech();
                var techBy = _techBy == null ? "" : _techBy.Trim().ToLower();

                if (!string.IsNullOrEmpty(techBy))
                {
                    _projectsList = _projectsList.Where(w => w.TechnologyStack.Trim().ToLower() == techBy).ToList();
                    tech.tech = techBy;
                    tech.TotalEstimatedCost = _projectsList.Sum(s => s.EstimatedCost);
                    tech.TotalReceived = _projectsList.Where(w => w.PaymentReceived == true).Sum(s => s.EstimatedCost);
                    tech.projects = _projectsList;
                    _techList.Add(tech);
                }
                else
                {
                    foreach (var _tech in _projectsList.Select(x => x.TechnologyStack.Trim()).Distinct())
                    {
                        var _projects = _projectsList.Where(w => w.TechnologyStack.Trim().ToLower() == _tech.ToLower()).ToList();
                        //=>add detail
                        tech = new Tech();
                        tech.tech = _tech.Trim().ToLower();
                        tech.TotalEstimatedCost = _projects.Sum(s => s.EstimatedCost);
                        tech.TotalReceived = _projects.Where(w => w.PaymentReceived == true).Sum(s => s.EstimatedCost);
                        tech.projects = _projects;
                        _techList.Add(tech);
                    }
                }

            }
            catch (Exception ex)
            {
            }
            return _techList;
        }

        public static List<ProjectByTech> ProjectMilestonesByTech(List<Projects> _projectsList, string _techBy, string userType = "")
        {
            List<ProjectByTech> _techList = new List<ProjectByTech>();
            try
            {
                ProjectByTech tech = new ProjectByTech();
                var techBy = _techBy == null ? "" : _techBy.Trim().ToLower();

                if (!string.IsNullOrEmpty(techBy))
                {
                    _projectsList = _projectsList.Where(w => w.TechnologyStack.Trim().ToLower() == techBy).ToList();
                    var _projectMilestonesList = ProjectMilestonesList(_projectsList, userType);

                    tech.tech = techBy;
                    tech.TotalEstimatedCost = _projectMilestonesList.Sum(s => s.TotalMilestoneCost);  //_projectsList.Sum(s => s.EstimatedCost);
                    tech.TotalReceived = _projectMilestonesList.Sum(s => s.TotalMilestoneCostReceived);//_projectsList.Where(w => w.PaymentReceived == true).Sum(s => s.EstimatedCost);
                    tech.projects = _projectMilestonesList; // _projectsList;
                    _techList.Add(tech);
                }
                else
                {
                    foreach (var _tech in _projectsList.Select(x => x.TechnologyStack.Trim()).Distinct())
                    {
                        var _projects = _projectsList.Where(w => w.TechnologyStack.Trim().ToLower() == _tech.Trim().ToLower()).ToList();
                        //=>add detail
                        //tech.tech = _tech.Trim().ToLower();
                        //tech.TotalEstimatedCost = _projects.Sum(s => s.EstimatedCost);
                        //tech.TotalReceived = _projects.Where(w => w.PaymentReceived == true).Sum(s => s.EstimatedCost);
                        //tech.projects = ProjectMilestonesList(_projects); //_projects;
                        //_techList.Add(tech);
                        var _projectMilestonesList = ProjectMilestonesList(_projects, userType);

                        tech = new ProjectByTech();
                        tech.tech = _tech.Trim().ToLower();
                        tech.TotalEstimatedCost = _projectMilestonesList.Sum(s => s.TotalMilestoneCost);  //_projectsList.Sum(s => s.EstimatedCost);
                        tech.TotalReceived = _projectMilestonesList.Sum(s => s.TotalMilestoneCostReceived);//_projectsList.Where(w => w.PaymentReceived == true).Sum(s => s.EstimatedCost);
                        tech.projects = _projectMilestonesList; // _projectsList;
                        _techList.Add(tech);
                    }
                }

            }
            catch (Exception ex)
            {
            }
            return _techList;
        }

        public static List<ProjectByDeptt> ProjectMilestonesByDeptt(List<Projects> _projectsList, string _techBy, string userType = "")
        {
            List<ProjectByDeptt> _techDepttList = new List<ProjectByDeptt>();
            try
            {
                var techBy = _techBy == null ? "" : _techBy.Trim().ToLower();
                List<ProjectByTech> _techList = Helpers.ProjectMilestonesByTech(_projectsList, techBy, userType);
                List<ProjectByTech> _ProjectByTechList = new List<ProjectByTech>();
                bool isMobile = false;
                decimal TotalProjectsCost = 0; decimal TotalReceivedCost = 0;
                var _techDeptt = new ProjectByDeptt();

                foreach (var tech in _techList.Select(x => x.tech.Trim()).Distinct())
                {
                    switch (tech.ToLower())
                    {
                        case "android":
                        case "ios":
                        case "react native":
                        case "ionic":
                            isMobile = true;
                            TotalProjectsCost += _techList.Where(w => w.tech.ToLower() == tech.ToLower()).Sum(s => s.TotalEstimatedCost);
                            TotalReceivedCost += _techList.Where(w => w.tech.ToLower() == tech.ToLower()).Sum(s => s.TotalReceived);
                            _ProjectByTechList.AddRange(_techList.Where(w => w.tech.ToLower() == tech.ToLower()));
                            break;
                        default:
                            _techDeptt = new ProjectByDeptt();
                            _techDeptt.deptt = tech;
                            _techDeptt.totalProjectsCost = _techList.Where(w => w.tech.ToLower() == tech.ToLower()).Sum(s => s.TotalEstimatedCost);
                            _techDeptt.totalReceivedCost = _techList.Where(w => w.tech.ToLower() == tech.ToLower()).Sum(s => s.TotalReceived);
                            _techDeptt.data = _techList.Where(w => w.tech.ToLower() == tech.ToLower()).ToList();
                            _techDepttList.Add(_techDeptt);
                            break;
                    }
                }

                if (isMobile)
                {
                    _techDeptt = new ProjectByDeptt();
                    _techDeptt.deptt = "mobile";
                    _techDeptt.totalProjectsCost = TotalProjectsCost;
                    _techDeptt.totalReceivedCost = TotalReceivedCost;
                    _techDeptt.data = _ProjectByTechList;
                    _techDepttList.Add(_techDeptt);
                }
            }
            catch (Exception ex)
            {
            }
            return _techDepttList;
        }

        public static List<ProjectByMonth> ProjectMilestonesByMonth(List<Projects> _projectsList, int _maxMonth, int _month, int _year, string _techBy)
        {
            List<ProjectByMonth> _projectByMonth = new List<ProjectByMonth>();
            try
            {
                var maxMonth = _month + _maxMonth;
                var techBy = _techBy == null ? "" : _techBy.Trim().ToLower();

                for (int mnth = _month; mnth < maxMonth; mnth++)
                {
                    DateTime dtDate = new DateTime(DateTime.Now.Year, mnth, 1);
                    string shortMonthName = dtDate.ToString("MMM");
                    string fullMonthName = dtDate.ToString("MMMM");

                    var _projectsListByMonth = GetProjectsByMonthYear(_projectsList, mnth, _year);
                    List<ProjectByTech> _techList = Helpers.ProjectMilestonesByTech(_projectsListByMonth, techBy);

                    var projectMonth = new ProjectByMonth();
                    projectMonth.month = fullMonthName.ToLower();
                    projectMonth.totalProjectsCost = _techList.Sum(s => s.TotalEstimatedCost);
                    projectMonth.totalReceivedCost = _techList.Sum(s => s.TotalReceived);
                    projectMonth.data = _techList;
                    _projectByMonth.Add(projectMonth);
                }

            }
            catch (Exception ex)
            {
            }
            return _projectByMonth;
        }

        private static List<Projects> GetProjectsByMonthYear(List<Projects> _projectsList, int _monthBy, int _yearBy)
        {

            List<Projects> _projectList = new List<Projects>();
            try
            {
                var monthBy = _monthBy; var yearBy = _yearBy;
                //   _projectsList = _projectsList.Where(o =>  o.Milestones.Any(m=>m.DueDate.Month == monthBy && m.DueDate.Year == yearBy)).ToList();
                foreach (var project in _projectsList)
                {
                    Projects _project = new Projects();
                    _project.Milestones = project.Milestones.Where(m => m.DueDate.Month == monthBy && m.DueDate.Year == yearBy).ToList();
                    _project.EstimatedCost = _project.Milestones.Sum(m => m.Amount);
                    _projectList.Add(_project);
                }

            }
            catch (Exception ex)
            {
            }
            return _projectList;
        }

        public static List<CostByMonthList> ProjectCostByMonth(List<Projects> _projectsList, int _maxMonth, int _month, int _year, string _techBy)
        {
            List<CostByMonthList> __CostByMonthList = new List<CostByMonthList>();
            try
            {
                var maxMonth = _month + _maxMonth;
                var techBy = _techBy == null ? "" : _techBy.Trim().ToLower();

                _projectsList.Where(d=>d.TechnologyStack== "Android" || d.TechnologyStack == "Ionic" || d.TechnologyStack == "Ios" || d.TechnologyStack == "React Native").ToList().ForEach(c => c.TechnologyStack = "Mobile");
              

                foreach (var _tech in _projectsList.Select(x => x.TechnologyStack.Trim()).Distinct())
                {
                    List<CostByMonth> _costByMonthList = new List<CostByMonth>();
                    for (int mnth = _month; mnth < maxMonth; mnth++)
                    {
                        if (mnth > 12)
                            continue;
                        DateTime dtDate = new DateTime(DateTime.Now.Year, mnth, 1);
                        string shortMonthName = dtDate.ToString("MMM");
                        string fullMonthName = dtDate.ToString("MMMM");

                        var _projectsListByMonth = GetProjectsByMonthYear(_projectsList.Where(w => w.TechnologyStack.ToLower() == _tech.ToLower()).ToList(), mnth, _year);
                        CostByMonth costByMonth = new CostByMonth();
                        costByMonth.month = fullMonthName.ToLower();
                        costByMonth.year = _year;
                        costByMonth.projectCount = _projectsListByMonth.Count;
                        costByMonth.EstimatedCost = _projectsListByMonth.Sum(s => s.EstimatedCost);
                        _costByMonthList.Add(costByMonth);
                    }

                    var CostByMonthList = new CostByMonthList();
                    CostByMonthList.tech = _tech.ToLower();
                    CostByMonthList.data = _costByMonthList;
                    __CostByMonthList.Add(CostByMonthList);
                }

            }
            catch (Exception ex)
            {
            }
            return __CostByMonthList;
        }



        #endregion

        #region //=> Sort By Methods

        public static List<Milestones> SortMilestones(List<Milestones> _milestonesList, string _sort, string _by)
        {
            try
            {
                var sort = _sort == null ? "" : _sort.ToLower();
                var by = _by == null ? "" : _by.ToLower();
                switch (sort)
                {
                    case "milestoneid":
                        if (by == "asc")
                            _milestonesList = _milestonesList.OrderBy(o => o.MilestoneId).ToList();
                        else
                            _milestonesList = _milestonesList.OrderByDescending(o => o.MilestoneId).ToList();
                        break;
                    case "name":
                        if (by == "asc")
                            _milestonesList = _milestonesList.OrderBy(o => o.Name).ToList();
                        else
                            _milestonesList = _milestonesList.OrderByDescending(o => o.Name).ToList();
                        break;
                    case "amount":
                        if (by == "asc")
                            _milestonesList = _milestonesList.OrderBy(o => o.Amount).ToList();
                        else
                            _milestonesList = _milestonesList.OrderByDescending(o => o.Amount).ToList();
                        break;
                    case "recievedamount":
                        if (by == "asc")
                            _milestonesList = _milestonesList.OrderBy(o => o.RecievedAmount).ToList();
                        else
                            _milestonesList = _milestonesList.OrderByDescending(o => o.RecievedAmount).ToList();
                        break;
                    case "startdate":
                        if (by == "asc")
                            _milestonesList = _milestonesList.OrderBy(o => o.StartDate).ToList();
                        else
                            _milestonesList = _milestonesList.OrderByDescending(o => o.StartDate).ToList();
                        break;
                    case "duedate":
                        if (by == "asc")
                            _milestonesList = _milestonesList.OrderBy(o => o.DueDate).ToList();
                        else
                            _milestonesList = _milestonesList.OrderByDescending(o => o.DueDate).ToList();
                        break;
                    case "enddate":
                        if (by == "asc")
                            _milestonesList = _milestonesList.OrderBy(o => o.EndDate).ToList();
                        else
                            _milestonesList = _milestonesList.OrderByDescending(o => o.EndDate).ToList();
                        break;
                    case "iscompleted":
                        if (by == "asc")
                            _milestonesList = _milestonesList.OrderBy(o => o.IsCompleted).ToList();
                        else
                            _milestonesList = _milestonesList.OrderByDescending(o => o.IsCompleted).ToList();
                        break;
                    case "paymentreceived":
                        if (by == "asc")
                            _milestonesList = _milestonesList.OrderBy(o => o.PaymentReceived).ToList();
                        else
                            _milestonesList = _milestonesList.OrderByDescending(o => o.PaymentReceived).ToList();
                        break;
                    case "projectid":
                        if (by == "asc")
                            _milestonesList = _milestonesList.OrderBy(o => o.ProjectId).ToList();
                        else
                            _milestonesList = _milestonesList.OrderByDescending(o => o.ProjectId).ToList();
                        break;
                    case "date":
                        if (by == "asc")
                            _milestonesList = _milestonesList.OrderBy(o => o.StartDate).ToList();
                        else
                            _milestonesList = _milestonesList.OrderByDescending(o => o.StartDate).ToList();
                        break;

                    default: break;
                }
            }
            catch (Exception ex)
            {
            }
            return _milestonesList;
        }

        public static List<Projects> SortProjects(List<Projects> _projectsList, string _sort, string _by)
        {
            try
            {

                var sort = _sort == null ? "" : _sort.ToLower();
                var by = _by == null ? "" : _by.ToLower();
                switch (sort)
                {
                    case "projectid":
                        if (by == "asc")
                            _projectsList = _projectsList.OrderBy(o => o.ProjectId).ToList();
                        else
                            _projectsList = _projectsList.OrderByDescending(o => o.ProjectId).ToList();
                        break;
                    case "name":
                        if (by == "asc")
                            _projectsList = _projectsList.OrderBy(o => o.Name).ToList();
                        else
                            _projectsList = _projectsList.OrderByDescending(o => o.Name).ToList();
                        break;
                    case "description":
                        if (by == "asc")
                            _projectsList = _projectsList.OrderBy(o => o.Description).ToList();
                        else
                            _projectsList = _projectsList.OrderByDescending(o => o.Description).ToList();
                        break;
                    case "client":
                        if (by == "asc")
                            _projectsList = _projectsList.OrderBy(o => o.Client).ToList();
                        else
                            _projectsList = _projectsList.OrderByDescending(o => o.Client).ToList();
                        break;
                    case "estimatedCost":
                        if (by == "asc")
                            _projectsList = _projectsList.OrderBy(o => o.EstimatedCost).ToList();
                        else
                            _projectsList = _projectsList.OrderByDescending(o => o.EstimatedCost).ToList();
                        break;
                    case "dateassigned":
                        if (by == "asc")
                            _projectsList = _projectsList.OrderBy(o => o.DateAssigned).ToList();
                        else
                            _projectsList = _projectsList.OrderByDescending(o => o.DateAssigned).ToList();
                        break;
                    case "dateCompleted":
                        if (by == "asc")
                            _projectsList = _projectsList.OrderBy(o => o.DateCompleted).ToList();
                        else
                            _projectsList = _projectsList.OrderByDescending(o => o.DateCompleted).ToList();
                        break;
                    case "iscompleted":
                        if (by == "asc")
                            _projectsList = _projectsList.OrderBy(o => o.IsCompleted).ToList();
                        else
                            _projectsList = _projectsList.OrderByDescending(o => o.IsCompleted).ToList();
                        break;
                    case "paymentreceived":
                        if (by == "asc")
                            _projectsList = _projectsList.OrderBy(o => o.PaymentReceived).ToList();
                        else
                            _projectsList = _projectsList.OrderByDescending(o => o.PaymentReceived).ToList();
                        break;
                    case "isactive":
                        if (by == "asc")
                            _projectsList = _projectsList.OrderBy(o => o.IsActive).ToList();
                        else
                            _projectsList = _projectsList.OrderByDescending(o => o.IsActive).ToList();
                        break;
                    case "technologystack":
                        if (by == "asc")
                            _projectsList = _projectsList.OrderBy(o => o.TechnologyStack).ToList();
                        else
                            _projectsList = _projectsList.OrderByDescending(o => o.TechnologyStack).ToList();
                        break;
                    case "projecttype":
                        if (by == "asc")
                            _projectsList = _projectsList.OrderBy(o => o.ProjectType).ToList();
                        else
                            _projectsList = _projectsList.OrderByDescending(o => o.ProjectType).ToList();
                        break;
                    case "isdeleted":
                        if (by == "asc")
                            _projectsList = _projectsList.OrderBy(o => o.IsDeleted).ToList();
                        else
                            _projectsList = _projectsList.OrderByDescending(o => o.IsDeleted).ToList();
                        break;
                    case "date":
                        if (by == "asc")
                            _projectsList = _projectsList.OrderBy(o => o.DateAssigned).ToList();
                        else
                            _projectsList = _projectsList.OrderByDescending(o => o.DateAssigned).ToList();
                        break;

                    default: break;
                }
            }
            catch (Exception ex)
            {
            }
            return _projectsList;
        }

        public static string SortByMilestones(string _sort, string _by)
        {
            string sortBy = "";
            try
            {
                var sort = _sort == null ? "" : _sort.ToLower();
                var by = _by == null ? "" : _by.ToLower() == "asc" ? "asc" : "desc";
                sortBy = sort + " " + by;

                switch (_sort.ToLower())
                {
                    case "milestoneid": break;
                    case "name": break;
                    case "amount": break;
                    case "startdate": break;
                    case "duedate": break;
                    case "enddate": break;
                    case "iscompleted": break;
                    case "paymentreceived": break;
                    case "projectid": break;
                    case "date": break;
                    default: sort = "startDate"; break;
                }
                sortBy = sort + " " + by;
            }
            catch (Exception ex)
            {
            }
            return sortBy;
        }

        public static string SortByProjects(string _sort, string _by)
        {
            string sortBy = "";
            try
            {
                var sort = _sort == null ? "" : _sort.ToLower();
                var by = _by == null ? "" : _by.ToLower() == "asc" ? "asc" : "desc";
                sortBy = sort + " " + by;

                switch (sort)
                {
                    case "projectid": break;
                    case "name": break;
                    case "description": break;
                    case "client": break;
                    case "estimatedCost": break;
                    case "dateassigned": break;
                    case "dateCompleted": break;
                    case "iscompleted": break;
                    case "paymentreceived": break;
                    case "isactive": break;
                    case "technologystack": break;
                    case "projecttype": break;
                    case "isdeleted": break;
                    case "date": break;
                    default: sort = "dateAssigned"; break;
                }
                sortBy = sort + " " + by;
            }
            catch (Exception ex)
            {
            }
            return sortBy;
        }

        #endregion

        #region //=> Filter By Methods

        public static List<Milestones> FilterMilestones(List<Milestones> _milestonesList, string _filterBy, string _by, int _monthBy, int _yearBy = 0)
        {
            try
            {
                var filterBy = _filterBy == null ? "" : _filterBy.ToLower();
                var by = _by == null ? "" : _by.ToLower();
                var monthBy = _monthBy;
                var yearBy = _yearBy;

                switch (filterBy)
                {
                    case "month":
                        if (by == "asc")
                            _milestonesList = _milestonesList.OrderBy(o => o.DueDate.Month).ToList();
                        else
                            _milestonesList = _milestonesList.OrderByDescending(o => o.DueDate.Month).ToList();
                        break;
                    case "year":
                        if (by == "asc")
                            _milestonesList = _milestonesList.OrderBy(o => o.DueDate.Year).ToList();
                        else
                            _milestonesList = _milestonesList.OrderByDescending(o => o.DueDate.Year).ToList();
                        break;
                    default: break;
                }

                if (yearBy > 0)
                {
                    _milestonesList = _milestonesList.Where(o => o.DueDate.Year == yearBy).ToList();
                    if (monthBy > 0)
                    {
                        _milestonesList = _milestonesList.Where(o => o.DueDate.Month == monthBy).ToList();
                    }
                }
                else if (monthBy > 0 && yearBy == 0)
                {
                    _milestonesList = _milestonesList.Where(o => o.DueDate.Month == monthBy).ToList();
                }

            }
            catch (Exception ex)
            {
            }
            return _milestonesList;
        }

        public static List<Projects> FilterProjects(List<Projects> _projectsList, string _filterBy, string _by, int _monthBy, int _yearBy = 0)
        {
            try
            {
                var filterBy = _filterBy == null ? "" : _filterBy.ToLower();
                var by = _by == null ? "" : _by.ToLower();
                var monthBy = _monthBy;
                var yearBy = _yearBy;

                switch (filterBy)
                {
                    case "month":
                        if (by == "asc")
                            _projectsList = _projectsList.OrderByDescending(o => o.DateAssigned.Month).ToList();
                        else
                            _projectsList = _projectsList.OrderByDescending(o => o.DateAssigned.Month).ToList();
                        break;
                    case "year":
                        if (by == "asc")
                            _projectsList = _projectsList.OrderByDescending(o => o.DateAssigned.Year).ToList();
                        else
                            _projectsList = _projectsList.OrderByDescending(o => o.DateAssigned.Year).ToList();
                        break;
                    default: break;
                }

                if (yearBy > 0)
                {
                    _projectsList = _projectsList.Where(o => o.DateAssigned.Year == yearBy).ToList();
                    if (monthBy > 0)
                    {
                        _projectsList = _projectsList.Where(o => o.DateAssigned.Month == monthBy).ToList();
                    }
                }
                else if (monthBy > 0 && yearBy == 0)
                {
                    _projectsList = _projectsList.Where(o => o.DateAssigned.Month == monthBy).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return _projectsList;
        }

        public static List<Projects> FilterProjectsByNextMonth(List<Projects> _projectsList, string _filterBy, string _by, int _monthBy, int _maxMonth, int _yearBy = 0)
        {
            try
            {
                var filterBy = _filterBy == null ? "" : _filterBy.ToLower();
                var by = _by == null ? "" : _by.ToLower();
                var monthBy = _monthBy; var yearBy = _yearBy; var maxMonth = _monthBy + _maxMonth;

                switch (filterBy)
                {
                    case "month":
                        if (by == "asc")
                            _projectsList = _projectsList.OrderByDescending(o => o.DateAssigned.Month).ToList();
                        else
                            _projectsList = _projectsList.OrderByDescending(o => o.DateAssigned.Month).ToList();
                        break;
                    case "year":
                        if (by == "asc")
                            _projectsList = _projectsList.OrderByDescending(o => o.DateAssigned.Year).ToList();
                        else
                            _projectsList = _projectsList.OrderByDescending(o => o.DateAssigned.Year).ToList();
                        break;
                    default: break;
                }

                if (yearBy > 0)
                {
                    // _projectsList = _projectsList.Where(o => o.DateAssigned.Month == yearBy).ToList();
                    _projectsList = _projectsList.Where(o => o.Milestones.Any(m => m.DueDate.Year == yearBy)).ToList();
                    if (monthBy > 0)
                    {
                        if (_maxMonth > 0)
                        {
                            //_projectsList = _projectsList.Where(o =>  o.DateAssigned.Month >= monthBy && o.DateAssigned.Month < maxMonth).ToList();
                            _projectsList = _projectsList.Where(o => o.Milestones.Any(m => m.DueDate.Month >= monthBy && m.DueDate.Month < maxMonth)).ToList();
                        }
                        else
                        {
                            _projectsList = _projectsList.Where(o => o.Milestones.Any(m => m.DueDate.Month == monthBy)).ToList();
                        }
                    }
                }
                else if (monthBy > 0 && yearBy == 0)
                {
                    if (_maxMonth > 0)
                    {
                        // _projectsList = _projectsList.Where(o => o.DateAssigned.Month >= monthBy && o.DateAssigned.Month < maxMonth).ToList();
                        _projectsList = _projectsList.Where(o => o.Milestones.Any(m => m.DueDate.Month >= monthBy && m.DueDate.Month < maxMonth)).ToList();
                    }
                    else
                    {
                        // _projectsList = _projectsList.Where(o => o.DateAssigned.Month >= monthBy && o.DateAssigned.Month < maxMonth).ToList();
                        _projectsList = _projectsList.Where(o => o.Milestones.Any(m => m.DueDate.Month == monthBy)).ToList();
                    }
                }
                foreach (var project in _projectsList)
                {
                    if (yearBy > 0)
                    {
                        project.Milestones = project.Milestones.Where(m => m.DueDate.Year == yearBy).ToList();
                        if (monthBy > 0)
                        {
                            if (_maxMonth > 0)
                            {
                                project.Milestones = project.Milestones.Where(m => m.DueDate.Month >= monthBy && m.DueDate.Month < maxMonth).ToList();
                            }
                            else
                            {
                                project.Milestones = project.Milestones.Where(m => m.DueDate.Month == monthBy).ToList();
                            }
                        }
                    }
                    else if (monthBy > 0 && yearBy == 0)
                    {
                        if (_maxMonth > 0)
                        {
                            project.Milestones = project.Milestones.Where(m => m.DueDate.Month >= monthBy && m.DueDate.Month < maxMonth).ToList();
                        }
                        else
                        {
                            project.Milestones = project.Milestones.Where(m => m.DueDate.Month == monthBy).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return _projectsList;
        }

        public static List<Projects> FilterProjectsByTech(List<Projects> _projectsList, string _techBy, string _filterBy = "", string _searchBy = "")
        {
            try
            {
                var techBy = _techBy == null ? "" : _techBy.Trim().ToLower();

                if (!string.IsNullOrEmpty(techBy))
                {
                    if (techBy == "mobile")
                        _projectsList = _projectsList.Where(d => d.TechnologyStack == "Android" || d.TechnologyStack == "Ionic" || d.TechnologyStack == "Ios" || d.TechnologyStack == "React Native").ToList();
                    else
                        _projectsList = _projectsList.Where(w => w.TechnologyStack.ToLower() == techBy).ToList();
                }
                if (_filterBy.ToLower() == "active")
                {
                    _projectsList = _projectsList.Where(w => w.IsActive == true).ToList();
                }
                else if (_filterBy.ToLower() == "inactive")
                {
                    _projectsList = _projectsList.Where(w => w.IsActive == false).ToList();
                }
                //switch ("not implemented")
                //{
                //    case "name":
                //        _projectsList = _projectsList.Where(w => w.Name == _searchBy.ToLower()).ToList();
                //        break;
                //    case "description":
                //        _projectsList = _projectsList.Where(w => w.Description == _searchBy.ToLower()).ToList();
                //        break;
                //    case "client":
                //        _projectsList = _projectsList.Where(w => w.Client == _searchBy.ToLower()).ToList();
                //        break;
                //    case "estimatedCost":
                //        //_projectsList = _projectsList.Where(w => w.EstimatedCost == (decimal)_searchBy).ToList();
                //        break;
                //    case "dateassigned":
                //        //_projectsList = _projectsList.Where(w => w.DateAssigned == _searchBy).ToList();
                //        break;
                //    case "dateCompleted":
                //        //_projectsList = _projectsList.Where(w => w.DateCompleted == _searchBy).ToList();
                //        break;
                //    case "iscompleted":
                //        _projectsList = _projectsList.Where(w => w.IsCompleted == true).ToList();
                //        break;
                //    case "paymentreceived":
                //        _projectsList = _projectsList.Where(w => w.PaymentReceived == true).ToList();
                //        break;
                //    case "isactive":
                //        _projectsList = _projectsList.Where(w => w.IsActive == true).ToList();
                //        break;
                //    case "technologystack":
                //        _projectsList = _projectsList.Where(w => w.TechnologyStack.ToLower() == _searchBy.ToLower()).ToList();
                //        break;
                //    case "projecttype":
                //        _projectsList = _projectsList.Where(w => w.ProjectType.ToLower() == _searchBy.ToLower()).ToList();
                //        break;
                //    case "isdeleted":
                //        _projectsList = _projectsList.Where(w => w.IsDeleted == true).ToList();
                //        break;

                //    default: break;
                //}
            }
            catch (Exception ex)
            {
            }
            return _projectsList;
        }

        #endregion


        public static List<Projects> FilterProjectsByDate(List<Projects> _projectsList, string dateFrom, string dateTo)
        {
            DateTime assignedFromDate, assignedToDate;
           
            try
            {
                if (dateFrom != "")
                {
                    assignedFromDate = Convert.ToDateTime(dateFrom);
                    _projectsList = _projectsList.Where(o => o.DateAssigned.Date >= assignedFromDate.Date ).ToList();
                }
                else if (dateTo != "")
                {
                    assignedToDate = Convert.ToDateTime(dateTo);
                    _projectsList = _projectsList.Where(o => o.DateAssigned.Date <= assignedToDate.Date).ToList();
                }
                else
                {
                    assignedFromDate = Convert.ToDateTime(dateFrom);
                    assignedToDate = Convert.ToDateTime(dateTo);
                    _projectsList = _projectsList.Where(o => o.DateAssigned.Date >= assignedFromDate.Date && o.DateAssigned.Date <= assignedToDate.Date).ToList();

                }//_projectsList = _projectsList.Where(o => o.DateAssigned.Date == assignedFromDate.Date).ToList();


            }
            catch (Exception ex)
            {
            }
            return _projectsList;
        }






        //switch(monthBy)
        //        {
        //            case "1":
        //            case "jan":
        //            case "january":
        //                _milestonesList = _milestonesList.OrderByDescending(o => o.Name).ToList();
        //                break;
        //            case "2":
        //            case "feb":
        //            case "febuary":
        //                _milestonesList = _milestonesList.OrderByDescending(o => o.Name).ToList();
        //                break;
        //            case "3":
        //            case "mar":
        //            case "march":
        //                _milestonesList = _milestonesList.OrderByDescending(o => o.Name).ToList();
        //                break;
        //            case "4":
        //            case "apr":
        //            case "april":
        //                _milestonesList = _milestonesList.OrderByDescending(o => o.Name).ToList();
        //                break;
        //            case "5":
        //            case "may":
        //                _milestonesList = _milestonesList.OrderByDescending(o => o.Name).ToList();
        //                break;
        //            case "6":
        //            case "jun":
        //            case "june":
        //                _milestonesList = _milestonesList.OrderByDescending(o => o.Name).ToList();
        //                break;
        //            case "7":
        //            case "jul":
        //            case "july":
        //                _milestonesList = _milestonesList.OrderByDescending(o => o.Name).ToList();
        //                break;
        //            case "8":
        //            case "aug":
        //            case "august":
        //                _milestonesList = _milestonesList.OrderByDescending(o => o.Name).ToList();
        //                break;
        //            case "9":
        //            case "sep":
        //            case "september":
        //                _milestonesList = _milestonesList.OrderByDescending(o => o.Name).ToList();
        //                break;
        //            case "10":
        //            case "oct":
        //            case "october":
        //                _milestonesList = _milestonesList.OrderByDescending(o => o.Name).ToList();
        //                break;
        //            case "11":
        //            case "nov":
        //            case "november":
        //                _milestonesList = _milestonesList.OrderByDescending(o => o.Name).ToList();
        //                break;
        //            case "12":
        //            case "dec":
        //            case "december":
        //                _milestonesList = _milestonesList.OrderByDescending(o => o.Name).ToList();
        //                break;
        //        }

    }
}
