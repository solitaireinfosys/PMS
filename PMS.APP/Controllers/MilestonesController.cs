using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS.APP.Context;
using PMS.APP.Helper;
using PMS.APP.Models;

namespace PMS.APP.Controllers
{
    [Produces("application/json")]
    [Route("api/Milestones")]
    public class MilestonesController : Controller
    {
        private readonly PMSContext _context;

        public MilestonesController(PMSContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetMilestones([FromQuery] Pagination pagination)
        {
            //return _context.Milestones;
            try
            {
                var _milestonesList = await _context.Milestones.OrderBy(a => a.StartDate).ToListAsync();
                _milestonesList = Helpers.FilterMilestones(_milestonesList, pagination.filter, pagination.by, pagination.month, pagination.year);

                int _count = _milestonesList.Count();
                int CurrentPage = pagination.pageNo;
                int PageSize = pagination.pageSize;
                int _takeCount = _count < pagination.pageSize ? _count : pagination.pageSize;
                var _sortBy = Helpers.SortByMilestones(pagination.sort, pagination.by);
                int TotalPages = (int)Math.Ceiling(_count / (double)PageSize);
                var previousPage = CurrentPage > 1 ? "Yes" : "No";
                var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

                _milestonesList = _milestonesList.Skip((CurrentPage - 1) * PageSize).Take(_takeCount).ToList();
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
            catch
            {
                return BadRequest(new
                {
                    status = false,
                    message = HttpStatusCode.BadRequest.ToString()
                });
            }
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetMilestonesAll([FromQuery] Pagination pagination)
        {
            try
            {
                var _milestonesList = await _context.Milestones.OrderBy(a => a.StartDate).ToListAsync();
                //=> filterby
                _milestonesList = Helpers.FilterMilestones(_milestonesList, pagination.filter, pagination.by, pagination.month, pagination.year);
                //=> sortby
                _milestonesList = Helpers.SortMilestones(_milestonesList, pagination.sort, pagination.by);

                // Setting Header  
                //HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));
                var paginationMetadata = new
                {
                    totalCount = _milestonesList.Count,
                    pageCount = _milestonesList.Count,
                    currentPage = 1,
                    totalPages = 1
                };

                return Ok(new
                {
                    status = true,
                    meta = paginationMetadata,
                    data = _milestonesList
                });

            }
            catch
            {
                return BadRequest(new
                {
                    status = false,
                    message = HttpStatusCode.BadRequest.ToString()
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMilestones([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    status = false,
                    message = HttpStatusCode.BadRequest.ToString()
                });
            }

            var milestones = await _context.Milestones.FirstOrDefaultAsync(m => m.MilestoneId == id);

            if (milestones == null)
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
                data = milestones
            });
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMilestones([FromRoute] int id, [FromBody] Milestones milestones)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    status = false,
                    message = HttpStatusCode.BadRequest.ToString()
                });
            }

            //if (EmptyOrNullValidation.IsAnyNullOrEmpty(milestones))
            //{
            //    //return BadRequest(new
            //    //{
            //    //    status = HttpStatusCode.BadRequest.ToString(),
            //    //    message = "Values cannot be empty"
            //    //});
            //}

            if (id != milestones.MilestoneId)
            {
                return BadRequest(new
                {
                    status = false,
                    message = HttpStatusCode.BadRequest.ToString()
                });
            }

            //if (MilestonesExistsByNameId(milestones.MilestoneId, milestones.Name))
            //{
            //    return BadRequest(new
            //    {
            //        status = false,
            //        message = "Milestone already exists with name : " + milestones.Name + "."
            //    });
            //}
            Milestones objMilestones = _context.Milestones.Where(x => x.MilestoneId == id).FirstOrDefault();
            if (objMilestones != null)
            {
                //var _RecievedAmount = objMilestones.RecievedAmount + milestones.RecievedAmount;                
                objMilestones.RecievedAmount += milestones.RecievedAmount;
                objMilestones.Name = milestones.Name;
                objMilestones.Notes = milestones.Notes;
                objMilestones.IsCompleted = milestones.IsCompleted;
                objMilestones.PaymentReceived = milestones.PaymentReceived;
                objMilestones.ProjectId = milestones.ProjectId;                               
                objMilestones.StartDate = milestones.StartDate;
                objMilestones.Amount = milestones.Amount;
                objMilestones.DatePaymentReceived = milestones.DatePaymentReceived;
                objMilestones.DueDate = milestones.DueDate;
                objMilestones.EndDate = milestones.EndDate;
                objMilestones.Description = milestones.Description;
            }

            try
            {
                await _context.SaveChangesAsync();
                //=> To update project estimated cost based on milestone
                UpdateProjectEstimatedCost(milestones.ProjectId);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MilestonesExists(id))
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
                data = milestones
            });
        }

        [HttpPut("Payment/{id}")]
        public async Task<IActionResult> PutMilestonesPayment([FromRoute] int id, [FromBody] MPayment payment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    status = false,
                    message = ModelState.Keys
                });
            }

            if (id != payment.MilestoneId || payment.PaymentReceived == null
                || (payment.PaymentReceived == true && payment.RecievedAmount == 0))
            {
                return BadRequest(new
                {
                    status = false,
                    message = HttpStatusCode.BadRequest.ToString()
                });
            }

            Milestones milestones = _context.Milestones.Where(p => p.MilestoneId == id).FirstOrDefault();
            milestones.PaymentReceived = payment.PaymentReceived ?? false;
            if (payment.PaymentReceived == true)
            {
                milestones.RecievedAmount += payment.RecievedAmount;
                if(milestones.RecievedAmount > milestones.Amount)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = "Total recieved amount should be less then total milestone amount."
                    });
                }

                milestones.DatePaymentReceived = DateTime.Now;
            }
            // _context.Entry(milestones).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                //=> To update project estimated cost based on milestone
                UpdateProjectEstimatedCost(milestones.ProjectId);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MilestonesExists(id))
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
                data = milestones
            });
        }

        [HttpPut("Completed/{id}")]
        public async Task<IActionResult> PutMilestonesComplete([FromRoute] int id, [FromBody] MComplete complete)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    status = false,
                    message = ModelState.Keys
                });
            }

            if (id != complete.MilestoneId || complete.IsCompleted == null)
            {
                return BadRequest(new
                {
                    status = false,
                    message = HttpStatusCode.BadRequest.ToString()
                });
            }

            var milestones = await _context.Milestones.FirstOrDefaultAsync(p => p.MilestoneId == id);
            milestones.IsCompleted = complete.IsCompleted ?? false;
            milestones.EndDate = DateTime.Now;

            // _context.Entry(milestones).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MilestonesExists(id))
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
                data = milestones
            });
        }

        [HttpPost] //=> To add milestone
        public async Task<IActionResult> PostMilestones([FromBody] Milestones milestones)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    status = false,
                    message = ModelState.Keys
                });
            }

            if (MilestonesExistsByName(milestones.Name, milestones.ProjectId))
            {
                return BadRequest(new
                {
                    status = false,
                    message = "Milestone already exists."
                });
            }

            _context.Milestones.Add(milestones);
            await _context.SaveChangesAsync();

            //=> To update project estimated cost based on milestone
            UpdateProjectEstimatedCost(milestones.ProjectId);

            try
            {
                return Ok(new
                {
                    status = true,
                    data = milestones
                });
            }
            catch
            {
                return CreatedAtAction("GetMilestones", new { id = milestones.MilestoneId }, milestones);
            }
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMilestones([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    status = false,
                    message = HttpStatusCode.BadRequest.ToString()
                });
            }

            var milestones = await _context.Milestones.FirstOrDefaultAsync(m => m.MilestoneId == id);
            if (milestones == null)
            {
                return NotFound(new
                {
                    status = false,
                    message = HttpStatusCode.NotFound.ToString()
                });
            }
            _context.Milestones.Remove(milestones);
            await _context.SaveChangesAsync();
            
            return Ok(new
            {
                status = true,
                data = milestones
            });
        }

        private bool MilestonesExists(int id)
        {
            return _context.Milestones.Any(e => e.MilestoneId == id);
        }

        private bool MilestonesExistsByName(string Name, int projectId)
        {
            return _context.Milestones.Any(e => e.Name.ToLower() == Name.Trim().ToLower() && e.ProjectId == projectId);
        }
        
        private bool MilestonesExistsByNameId(int id, string Name)
        {
            return _context.Milestones.Any(e => e.MilestoneId != id && e.Name.ToLower() == Name.Trim().ToLower());
        }

        private void UpdateProjectEstimatedCost(int id)
        {
            try
            {
                var projects = _context.Projects.Include(x => x.Milestones).FirstOrDefault(p => p.ProjectId == id);
                projects.EstimatedCost = projects.EstimatedCost<projects.Milestones.Sum(s => s.Amount)? projects.Milestones.Sum(s => s.Amount) : projects.EstimatedCost;
                _context.SaveChanges();
            }
            catch { }
        }

    }
}