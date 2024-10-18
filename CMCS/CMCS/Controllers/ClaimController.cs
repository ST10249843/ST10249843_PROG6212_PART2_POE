using Microsoft.AspNetCore.Mvc;
using CMCS.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace CMCS.Controllers
{
    public class ClaimController : Controller
    {
        // Static list to simulate a data source
        private static readonly List<Claim> Claims = new List<Claim>();

        // GET: SubmitClaim
        [Authorize(Roles = "Lecturer")]  // Only lecturers can access this
        public IActionResult SubmitClaim()
        {
            ViewBag.IsSuccess = (bool?)null;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Lecturer")]  // Only lecturers can submit claims
        public async Task<IActionResult> SubmitClaim(Claim claim)
        {
            if (ModelState.IsValid)
            {
                claim.CalculateTotalAmount();

                if (claim.SupportingDocument != null && claim.SupportingDocument.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    var filePath = Path.Combine(uploadsFolder, claim.SupportingDocument.FileName);

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await claim.SupportingDocument.CopyToAsync(stream);
                    }
                }
                else
                {
                    ModelState.AddModelError("SupportingDocument", "You must upload a file.");
                    return View(claim);
                }

                claim.ID = Claims.Count > 0 ? Claims.Max(c => c.ID) + 1 : 1;
                Claims.Add(claim);
                ViewBag.IsSuccess = true;

                return View(claim);
            }

            return View(claim);
        }

        [Authorize(Roles = "Lecturer")]  // Both lecturers and admins can view claims
        public IActionResult ViewClaims()
        {
            var claims = Claims.OrderBy(c => c.DateOfSubmission).ToList();
            return View(claims);
        }

        // GET: ApproveClaims
        [Authorize(Roles = "Admin")]  // Only admins can approve claims
        public IActionResult ApproveClaims()
        {
            var claims = Claims.Where(c => c.Status == "Pending")
                               .OrderBy(c => c.DateOfSubmission)
                               .ToList();
            return View(claims);
        }

        // POST: ApproveClaims/Approve
        [HttpPost]
        [Authorize(Roles = "Admin")]  // Only admins can approve claims
        public IActionResult Approve(int claimId)
        {
            var claim = Claims.FirstOrDefault(c => c.ID == claimId);
            if (claim != null)
            {
                claim.Status = "Approved";
            }

            return RedirectToAction("ApproveClaims");
        }

        // POST: ApproveClaims/Deny
        [HttpPost]
        [Authorize(Roles = "Admin")]  // Only admins can deny claims
        public IActionResult Deny(int claimId)
        {
            var claim = Claims.FirstOrDefault(c => c.ID == claimId);
            if (claim != null)
            {
                claim.Status = "Denied";
            }

            return RedirectToAction("ApproveClaims");
        }
    }
}
