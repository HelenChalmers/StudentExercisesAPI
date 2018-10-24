using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using StudentExercisesAPI.Data;
using Dapper;
using Microsoft.AspNetCore.Http;

namespace StudentExercisesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CohortsController : ControllerBase
    {
        private readonly IConfiguration _config;

        public CohortsController(IConfiguration config)
        {
            _config = config;
        }
        //makes connect using Dapper
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        // GET api/cohorts?q=Taco
        [HttpGet]
        public async Task<IActionResult> Get(string q)
        {
            string sql = @"
            SELECT
     
                c.Id,
                c.Name
            FROM Cohort c
            WHERE 1=1
            ";

            if (q != null)
            {
                string isQ = $@"
                    AND c.Name LIKE '%{q}%'
                    
                ";
                sql = $"{sql} {isQ}";
            }




            using (IDbConnection conn = Connection)
            {

                IEnumerable<Cohort> cohorts = await conn.QueryAsync<Cohort>(
                    sql
                );
                return Ok(cohorts);
            }
        }
    }
}

        
