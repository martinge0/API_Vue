using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Vue.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace API_Vue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        public EmployeeController(IWebHostEnvironment env)
        {
            _env = env;
        }
        [HttpGet]
        public ActionResult GetEmployees()
        {
            var emp = new List<Employee>();
            using (var db = new mytestdbContext())
            {
                var em = db.Employee.ToList();
                emp.AddRange(em);
            }
            if (!emp.Any())
            {
                return NotFound();
            }
            return Ok(emp);
        }
        [HttpPost]
        public ActionResult CreateEmployee(Employee arg)
        {
            using (var db = new mytestdbContext())
            {
                db.Employee.Add(arg);
                db.SaveChanges();
            }
            return Ok("Added Successfully");
        }
        [HttpDelete("{id}")]
        public ActionResult DeleteEmployee(int id)
        {
            using (var db = new mytestdbContext())
            {
                var forDelete = db.Employee.FirstOrDefault(x => x.EmployeeId == id );
                if (forDelete == null)
                    return NotFound();
                db.Remove(forDelete);
                db.SaveChanges();
            }
            return NoContent();
        }
        [HttpPut]
        public ActionResult UpdateEmployee(Employee arg)
        {
            var emp = new Employee();
            using (var db = new mytestdbContext())
            {
                emp = db.Employee.FirstOrDefault(x => x.EmployeeId == arg.EmployeeId);
                if (emp == null)
                    return NotFound();
                emp.EmployeeId = arg.EmployeeId;
                emp.EmployeeName = arg.EmployeeName;
                emp.Department = arg.Department;
                emp.DateOfJoining = arg.DateOfJoining;
                db.SaveChanges();
            }
            return Ok(emp);
        }
        [Route("Savefile")]
        [HttpPost]
        public ActionResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                var fileName = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + fileName;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return Ok(fileName);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
    }
}
