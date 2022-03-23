using API_Vue.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace API_Vue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public ActionResult GetDeparments()
        {
            var dep = new List<Department>();
            using (var db = new mytestdbContext())
            {
                var dp = db.Department.ToList();
                dep.AddRange(dp);
            }
            if (!dep.Any())
            {
                return NotFound();
            }
            return Ok(dep);
        }

        [HttpPost]
        public ActionResult CreateNewDeparment(Department arg)
        {
            var dep = new List<Department>();
            using (var db = new mytestdbContext())
            {
                db.Department.Add(arg);
                db.SaveChanges();
            }
            return Ok("Added Successfully");
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteDepartment(int id)
        {
            using (var db = new mytestdbContext())
            {
                var forDelete = db.Department.FirstOrDefault(x=> x.DepartmentId == id);
                db.Department.Remove(forDelete);
                db.SaveChanges();
            }
            return NoContent();
        }

        //public JsonResult Get()
        //{
        //    var table = new DataTable();
        //    var list = new List<ViewDepartment>();
        //    using (var db = new mytestdbContext())
        //    {
        //        var dep = db.Department.ToList();
        //        //var dep = db.Department.Select(x => new Department()
        //        //{
        //        //    DepartmentId = x.DepartmentId,
        //        //    DepartmentName = x.DepartmentName
        //        //}).ToList();


        //    }

        //    return new JsonResult(table);
        //}
    }
}
