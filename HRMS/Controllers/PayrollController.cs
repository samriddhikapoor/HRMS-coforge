using AutoMapper;
using HRMS.Models;
using HRMS.Repositories.Interfaces;
using HRMS.ViewModels.Payroll;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRMS.Controllers
{
    [Authorize(Roles = "HR")]
    public class PayrollController : Controller
    {
        private readonly IPayrollRepository _payrollRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public PayrollController(
            IPayrollRepository payrollRepository,
            IEmployeeRepository employeeRepository,
            IMapper mapper)
        {
            _payrollRepository = payrollRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        // GET: Payroll
        public async Task<IActionResult> Index()
        {
            var payrolls =
                await _payrollRepository.GetAllPayrollsAsync();

            var viewModels =
                _mapper.Map<IEnumerable<PayrollViewModel>>(payrolls);

            return View(viewModels);
        }

        // GET: Payroll/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadEmployeesAsync();

            return View();
        }

        // POST: Payroll/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            PayrollViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadEmployeesAsync(model.EmployeeId);
                return View(model);
            }

            var payroll =
                _mapper.Map<Payroll>(model);

            // Automatically calculate Net Salary
            payroll.NetSalary =
                payroll.BasicSalary
                + payroll.Allowance
                - payroll.Deduction;

            await _payrollRepository.AddPayrollAsync(payroll);

            return RedirectToAction("Index");
        }

        // GET: Payroll/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var payroll =
                await _payrollRepository.GetPayrollByIdAsync(id);

            if (payroll == null)
            {
                return NotFound();
            }

            var viewModel =
                _mapper.Map<PayrollViewModel>(payroll);

            return View(viewModel);
        }

        // GET: Payroll/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var payroll =
                await _payrollRepository.GetPayrollByIdAsync(id);

            if (payroll == null)
            {
                return NotFound();
            }

            var viewModel =
                _mapper.Map<PayrollViewModel>(payroll);

            await LoadEmployeesAsync(viewModel.EmployeeId);

            return View(viewModel);
        }

        // POST: Payroll/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            PayrollViewModel model)
        {
            if (id != model.PayrollId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                await LoadEmployeesAsync(model.EmployeeId);
                return View(model);
            }

            var payroll =
                _mapper.Map<Payroll>(model);

            // Automatically calculate Net Salary
            payroll.NetSalary =
                payroll.BasicSalary
                + payroll.Allowance
                - payroll.Deduction;

            await _payrollRepository.UpdatePayrollAsync(payroll);

            return RedirectToAction("Index");
        }

        // GET: Payroll/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var payroll =
                await _payrollRepository.GetPayrollByIdAsync(id);

            if (payroll == null)
            {
                return NotFound();
            }

            var viewModel =
                _mapper.Map<PayrollViewModel>(payroll);

            return View(viewModel);
        }

        // POST: Payroll/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _payrollRepository.DeletePayrollAsync(id);

            return RedirectToAction("Index");
        }

        // Load employees for dropdown
        private async Task LoadEmployeesAsync(
            int? selectedEmployeeId = null)
        {
            var employees =
                await _employeeRepository.GetEmployeesAsync();

            ViewBag.Employees = new SelectList(
                employees,
                "EmployeeId",
                "EmployeeName",
                selectedEmployeeId);
        }
    }
}