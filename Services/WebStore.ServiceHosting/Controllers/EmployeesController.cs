using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase, IEmployeesData
    {
        private readonly IEmployeesData _EmployeesData;
        private readonly ILogger<EmployeesController> _Logger;

        public EmployeesController(IEmployeesData EmployeesData, ILogger<EmployeesController> Logger)
        {
            _EmployeesData = EmployeesData;
            _Logger = Logger;
        }

        [HttpGet, ActionName("Get")]
        public IEnumerable<Employee> GetAll() => _EmployeesData.GetAll();

        [HttpGet("{id}"), ActionName("Get")]
        public Employee GetById(int id) => _EmployeesData.GetById(id);

        [HttpPost, ActionName("Post")]
        public void AddNew(Employee employee)
        {
            using (_Logger.BeginScope("Добавление нового сотрудника {0}", employee.FirstName))
            {
                _EmployeesData.AddNew(employee);
            }
        }

        [HttpPut("{id}"), ActionName("Put")]
        public Employee Update(int id, [FromBody] Employee employee) => _EmployeesData.Update(id, employee);

        [HttpDelete("{id}")]
        public void Delete(int id) => _EmployeesData.Delete(id);

        [NonAction]
        public void SaveChanges() => _EmployeesData.SaveChanges();
    }
}