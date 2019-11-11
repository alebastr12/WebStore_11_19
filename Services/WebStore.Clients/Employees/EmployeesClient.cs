using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Configuration;
using WebStore.Clients.Base;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Employees
{
    public class EmployeesClient : BaseClient, IEmployeesData
    {
        public EmployeesClient(IConfiguration config) : base(config, "api/employees") { }

        public IEnumerable<Employee> GetAll() => Get<List<Employee>>(_ServiceAddress);

        public Employee GetById(int id) => Get<Employee>($"{_ServiceAddress}/{id}");

        public void AddNew(Employee employee) => Post(_ServiceAddress, employee);

        public Employee Update(int id, Employee employee)
        {
            var response = Put($"{_ServiceAddress}/{id}", employee);
            return response.Content.ReadAsAsync<Employee>().Result;
        }

        public void Delete(int id) => Delete($"{_ServiceAddress}/{id}");

        public void SaveChanges() { }
    }
}
