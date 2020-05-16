using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tutorial10.DTOs.Request;
using Tutorial10.DTOs.Response;
using Tutorial10.Entities;
using Tutorial10.Helper;
using Tutorial10.Services;

namespace Tutorial10.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IDbService _db;
        private readonly IMapper _mapper;

        public StudentController(IDbService context, IMapper mapper)
        {
            _db = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudetns()
        {
            var users = await _db.GetStudents();
            var usersToReturn = _mapper.Map<IEnumerable<StudentsToListResponse>>(users);

            return Ok(usersToReturn);

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteStudent(StudentToDeleteRequest request)
        {
            var student = new Student
            {
                IndexNumber = request.IndexNumber,
            };

            try
            {
                _db.DeleteStudent(student);
                await _db.SaveAll();
                return Ok("Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest("Not Deleted " + ex.Message);
            }

        }
        [HttpPost("inserting")]
        public async Task<IActionResult> InsertStudent(StudentInsertRequest request)
        {
            var student = new Student
            {
                IndexNumber = request.IndexNumber,
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthDate = request.BirthDate,
                IdEnrollment = request.IdEnrollment
            };


            try
            {
                _db.AddStudent(student);
                await _db.SaveAll();
                return Ok("Inserted");
            }
            catch (Exception ex)
            {

                return BadRequest("Not Inserted " + ex.Message);
            }

        }

        [HttpPost("upd")]
        public async Task<IActionResult> UpdateStudent(StudentRequest request)
        {
            try
            {
                _db.UpdateStudent(request);
                await _db.SaveAll();
                return Ok("Updated");
            }
            catch(DbServiceExceptionHandler e)
            {
                if (e.Type == ExceptionHandlerEnumType.NotFound)
                    return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest("Not updated " + e.Message);
            }
            return BadRequest("Noting");
        }

        [HttpPost("enrollment")]
        public async Task<IActionResult> Enrollment(StudentEnrollmentRequest request)
        {
            var enrollment = await _db.EnrollStudentWithExistingStudy(request);

            try
            {
                await _db.SaveAll();
                return Ok(enrollment);
            }
            catch (DbServiceExceptionHandler e)
            {
                if (e.Type == ExceptionHandlerEnumType.NotFound)
                    return NotFound(e.Message);
                else if (e.Type == ExceptionHandlerEnumType.NotUnique)
                    return BadRequest(e.Message);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

            return BadRequest("Noting");

            
        }

        [HttpPost("promoting")]
        public async Task<IActionResult> Promote(PromoteStudentRequest request)
        {
            try
            {
                var enrollment = await _db.PromoteStudent(request);
                 await _db.SaveAll();
                return Ok(enrollment);
            }
            catch(DbServiceExceptionHandler e)
            {
                if (e.Type == ExceptionHandlerEnumType.NotFound)
                    return NotFound(e.Message);
                else if (e.Type == ExceptionHandlerEnumType.NotUnique)
                    return BadRequest(e.Message);
            }
            catch (Exception e)
            {

                return BadRequest("Error: " + e.Message);
            }
            return Ok();
        }

    }
}