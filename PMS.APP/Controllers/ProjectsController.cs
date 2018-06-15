using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PMS.APP.Context;
using PMS.APP.Helper;
using PMS.APP.Models;

namespace PMS.APP.Controllers
{
    [Produces("application/json")]
    [Route("api/Projects")]
    public class ProjectsController : Controller
    {
        private readonly PMSContext _context;

        public ProjectsController(PMSContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects([FromQuery] Pagination pagination)
        {
            //return _context.Projects;
            try
            {
                var _projectList = await _context.Projects.Include(p => p.Milestones).ToListAsync();
                //=> filterby
                _projectList = Helpers.FilterProjects(_projectList, pagination.filter, pagination.by, pagination.month, pagination.year);
                //=> sorting 
                _projectList = Helpers.SortProjects(_projectList, pagination.sort, pagination.by);

                int _count = _projectList.Count;
                int CurrentPage = pagination.pageNo;
                int PageSize = pagination.pageSize;
                int _takeCount = _count < pagination.pageSize ? _count : pagination.pageSize;
                var _sortBy = Helpers.SortByProjects(pagination.sort, pagination.by);
                int TotalPages = (int)Math.Ceiling(_count / (double)PageSize);
                var previousPage = CurrentPage > 1 ? "Yes" : "No";
                var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

                if (pagination.pageNo > 0 && PageSize > 0)
                    _projectList = _projectList.Skip((CurrentPage - 1) * PageSize).Take(_takeCount).ToList();

                _takeCount = _projectList.Count;
                // Setting Header  
                //HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));
                var paginationMetadata = new
                {
                    totalCount = _count,
                    pageCount = _takeCount,
                    currentPage = CurrentPage,
                    totalPages = TotalPages,
                    previousPage,
                    nextPage
                };

                //=> sorting 
                _projectList = Helpers.SortProjects(_projectList, pagination.sort, pagination.by);
                try
                {
                    //removing circular reference error
                    var _listJson = JsonConvert.SerializeObject(_projectList, Formatting.Indented,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    });

                    _projectList = JsonConvert.DeserializeObject<List<Projects>>(_listJson);
                }
                catch (Exception ex)
                {
                }

                return Ok(new
                {
                    status = true,
                    meta = paginationMetadata,
                    data = _projectList
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    status = false,
                    message = HttpStatusCode.BadRequest.ToString()
                });
            }
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetProjectsAll([FromQuery] Pagination pagination)
        {
            //return _context.Projects;
            try
            {
                var _projectList = await _context.Projects.Include(p => p.Milestones).OrderBy(a => a.DateAssigned).ToListAsync();
                //=> filterby
                _projectList = Helpers.FilterProjects(_projectList, pagination.filter, pagination.by, pagination.month, pagination.year);

                //=> sorting 
                _projectList = Helpers.SortProjects(_projectList, pagination.sort, pagination.by);

                //removing circular reference error
                var _listJson = JsonConvert.SerializeObject(_projectList, Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });

                _projectList = JsonConvert.DeserializeObject<List<Projects>>(_listJson);

                var paginationMetadata = new
                {
                    totalCount = _projectList.Count,
                    pageCount = _projectList.Count,
                    currentPage = 1,
                    totalPages = 1
                };

                return Ok(new
                {
                    status = true,
                    meta = paginationMetadata,
                    data = _projectList
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    status = false,
                    message = HttpStatusCode.BadRequest.ToString()
                });
            }
        }

        [HttpGet("GetProjectsByTech")]
        public async Task<IActionResult> GetProjectsByTech([FromQuery] Pagination pagination)
        {
            //return _context.Projects;
            try
            {
                var _projectList = await _context.Projects.Include(p => p.Milestones).OrderBy(a => a.DateAssigned).ToListAsync();


                //=> filter by month / year
                _projectList = Helpers.FilterProjects(_projectList, pagination.filter, pagination.by, pagination.month, pagination.year);

                //=> filter by Tech 
                _projectList = Helpers.FilterProjectsByTech(_projectList, pagination.tech, pagination.filter);

                //=> sorting 
                _projectList = Helpers.SortProjects(_projectList, pagination.sort, pagination.by);

                //removing circular reference error
                var _listJson = JsonConvert.SerializeObject(_projectList, Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });

                _projectList = JsonConvert.DeserializeObject<List<Projects>>(_listJson);

                var paginationMetadata = new
                {
                    totalCount = _projectList.Count,
                    pageCount = _projectList.Count,
                    currentPage = 1,
                    totalPages = 1
                };

                //=> filter by Tech 
                List<Tech> _techList = Helpers.ProjectsByTech(_projectList, pagination.tech);

                return Ok(new
                {
                    status = true,
                    meta = paginationMetadata,
                    data = _techList
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    status = false,
                    message = HttpStatusCode.BadRequest.ToString()
                });
            }
        }

        //detail by technologies
        [HttpGet("GetDetailsByTech")]
        public async Task<IActionResult> GetDetailsByTech([FromQuery] Pagination pagination)
        {
            //return _context.Projects;
            try
            {
                var _projectList = await _context.Projects.Include(p => p.Milestones).OrderBy(a => a.DateAssigned).ToListAsync();

                //=> filter by month / year

                if (pagination.maxMonth > 0 && pagination.month > 0 && pagination.year > 0)
                    _projectList = Helpers.FilterProjectsByNextMonth(_projectList, pagination.filter, pagination.by, pagination.month, pagination.maxMonth, pagination.year);
                else
                    _projectList = Helpers.FilterProjects(_projectList, pagination.filter, pagination.by, pagination.month, pagination.year);

                //=> filter by Tech 
                _projectList = Helpers.FilterProjectsByTech(_projectList, pagination.tech, pagination.filter);

                //=> sorting 
                _projectList = Helpers.SortProjects(_projectList, pagination.sort, pagination.by);

                //removing circular reference error
                var _listJson = JsonConvert.SerializeObject(_projectList, Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });

                _projectList = JsonConvert.DeserializeObject<List<Projects>>(_listJson);


                var paginationMetadata = new
                {
                    totalCount = _projectList.Count,
                    pageCount = _projectList.Count,
                    currentPage = 1,
                    totalPages = 1
                };

                //for api result
                //List<ProjectMilestones> _projectMilestones = new List<ProjectMilestones>();
                //_projectMilestones = Helpers.ProjectMilestonesList(_projectList);

                //=> filter by Tech 


                List<ProjectByTech> _techList = Helpers.ProjectMilestonesByTech(_projectList, pagination.tech);

                return Ok(new
                {
                    status = true,
                    //meta = paginationMetadata,
                    data = _techList
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    status = false,
                    message = HttpStatusCode.BadRequest.ToString()
                });
            }
        }

        //detail by department

        [HttpGet("GetDetailsByDeptt")]
        public async Task<IActionResult> GetDetailsByDeptt([FromQuery] Pagination pagination, Int32 userId = 0)
        {

            //return _context.Projects;
            try
            {
                string userType = string.Empty;
                var _projectList = await _context.Projects.Include(p => p.Milestones).OrderBy(a => a.DateAssigned).ToListAsync();

                //=> filter by month / year

                if (pagination.maxMonth > 0 && pagination.month > 0 && pagination.year > 0)
                    _projectList = Helpers.FilterProjectsByNextMonth(_projectList, pagination.filter, pagination.by, pagination.month, pagination.maxMonth, pagination.year);
                else if (pagination.month > 0 && pagination.year > 0)
                    _projectList = Helpers.FilterProjectsByNextMonth(_projectList, pagination.filter, pagination.by, pagination.month, pagination.maxMonth, pagination.year);
                else
                    _projectList = Helpers.FilterProjects(_projectList, pagination.filter, pagination.by, pagination.month, pagination.year);


                //=>filter by Date
                if (pagination.dateFrom != "" && pagination.dateTo !="")
                { 
                _projectList = Helpers.FilterProjectsByDate(_projectList, pagination.dateFrom, pagination.dateTo);
                }
                //=> filter by Tech 
                _projectList = Helpers.FilterProjectsByTech(_projectList, pagination.tech, pagination.filter);

                //=> sorting 
                _projectList = Helpers.SortProjects(_projectList, pagination.sort, pagination.by);

                //removing circular reference error
                var _listJson = JsonConvert.SerializeObject(_projectList, Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });

                _projectList = JsonConvert.DeserializeObject<List<Projects>>(_listJson);


                var paginationMetadata = new
                {
                    totalCount = _projectList.Count,
                    pageCount = _projectList.Count,
                    currentPage = 1,
                    totalPages = 1
                };

                //for api result
                //List<ProjectMilestones> _projectMilestones = new List<ProjectMilestones>();
                //_projectMilestones = Helpers.ProjectMilestonesList(_projectList);


                Users _user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
                if (_user != null)
                {
                    userType = _user.UserType;
                }
                //=> filter by Tech 
                List<ProjectByDeptt> _techDepttList = Helpers.ProjectMilestonesByDeptt(_projectList, pagination.tech, userType);

                return Ok(new
                {
                    status = true,
                    //meta = paginationMetadata,
                    data = _techDepttList
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    status = false,
                    message = HttpStatusCode.BadRequest.ToString()
                });
            }
        }


        //detail by department
        [HttpGet("GetDetailsByMonth")]
        public async Task<IActionResult> GetDetailsByMonth([FromQuery] Pagination pagination)
        {
            //return _context.Projects;
            try
            {
                if (pagination.maxMonth <= 0 || pagination.month <= 0 || pagination.year <= 0)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "Valid values for api parameters maxMonth, month, year are required."
                    });
                }

                var _projectList = await _context.Projects.Include(p => p.Milestones).ToListAsync();

                //=> filter by month / year

                //if (pagination.maxMonth > 0 && pagination.month > 0 && pagination.year > 0)
                // _projectList = Helpers.FilterProjectsByNextMonth(_projectList, pagination.filter, pagination.by, pagination.month, pagination.maxMonth, pagination.year);
                //_projectList = Helpers.FilterProjects(_projectList, pagination.filter, pagination.by, pagination.month, pagination.year);

                //=> filter by Tech 
                _projectList = Helpers.FilterProjectsByTech(_projectList, pagination.tech, pagination.filter);

                //=> sorting 
                _projectList = Helpers.SortProjects(_projectList, pagination.sort, pagination.by);

                //removing circular reference error
                var _listJson = JsonConvert.SerializeObject(_projectList, Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });

                _projectList = JsonConvert.DeserializeObject<List<Projects>>(_listJson);

                //=> filter by month 

                List<ProjectByMonth> _techDepttMonth = Helpers.ProjectMilestonesByMonth(_projectList, pagination.maxMonth, pagination.month, pagination.year, pagination.tech);

                var paginationMetadata = new
                {
                    totalCount = _projectList.Count,
                    pageCount = _projectList.Count,
                    currentPage = 1,
                    totalPages = 1
                };

                return Ok(new
                {
                    status = true,
                    //meta = paginationMetadata,
                    data = _techDepttMonth
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    status = false,
                    message = HttpStatusCode.BadRequest.ToString()
                });
            }
        }

        enum Day {
            Sunday =1,
            Monday =2,
            Tuesday =3,
            Wednesday =4,
            Thursday =5,
            Friday =6,
            Saturday =7 };
        //Get Estimate By Month
        [HttpGet("GetEstimateByMonth")]
        public async Task<IActionResult> GetEstimateByMonth([FromQuery] Pagination pagination)
        {
            try { 
      

                if (pagination.maxMonth <= 0 || pagination.month <= 0 || pagination.year <= 0)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "Valid values for api parameters maxMonth, month, year are required."
                    });
                }

                var _projectList = await _context.Projects.Include(p => p.Milestones).ToListAsync();

                //=> filter by month / year

                //if (pagination.maxMonth > 0 && pagination.month > 0 && pagination.year > 0)
                // _projectList = Helpers.FilterProjectsByNextMonth(_projectList, pagination.filter, pagination.by, pagination.month, pagination.maxMonth, pagination.year);
                //_projectList = Helpers.FilterProjects(_projectList, pagination.filter, pagination.by, pagination.month, pagination.year);

                //=> filter by Tech 
                _projectList = Helpers.FilterProjectsByTech(_projectList, pagination.tech, pagination.filter);

                //=> sorting 
                _projectList = Helpers.SortProjects(_projectList, pagination.sort, pagination.by);

                //removing circular reference error
                var _listJson = JsonConvert.SerializeObject(_projectList, Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });

                _projectList = JsonConvert.DeserializeObject<List<Projects>>(_listJson);

                //=> filter by month  
                List<CostByMonthList> _techDepttMonth = Helpers.ProjectCostByMonth(_projectList, pagination.maxMonth, pagination.month, pagination.year, pagination.tech);

                var paginationMetadata = new
                {
                    totalCount = _projectList.Count,
                    pageCount = _projectList.Count,
                    currentPage = 1,
                    totalPages = 1
                };

                return Ok(new
                {
                    status = true,
                    //meta = paginationMetadata,
                    data = _techDepttMonth
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    status = false,
                    message = HttpStatusCode.BadRequest.ToString()
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjects([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    status = false,
                    message = ModelState.Keys
                });
            }

            var projects = await _context.Projects.Include(p => p.Milestones).Where(m => m.ProjectId == id).ToListAsync();

            var _Project = Helpers.ProjectMilestonesList(projects);

            if (_Project == null)
            {
                return NotFound(new
                {
                    status = false,
                    message = HttpStatusCode.NotFound.ToString()
                });
            }

            return Ok(new
            {
                status = true,
                data = _Project
            });
        }

        [HttpGet("Milestone/{id}")]
        public async Task<IActionResult> GetMilestonebyProject([FromRoute] int id, [FromQuery] Pagination pagination)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    status = false,
                    message = HttpStatusCode.BadRequest.ToString()
                });
            }

            try
            {
                var _milestonesList = await _context.Milestones.Where(m => m.ProjectId == id).ToListAsync();
                _milestonesList = Helpers.FilterMilestones(_milestonesList, pagination.filter, pagination.by, pagination.month, pagination.year);

                int _count = _milestonesList.Count();
                int CurrentPage = pagination.pageNo;
                int PageSize = pagination.pageSize;
                int _takeCount = _count < pagination.pageSize ? _count : pagination.pageSize;
                var _sortBy = Helpers.SortByMilestones(pagination.sort, pagination.by);
                int TotalPages = (int)Math.Ceiling(_count / (double)PageSize);
                var previousPage = CurrentPage > 1 ? "Yes" : "No";
                var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

                _milestonesList = _milestonesList.OrderBy(o => o.MilestoneId).Skip((CurrentPage - 1) * PageSize).Take(_takeCount).ToList();
                _milestonesList = Helpers.SortMilestones(_milestonesList, pagination.sort, pagination.by);

                _takeCount = _milestonesList.Count;
                // Setting Header  
                //HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));
                var paginationMetadata = new
                {
                    totalCount = _count,
                    pageCount = _takeCount,
                    currentPage = CurrentPage,
                    totalPages = TotalPages,
                    previousPage,
                    nextPage
                };

                return Ok(new
                {
                    status = true,
                    meta = paginationMetadata,
                    data = _milestonesList
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    status = false,
                    message = HttpStatusCode.BadRequest.ToString()
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjects([FromRoute] int id, [FromBody] Projects projects)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    status = false,
                    message = ModelState.Keys
                });
            }

            if (id != projects.ProjectId)
            {
                return BadRequest(new
                {
                    status = false,
                    message = HttpStatusCode.BadRequest.ToString()
                });
            }

            //if (ProjectsExistsByNameId(projects.ProjectId, projects.Name))
            //{
            //    return BadRequest(new
            //    {
            //        status = false,
            //        message = "Project already exists with name : " + projects.Name + "."
            //    });
            //}

            _context.Entry(projects).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                Projects objProjects = _context.Projects.Where(e => e.ProjectId == projects.ProjectId).FirstOrDefault();
                if (objProjects.Milestones == null)
                {
                    if (projects.ProjectType.ToLower() == "hourly")
                    {
                        //=> Add milestone
                        Milestones _Milestones = new Milestones();
                        _Milestones.Name = "Milestone 1 " + projects.Name;
                        _Milestones.Description = "Milestone 1 Description";
                        _Milestones.Amount = projects.EstimatedCost;
                        _Milestones.RecievedAmount = 0;
                        _Milestones.PaymentReceived = false;
                        _Milestones.StartDate = projects.DateAssigned;
                        _Milestones.DueDate = projects.DateCompleted ?? projects.DateAssigned;
                        _Milestones.EndDate = projects.DateCompleted;
                        _Milestones.IsCompleted = false;
                        //_Milestones.DatePaymentReceived = DateTime.Now;
                        _Milestones.ProjectId = projects.ProjectId;
                        _Milestones.Notes = "This Milestone is added as default. Please update if required.";
                        _context.Add(_Milestones);
                        _context.SaveChanges();
                    }
                }
                else if (objProjects.Milestones.Count == 0)
                {
                    if (projects.ProjectType.ToLower() == "hourly")
                    {
                        //=> Add milestone
                        Milestones _Milestones = new Milestones();
                        _Milestones.Name = "Milestone 1 " + projects.Name;
                        _Milestones.Description = "Milestone 1 Description";
                        _Milestones.Amount = projects.EstimatedCost;
                        _Milestones.RecievedAmount = 0;
                        _Milestones.PaymentReceived = false;
                        _Milestones.StartDate = projects.DateAssigned;
                        _Milestones.DueDate = projects.DateCompleted ?? projects.DateAssigned;
                        _Milestones.EndDate = projects.DateCompleted;
                        _Milestones.IsCompleted = false;
                        //_Milestones.DatePaymentReceived = DateTime.Now;
                        _Milestones.ProjectId = projects.ProjectId;
                        _Milestones.Notes = "This Milestone is added as default. Please update if required.";
                        _context.Add(_Milestones);
                        _context.SaveChanges();
                    }

                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectsExists(id))
                {
                    return NotFound(new
                    {
                        status = false,
                        message = HttpStatusCode.NotFound.ToString()
                    });
                }
                else
                {
                    throw;
                }
            }

            // return Ok(projects);
            return Ok(new
            {
                status = true,
                data = projects
            });
        }

        [HttpPut("Payment/{id}")]
        public async Task<IActionResult> PutProjectsPayment([FromRoute] int id, [FromBody] PPayment payment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    status = false,
                    message = ModelState.Keys
                });
            }

            if (id != payment.ProjectId || payment.PaymentReceived == null)
            {
                return BadRequest(new
                {
                    status = false,
                    message = HttpStatusCode.BadRequest.ToString()
                });
            }

            Projects projects = _context.Projects.Where(p => p.ProjectId == id).FirstOrDefault();
            projects.PaymentReceived = payment.PaymentReceived ?? false;
            projects.DatePaymentReceived = DateTime.Now;
            // _context.Entry(projects).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectsExists(id))
                {
                    return NotFound(new
                    {
                        status = false,
                        message = HttpStatusCode.NotFound.ToString()
                    });
                }
                else
                {
                    throw;
                }
            }

            // return Ok(projects);
            return Ok(new
            {
                status = true,
                data = projects
            });
        }

        [HttpPut("Completed/{id}")]
        public async Task<IActionResult> PutProjectsComplete([FromRoute] int id, [FromBody] PComplete complete)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    status = false,
                    message = ModelState.Keys
                });
            }

            if (id != complete.ProjectId || complete.IsCompleted == null)
            {
                return BadRequest(new
                {
                    status = false,
                    message = HttpStatusCode.BadRequest.ToString()
                });
            }

            var projects = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);
            projects.IsCompleted = complete.IsCompleted ?? false;
            projects.DateCompleted = DateTime.Now;

            // _context.Entry(projects).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectsExists(id))
                {
                    return NotFound(new
                    {
                        status = false,
                        message = HttpStatusCode.NotFound.ToString()
                    });
                }
                else
                {
                    throw;
                }
            }
            return Ok(new
            {
                status = true,
                data = projects
            });
            // return Ok(projects);
        }

        [HttpPost] //=> To add project
        public async Task<IActionResult> PostProjects([FromBody] Projects projects)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    status = false,
                    message = ModelState.Keys
                });
            }

            if (ProjectsExistsByName(projects.Name, projects.TechnologyStack))
            {
                return BadRequest(new
                {
                    status = false,
                    message = "Project name already exists."
                });
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //=> Add Project
                    _context.Projects.Add(projects);
                    _context.SaveChanges();
                    if (projects.ProjectType.ToLower() == "hourly")
                    {
                        //=> Add milestone
                        Milestones _Milestones = new Milestones();
                        _Milestones.Name = "Milestone 1 " + projects.Name;
                        _Milestones.Description = "Milestone 1 Description";
                        _Milestones.Amount = projects.EstimatedCost;
                        _Milestones.RecievedAmount = 0;
                        _Milestones.PaymentReceived = false;
                        _Milestones.StartDate = projects.DateAssigned;
                        _Milestones.DueDate = projects.DateCompleted ?? projects.DateAssigned;
                        _Milestones.EndDate = projects.DateCompleted;
                        _Milestones.IsCompleted = false;
                        //_Milestones.DatePaymentReceived = DateTime.Now;
                        _Milestones.ProjectId = projects.ProjectId;
                        _Milestones.Notes = "This Milestone is added as default. Please update if required.";
                        _context.Add(_Milestones);
                        _context.SaveChanges();
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
            try
            {
                return Ok(new
                {
                    status = true,
                    data = projects
                });
            }
            catch
            {
                return CreatedAtAction("GetProjects", new { id = projects.ProjectId }, projects);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjects([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    status = false,
                    message = ModelState.Keys
                });
            }

            var projects = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);
            if (projects == null)
            {
                return NotFound(new
                {
                    status = false,
                    message = HttpStatusCode.NotFound.ToString()
                });
            }

            projects.IsDeleted = true;
            // _context.Projects.Remove(projects);
            await _context.SaveChangesAsync();


            return Ok(new
            {
                status = true,
                data = projects
            });
        }

        private bool ProjectsExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectId == id);
        }

        private bool ProjectsExistsByName(string name, string tech)
        {
            return _context.Projects.Any(e => e.Name.ToLower() == name.Trim().ToLower() && e.TechnologyStack.ToLower() == tech.ToLower().Trim());
        }

        private bool ProjectsExistsByNameId(int id, string name)
        {
            return _context.Projects.Any(e => e.ProjectId != id && e.Name.ToLower() == name.Trim().ToLower());
        }

    }
}