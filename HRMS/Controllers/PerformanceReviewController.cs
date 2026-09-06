using AutoMapper;
using HRMS.Models;
using HRMS.Repositories.Interfaces;
using HRMS.ViewModels.PerformanceReview;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    [Authorize(Roles = "HR")]
    public class PerformanceReviewController : Controller
    {
        private readonly IPerformanceReviewRepository _performanceReviewRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public PerformanceReviewController(
            IPerformanceReviewRepository performanceReviewRepository,
            IEmployeeRepository employeeRepository,
            IMapper mapper)
        {
            _performanceReviewRepository = performanceReviewRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        // GET: PerformanceReview
        public async Task<IActionResult> Index()
        {
            var reviews =
                await _performanceReviewRepository
                    .GetAllPerformanceReviewsAsync();

            return View(reviews);
        }

        // GET: PerformanceReview/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Employees =
                await _employeeRepository.GetEmployeesAsync();

            return View();
        }

        // POST: PerformanceReview/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            PerformanceReviewViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Employees =
                    await _employeeRepository.GetEmployeesAsync();

                return View(model);
            }

            var review =
                _mapper.Map<PerformanceReview>(model);

            await _performanceReviewRepository
                .AddPerformanceReviewAsync(review);

            return RedirectToAction(nameof(Index));
        }

        // GET: PerformanceReview/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var review =
                await _performanceReviewRepository
                    .GetPerformanceReviewByIdAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // GET: PerformanceReview/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var review =
                await _performanceReviewRepository
                    .GetPerformanceReviewByIdAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            ViewBag.Employees =
                await _employeeRepository.GetEmployeesAsync();

            var model =
                _mapper.Map<PerformanceReviewViewModel>(review);

            return View(model);
        }

        // POST: PerformanceReview/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            PerformanceReviewViewModel model)
        {
            if (id != model.PerformanceReviewId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Employees =
                    await _employeeRepository.GetEmployeesAsync();

                return View(model);
            }

            var review =
                await _performanceReviewRepository
                    .GetPerformanceReviewByIdAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            _mapper.Map(model, review);

            await _performanceReviewRepository
                .UpdatePerformanceReviewAsync(review);

            return RedirectToAction(nameof(Index));
        }

        // GET: PerformanceReview/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var review =
                await _performanceReviewRepository
                    .GetPerformanceReviewByIdAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: PerformanceReview/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review =
                await _performanceReviewRepository
                    .GetPerformanceReviewByIdAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            await _performanceReviewRepository
                .DeletePerformanceReviewAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}