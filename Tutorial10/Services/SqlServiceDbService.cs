using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tutorial10.DTOs.Request;
using Tutorial10.Entities;
using Tutorial10.Helper;

namespace Tutorial10.Services
{
    public class SqlServiceDbService : IDbService
    {
        private readonly StudentContext _studentContext;

        public SqlServiceDbService(StudentContext studentContext)
        {
            _studentContext = studentContext;
        }



        public void AddStudent(Student entity) 
        {
            _studentContext.Add(entity);
        }

        public void DeleteStudent(Student entity)
        {
            _studentContext.Remove(entity);
        }  

        public async Task<IEnumerable<Student>> GetStudents()
        {
            var users = await _studentContext.Student
                                             .Include(opt => opt.IdEnrollmentNavigation)
                                             .ThenInclude(opt => opt.IdStudyNavigation)
                                             .ToListAsync();

            return users;
        }

        public async Task<bool> SaveAll()
        {
            return await _studentContext.SaveChangesAsync() > 0;
        }

        public  void UpdateStudent(StudentRequest request)
        {
            var existStudent =  _studentContext.Student
                                                    .Where(opt => opt.IndexNumber == request.IndexNumber)
                                                    .Any();
            if (existStudent == false)
                throw new DbServiceExceptionHandler(ExceptionHandlerEnumType.NotFound, "Student does not exist!");

            var st = new Student
            {
                IndexNumber = request.IndexNumber,
                FirstName = request.FirstName,
                LastName = request.LastName
            };


            _studentContext.Entry(st).Property("FirstName").IsModified = true;
            _studentContext.Entry(st).Property("LastName").IsModified = true;
           
        }

        public async Task<Enrollment> EnrollStudentWithExistingStudy(StudentEnrollmentRequest request)
        {
            var ExistedStudy = await _studentContext.Studies
                                        .Where(opt => opt.Name == request.Name)
                                        .FirstOrDefaultAsync();

            if (ExistedStudy == null)
                throw new DbServiceExceptionHandler(ExceptionHandlerEnumType.NotFound, "Study does not exist!");

            var latestEnrollment = await _studentContext.Enrollment.Include(opt => opt.IdStudyNavigation)
                                                          .Where(opt => opt.Semester == 1 && opt.IdStudyNavigation.Name == request.Name)
                                                          .OrderByDescending(opt => opt.IdEnrollment)
                                                          .FirstAsync();

            if (latestEnrollment == null)
                latestEnrollment = new Enrollment
                {
                    IdEnrollment = _studentContext.Enrollment.Count()+1,
                    Semester = 1,
                    IdStudy = ExistedStudy.IdStudy,
                    StartDate = DateTime.Today,
                };

            var studentFound =  await _studentContext.Student
                                .AnyAsync(opt => opt.IndexNumber == request.IndexNumber);

            if (studentFound)
                throw new DbServiceExceptionHandler(ExceptionHandlerEnumType.NotUnique, "Student already exist");

            var newStudent = new Student
            {
                IndexNumber = request.IndexNumber,
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthDate = request.BirthDate,
                IdEnrollment = latestEnrollment.IdEnrollment
            };

            AddStudent(newStudent);
      
            return latestEnrollment;
        }

        public async Task<Enrollment> PromoteStudent(PromoteStudentRequest request)
        {
            var existedStudy = await _studentContext.Studies
                                            .Where(opt => opt.Name == request.Study)
                                            .FirstOrDefaultAsync();
            if (existedStudy == null)
                throw new DbServiceExceptionHandler(ExceptionHandlerEnumType.NotFound, "Study does not exist");
                /*return null;*/


            var existedEnrollment = await _studentContext.Enrollment
                                                         .Include(opt => opt.IdStudyNavigation)
                                                         .Where(opt => opt.Semester == request.Semester &&
                                                          opt.IdStudyNavigation.Name == request.Study)
                                                         .AnyAsync();
            if (existedEnrollment == false)
                throw new DbServiceExceptionHandler(ExceptionHandlerEnumType.NotFound, "Enrollment does not exist");
               /* return null;*/

            var promotedEnrollment = await _studentContext.Enrollment
                                                         .Include(opt => opt.IdStudyNavigation)
                                                         .Where(opt => (opt.Semester == request.Semester + 1) &&
                                                          opt.IdStudyNavigation.Name == request.Study)
                                                         .FirstOrDefaultAsync();

            if (promotedEnrollment == null)
                promotedEnrollment = new Enrollment
                {
                    IdEnrollment = _studentContext.Enrollment.Count() + 1,
                    Semester = request.Semester + 1,
                    IdStudy = existedStudy.IdStudy,
                    StartDate = DateTime.Today
                };

            _studentContext.Add(promotedEnrollment);


            var studens = await _studentContext.Student
                                               .Include(opt => opt.IdEnrollmentNavigation)
                                               .ThenInclude(opt => opt.IdStudyNavigation)
                                               .Where(opt => opt.IdEnrollmentNavigation.Semester == request.Semester
                                               && opt.IdEnrollmentNavigation.IdStudyNavigation.Name == request.Study)
                                               .ToListAsync();
            foreach(var student in studens)
            {
                student.IdEnrollment = promotedEnrollment.IdEnrollment;
                _studentContext.Entry(student).Property("IdEnrollment").IsModified = true;
            }

            return promotedEnrollment;
            
        }
    }
}
