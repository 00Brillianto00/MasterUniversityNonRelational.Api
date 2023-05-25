using MasterUniversityNonRelational.Api.Interfaces;
using MasterUniversityNonRelational.Api.Models;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;

namespace MasterUniversityNonRelational.Api.Services
{
    public class StudentService : IStudentService
    {
        private IMongoCollection<Student> _student;
        private Random rng = new Random();
        public StudentService(IMongoClient mongoDBClient, IDatabaseSettings databaseSettings)
        {

            var database = mongoDBClient.GetDatabase(databaseSettings.DatabaseName);
            databaseSettings.CollectionName = "Student";
            _student = database.GetCollection<Student>(databaseSettings.CollectionName);
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            try
            {
                var data = await _student.Find(Student => Student.IsDeleted == false).ToListAsync();
                //var data = await _student.Find(Student => true).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error When Retrieving Data");
            }
        }

        public async Task<Student> GetByIdAsync(Guid id)
        {
            var ID = id.ToString();
            try
            {
                var data = await _student.Find(Student => Student.Id.Equals(ID) && Student.IsDeleted == false).FirstOrDefaultAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error When Retrieving Data");
            }
        }

        public async Task<Student> Save(Student studentData)
        {
            studentData.Id = Guid.NewGuid().ToString();
            studentData.IsDeleted = false;
            try
            {
                await _student.InsertOneAsync(studentData);
                return studentData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error When Saving Data");
            }
        }

        public async Task<Student> Update(string id, Student studentData)
        {
            studentData.Id = id;
            studentData.IsDeleted = false;
            try
            {
                var data = await _student.ReplaceOneAsync(studentData => studentData.Id.Equals(id), studentData);
                return studentData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error When Updating Data");
            }
        }
        public async Task<bool> Delete(Guid id)
        {
            var ID = id.ToString();
            try
            {
                await _student.DeleteOneAsync(studentData => studentData.Id.Equals(ID));
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("error when deleting data");
            }
        }

        private string generatePhoneNum()
        {
            string firsTwoDigits = rng.Next(0,99).ToString("00");
            string nextFourDigits = rng.Next(0,1000).ToString("0000");
            string lastFourDigits = rng.Next(0,1000).ToString("0000");
            string phoneNum = "08"+firsTwoDigits +"-"+nextFourDigits+"-"+lastFourDigits ;
            return phoneNum;
        }

        private DateTime generateDoB()
        {
            DateTime startDate = new DateTime(1960, 1, 1);
            DateTime endDate = new DateTime(2000, 1, 1);
            int rangeDate = (endDate - startDate).Days;
            DateTime RandomDay = startDate.AddDays(rng.Next(rangeDate));
            return RandomDay;
        }

        public async Task<List<Student>> TestStudentInsert(int testCases, List<UniversityData> universities)
        {
            List<Student> studentDatas = new List<Student>();
            long StudentNumber = rng.NextInt64(1000000000, 9999999999);
            string firstName = "StudentFirstName";
            string middleName = "StudentMiddleName";
            string lastName = "StudentLastName_";
            string address = "JL Kemanggisan Raya";
            string country = "Indonesia";
            string province = "DKI Jakarta";
            string city = "Jakarta Barat";
            try
            {
                for (int x = 0; x < testCases; x++)
                {
                    Student studentData = new Student();
                    studentData.Id = Guid.NewGuid().ToString();
                    studentData.UniversityID = universities[rng.Next(0,universities.Count())].Id;
                    studentData.StudentNumber = StudentNumber++;
                    string getModifier = studentData.StudentNumber.ToString().Substring(StudentNumber.ToString().Length - 4, 4);
                    studentData.StudentEmail = firstName + "." + lastName + getModifier + "@Univ.ac.id";
                    studentData.StudentName = firstName + " "+ middleName + " "+lastName + getModifier;
                    studentData.StudentGPA = rng.NextDouble() * (4.0 - 1.0) + 1.0;
                    studentData.TotalCreditsEarned = rng.Next(0, 100); 
                    studentData.EnrolledYear = rng.Next(2000, 2023).ToString();
                    studentData.StudentDateOfBirth = generateDoB();
                    studentData.StudentPhoneNumber = generatePhoneNum();
                    studentData.StudentAddress = address + " No." +rng.Next(0, 50) + ","+ city+ ","+province+","+country;
                    studentData.StudentPostalCode = rng.Next(1000, 9999);
                    studentData.IsDeleted = false;
                    await _student.InsertOneAsync(studentData);
                    studentDatas.Add(studentData);
                }
                return studentDatas;
            }
            catch (Exception ex)
            {
                throw new Exception("Error When Saving Data");
            }
        }

        public async Task<List<Student>> TestStudentUpdate(int testCase)
        {

            List<Student> studentData = new List<Student>();
            List<Student> NewStudentData = new List<Student>();
            try
            {
                studentData = await TestStudentGet(testCase);
            }
            catch (Exception ex)
            {
                throw new Exception("Error When Retrieving Data");
            }

            long StudentNumber = rng.NextInt64(1000000000, 9999999999);
            string firstName = "UPDATED_StudentFirstName";
            string middleName = "UPDATED_StudentMiddleName";
            string lastName = "UPDATED_StudentLastName_";
            string address = "UPDATED_JL Kemanggisan Raya";
            string country = "UPDATED_Indonesia";
            string province = "UPDATED_DKI Jakarta";
            string city = "UPDATED_Jakarta Barat";
            
            for (int x=0; x < studentData.Count(); x++)
            {
                string Id = studentData[x].Id;
                    string getModifier = studentData[x].StudentNumber.ToString().Substring(StudentNumber.ToString().Length - 4, 4);
                    studentData[x].StudentEmail = firstName + "." + lastName + getModifier + "@Univ.ac.id";
                    studentData[x].StudentName = firstName + " "+ middleName + " "+lastName + getModifier;
                    studentData[x].StudentGPA = rng.NextDouble() * (4.0 - 1.0) + 1.0;
                    studentData[x].TotalCreditsEarned = rng.Next(0, 100); 
                    studentData[x].EnrolledYear = rng.Next(2000, 2023).ToString();
                    studentData[x].StudentDateOfBirth = generateDoB();
                    studentData[x].StudentPhoneNumber = generatePhoneNum();
                    studentData[x].StudentAddress = address + " No." +rng.Next(0, 50) + ","+ city+ ","+province+","+country;
                    studentData[x].StudentPostalCode = rng.Next(1000, 9999);
                    await _student.ReplaceOneAsync(studentData => studentData.Id.Equals(Id), studentData[x]);
                    NewStudentData.Add(studentData[x]);
            }
            return NewStudentData;
        }

        public async Task<List<Student>> TestStudentGet(int testCase)
        {

            List<Student> studentData = new List<Student>();
            try
            {
                studentData = await _student.Find(student => true && student.IsDeleted.Equals(false)).SortBy(student => student.StudentNumber).Limit(testCase).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error When Retrieving Data");
            }
            return studentData;
        }

        public async Task<string> TestCase(Student studentData, int testCases)
        {
            Stopwatch stopWatch = new Stopwatch();
            try
            {
                
                stopWatch.Start();
                for (int x=0; x<testCases;x++)
                {
                    studentData.Id = Guid.NewGuid().ToString();
                    studentData.StudentNumber = studentData.StudentNumber++;
                    studentData.StudentName = studentData.StudentName + studentData.Id.Substring(0,3);
                    studentData.StudentEmail = studentData.StudentName + "@Univ.ac.id";
                    studentData.EnrolledYear = rng.Next(2000, 2023).ToString();
                    studentData.StudentDateOfBirth = generateDoB();
                    studentData.StudentPhoneNumber = generatePhoneNum();
                    studentData.StudentAddress = studentData.StudentAddress +" " + studentData.StudentNumber.ToString();
                    studentData.StudentPostalCode = rng.Next(10000 - 9999);
                    studentData.IsDeleted = false;
                    await _student.InsertOneAsync(studentData);
                }
                stopWatch.Stop();

                var result = "Time Alotted "+stopWatch.Elapsed.Hours.ToString() + ":" + stopWatch.Elapsed.Minutes.ToString() + ":" + stopWatch.Elapsed.Seconds.ToString() + "." + stopWatch.Elapsed.Milliseconds.ToString();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error When Saving Data");
            }
        }
    }
}
