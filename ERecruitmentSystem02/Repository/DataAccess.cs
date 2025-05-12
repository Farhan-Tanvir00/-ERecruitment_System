using ERecruitmentSystem02.Common.HelperClasses;
using ERecruitmentSystem02.Models.Domain;
using ERecruitmentSystem02.Models.Shared;
using ERecruitmentSystem02.Models.View;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Configuration;
using System.Data;
using System.Transactions;

namespace ERecruitmentSystem02.Repository
{
    public class DataAccess : IDataAccess
    {
        private readonly DataBaseHelper _dbHelper;

        public DataAccess(IConfiguration config)
        {
            _dbHelper = new DataBaseHelper(config);
        }

        public async Task<List<UsersBasic>> GetUsers()
        {
            var users = new List<UsersBasic>();

            var cmd = await _dbHelper.CreateStoredProcedureCommandAsync("GET_ALL_USERS_BASIC");
            _dbHelper.AddRefCursor(cmd, "p_cursor");

            using var reader = await _dbHelper.ExecuteReaderAsync(cmd);

            while (await reader.ReadAsync())
            {
                users.Add(new UsersBasic
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString(),
                    Sex = reader["Sex"].ToString(),
                    DateOfBirth = DateOnly.FromDateTime(Convert.ToDateTime(reader["DateOfBirth"])),
                    Nationality = reader["Nationality"].ToString(),
                    Email = reader["Email"].ToString(),
                    NationalIdCardNo = reader["NationalIdCardNo"].ToString(),
                    ContactMobileNo = reader["ContactMobileNo"].ToString()
                });
            }

            return users;
        }

        public async Task<List<Job>> GetAllJobs()
        {
            var jobs = new List<Job>();
            var cmd = await _dbHelper.CreateStoredProcedureCommandAsync("GET_ALL_AVAILABLE_JOBS");
            _dbHelper.AddRefCursor(cmd, "p_cursor");

            using var reader = await _dbHelper.ExecuteReaderAsync(cmd);

            while (await reader.ReadAsync())
            {
                jobs.Add(new Job
                {
                    JobID = Convert.ToInt32(reader["JobID"]),
                    Position = reader["Position"].ToString(),
                    NumberOfPosts = Convert.ToInt32(reader["NumberOfPosts"]),
                    Salary = reader["Salary"] != DBNull.Value ? Convert.ToDecimal(reader["Salary"]) : 0,
                    Graduation = reader["Graduation"].ToString(),
                    PostGraduation = reader["PostGraduation"].ToString(),
                    Deadline = DateOnly.FromDateTime(Convert.ToDateTime(reader["Deadline"])),
                    Details = reader["Details"].ToString(),
                    EduReqDetails = reader["EduReqDetails"].ToString(),
                    PublishedBy = reader["publishedBy"].ToString()
                });
                
            }
            return jobs;
        }
        public Task<UsersBasic> GetUsersBasic(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetHassedPass(int Id)
        {
            string password = null;

            var cmd = await _dbHelper.CreateStoredProcedureCommandAsync("GET_P_HASHED");

            _dbHelper.AddInputParameter(cmd, "v_id", OracleDbType.Int32, Id);

            _dbHelper.AddOutputParameter(cmd, "v_password", OracleDbType.Varchar2);
            _dbHelper.AddOutputParameter(cmd, "v_Status", OracleDbType.Varchar2);
            _dbHelper.AddOutputParameter(cmd, "v_Message", OracleDbType.Varchar2);
            _dbHelper.AddOutputParameter(cmd, "v_Err_Details", OracleDbType.Varchar2);

            await _dbHelper.ExecuteNonQueryAsync(cmd);

            password = cmd.Parameters["v_password"].Value?.ToString();

            cmd.Dispose();

            return password;

        }

        public async Task<BaseResponse> IsApplicantAvailable(ForgotPassword forgotPassword)
        {
            BaseResponse res = new BaseResponse();
            var cmd = await _dbHelper.CreateStoredProcedureCommandAsync("USER_AVAILABLE");

            _dbHelper.AddOutputParameter(cmd, "v_Status", OracleDbType.Varchar2);
            _dbHelper.AddOutputParameter(cmd, "v_Message", OracleDbType.Varchar2);
            _dbHelper.AddOutputParameter(cmd, "v_Err_Details", OracleDbType.Varchar2);


            _dbHelper.AddInputParameter(cmd, "v_Id", OracleDbType.Int32, forgotPassword.Id);
            _dbHelper.AddInputParameter(cmd, "v_DOB", OracleDbType.Date, forgotPassword.DateOfBirth.ToDateTime(TimeOnly.MinValue));
            _dbHelper.AddInputParameter(cmd, "v_ContactNo", OracleDbType.Varchar2, forgotPassword.ContactMobileNo);

            await _dbHelper.ExecuteNonQueryAsync(cmd);

            res.Status = cmd.Parameters["v_Status"].Value?.ToString();
            res.Message = cmd.Parameters["v_Message"].Value?.ToString();
            res.Details = cmd.Parameters["v_Err_Details"].Value?.ToString();

            cmd.Dispose();
            return res;
        }

        public async Task<RetrieveIdNumber> RetrieveIdNumber(RetrieveIdNumber retrieveIdNumber)
        {
            BaseResponse res = new BaseResponse();
            var cmd = await _dbHelper.CreateStoredProcedureCommandAsync("GET_REGISTERED_ID");

            _dbHelper.AddOutputParameter(cmd, "v_Status", OracleDbType.Varchar2);
            _dbHelper.AddOutputParameter(cmd, "v_Message", OracleDbType.Varchar2);
            _dbHelper.AddOutputParameter(cmd, "v_Err_Details", OracleDbType.Varchar2);
            _dbHelper.AddOutputParameter(cmd, "v_Id", OracleDbType.Int32);

            _dbHelper.AddInputParameter(cmd, "v_Name", OracleDbType.Varchar2, retrieveIdNumber.Name);
            _dbHelper.AddInputParameter(cmd, "v_DateOfBirth", OracleDbType.Date, retrieveIdNumber.DateOfBirth.ToDateTime(TimeOnly.MinValue));
            _dbHelper.AddInputParameter(cmd, "v_ContactMobileNo", OracleDbType.Varchar2, retrieveIdNumber.ContactNo);

            await _dbHelper.ExecuteNonQueryAsync(cmd);

            res.Status = cmd.Parameters["v_Status"].Value?.ToString();
            res.Message = cmd.Parameters["v_Message"].Value?.ToString();
            res.Details = cmd.Parameters["v_Err_Details"].Value?.ToString();
            var oracleDecimal = (OracleDecimal)cmd.Parameters["v_Id"].Value;

            retrieveIdNumber.BaseResponse = res;
            retrieveIdNumber.Id = oracleDecimal.ToInt32();

            return retrieveIdNumber;
        }

        public async Task<Requirements> GetRequirements(int Id)
        {
            Requirements Requirements = new Requirements();
            BaseResponse res = new BaseResponse();

            try
            {
                var cmd = await _dbHelper.CreateStoredProcedureCommandAsync("GET_A_JOBS_REQUIREMENTS");

                _dbHelper.AddInputParameter(cmd, "P_Id", OracleDbType.Int32, Id);

                _dbHelper.AddOutputParameter(cmd, "O_STATUS", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(cmd, "O_MESSAGE", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(cmd, "O_ERR_DETAILS", OracleDbType.Varchar2);

                _dbHelper.AddOutputParameter(cmd, "O_MaxAge", OracleDbType.Int32);
                _dbHelper.AddOutputParameter(cmd, "O_Graduation", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(cmd, "O_PostGraduation", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(cmd, "O_Deadline", OracleDbType.Date);

                await _dbHelper.ExecuteNonQueryAsync(cmd);

                res.Status = cmd.Parameters["O_STATUS"].Value?.ToString();
                res.Message = cmd.Parameters["O_MESSAGE"].Value?.ToString();
                res.Details = cmd.Parameters["O_ERR_DETAILS"].Value?.ToString();

                if(res.Status == "FAILED")
                {
                    Requirements.BaseResponse = res;
                    return Requirements;
                }

                Requirements.Graduation = cmd.Parameters["O_Graduation"].Value?.ToString();
                Requirements.PostGraduation = cmd.Parameters["O_PostGraduation"].Value?.ToString();

                var oracleDate = (OracleDate)cmd.Parameters["O_Deadline"].Value;
                Requirements.Deadline = oracleDate.IsNull ? default : DateOnly.FromDateTime(oracleDate.Value);


                var oracleDecimal = (OracleDecimal)cmd.Parameters["O_MaxAge"].Value;
                Requirements.MaxAge = oracleDecimal.IsNull ? 0 : oracleDecimal.ToInt32();

                Requirements.BaseResponse = res;
                return Requirements;
            }
            catch(Exception e)
            {
                return Requirements;
            }
        }

        public async Task<List<ApplicantsEduQualifications>> GetApplicantsEdu(int Id)
        {
            var qualifications = new List<ApplicantsEduQualifications>();

            var cmd = await _dbHelper.CreateStoredProcedureCommandAsync("GET_APPLICANTS_EDUQUALIFICATIONS");

            _dbHelper.AddRefCursor(cmd, "p_cursor");
            _dbHelper.AddInputParameter(cmd, "v_Id", OracleDbType.Int32, Id);

            var reader =await _dbHelper.ExecuteReaderAsync(cmd);

            while (reader.Read())
            {
                qualifications.Add(new ApplicantsEduQualifications()
                {
                    NameOfExamination = reader["NameOfExamination"].ToString(),
                    GroupOrSubject = reader["GroupOrSubject"].ToString(),
                    DivisionOrClassOrGrade = reader["DivisionOrClassOrGrade"].ToString(),
                    GpaOrCgpa = reader["GpaOrCgpa"] != DBNull.Value ? Convert.ToDouble(reader["GpaOrCgpa"]) : 0,
                    GpaOutOf = reader["GpaOutOf"] != DBNull.Value ? Convert.ToDouble(reader["GpaOutOf"]) : 0,
                    BoardOrUnicersity = reader["BoardOrUnicersity"].ToString(),
                    PassingYear = reader["PassingYear"] != DBNull.Value ? Convert.ToInt32(reader["PassingYear"]) : 0,

                });
            }

            return qualifications;
        }

        public async Task<Job> GetJobsDetails(int id)
        {
            Job job = new Job();
            BaseResponse res = new BaseResponse();

            try
            {
                var cmd = await _dbHelper.CreateStoredProcedureCommandAsync("GET_A_JOBS_DETAILS");

                _dbHelper.AddInputParameter(cmd, "P_Id", OracleDbType.Int32, id);


                _dbHelper.AddOutputParameter(cmd, "O_STATUS", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(cmd, "O_MESSAGE", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(cmd, "O_ERR_DETAILS", OracleDbType.Varchar2);

                _dbHelper.AddOutputParameter(cmd, "O_Position", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(cmd, "O_NumberOfPosts", OracleDbType.Int32);
                _dbHelper.AddOutputParameter(cmd, "O_Salary", OracleDbType.Int32);
                _dbHelper.AddOutputParameter(cmd, "O_Details", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(cmd, "O_EduReqDetails", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(cmd, "O_publishedBy", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(cmd, "O_MaxAge", OracleDbType.Int32);
                _dbHelper.AddOutputParameter(cmd, "O_Graduation", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(cmd, "O_PostGraduation", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(cmd, "O_Deadline", OracleDbType.Date);

                await _dbHelper.ExecuteNonQueryAsync(cmd);

                res.Status = cmd.Parameters["O_STATUS"].Value?.ToString();
                res.Message = cmd.Parameters["O_MESSAGE"].Value?.ToString();
                res.Details = cmd.Parameters["O_ERR_DETAILS"].Value?.ToString();

                if (res.Status == "FAILED")
                {
                    job.BaseResponse = res;
                    return job;
                }
                job.JobID = id;
                job.Position = cmd.Parameters["O_Position"].Value?.ToString();

                var oracleDecimal = (OracleDecimal)cmd.Parameters["O_NumberOfPosts"].Value;
                job.NumberOfPosts = oracleDecimal.IsNull ? 0 : oracleDecimal.ToInt32();

                var oracleDecimal0 = (OracleDecimal)cmd.Parameters["O_Salary"].Value;
                job.Salary = oracleDecimal0.IsNull ? 0 : oracleDecimal0.ToInt32();

                job.Details = cmd.Parameters["O_Details"].Value?.ToString();
                job.EduReqDetails = cmd.Parameters["O_EduReqDetails"].Value?.ToString();
                job.PublishedBy = cmd.Parameters["O_publishedBy"].Value?.ToString();
                job.Graduation = cmd.Parameters["O_Graduation"].Value?.ToString();
                job.PostGraduation = cmd.Parameters["O_PostGraduation"].Value?.ToString();

                var oracleDate = (OracleDate)cmd.Parameters["O_Deadline"].Value;
                job.Deadline = oracleDate.IsNull ? default : DateOnly.FromDateTime(oracleDate.Value);


                var oracleDecimal1 = (OracleDecimal)cmd.Parameters["O_MaxAge"].Value;
                job.MaxAge = oracleDecimal1.IsNull ? 0 : oracleDecimal1.ToInt32();

                job.BaseResponse = res;
                return job;

            }
            catch(Exception e)
            {
                return job;
            }

        }

        public async Task<RegistrationPageViewModel> GetApplicantsCV(int id)
        {
            RegistrationPageViewModel viewModel = new RegistrationPageViewModel();

            UsersBasic basic = new UsersBasic();
            UsersDetails details = new UsersDetails();
            List<UsersEducationalQualifications> edu = new List<UsersEducationalQualifications>();
            List<UsersExperiences> exp = new List<UsersExperiences>();
            List<UsersLanguageproficiencies> lang = new List<UsersLanguageproficiencies>();
            List<UsersPersonalCertifications> cir = new List<UsersPersonalCertifications>();
            UsersPhoto pht = new UsersPhoto();
            UsersSignature sig = new UsersSignature();
            List<UsersReferences> refr = new List<UsersReferences>();

            BaseResponse res = new BaseResponse();

            try
            {
                using var connection = await _dbHelper.GetConnectionAsync();

                var command = await _dbHelper.CreateStoredProcedureCommandAsync("GET_A_USERS_BASIC");

                _dbHelper.AddInputParameter(command, "v_Id", OracleDbType.Int32, id);


                _dbHelper.AddOutputParameter(command, "O_STATUS", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(command, "O_MESSAGE", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(command, "O_ERR_DETAILS", OracleDbType.Varchar2);

                _dbHelper.AddOutputParameter(command, "v_Name", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(command, "v_Sex", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(command, "v_DateOfBirth", OracleDbType.Date);
                _dbHelper.AddOutputParameter(command, "v_Nationality", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(command, "v_Email", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(command, "v_NationalIdCardNo", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(command, "v_ContactMobileNo", OracleDbType.Varchar2);

                await _dbHelper.ExecuteNonQueryAsync(command);

                res.Status = command.Parameters["O_STATUS"].Value?.ToString();
                res.Message = command.Parameters["O_MESSAGE"].Value?.ToString();
                res.Details = command.Parameters["O_ERR_DETAILS"].Value?.ToString();

                basic.Id = id;
                basic.Name = command.Parameters["v_Name"].Value?.ToString();
                basic.Sex = command.Parameters["v_Sex"].Value?.ToString();
                basic.DateOfBirth = DateOnly.FromDateTime(Convert.ToDateTime(command.Parameters["v_DateOfBirth"].Value?.ToString()));
                basic.Nationality = command.Parameters["v_Nationality"].Value?.ToString();
                basic.Email = command.Parameters["v_Email"].Value?.ToString();
                basic.NationalIdCardNo = command.Parameters["v_NationalIdCardNo"].Value?.ToString();
                basic.ContactMobileNo = command.Parameters["v_ContactMobileNo"].Value?.ToString();

                command.Dispose();



                command = await _dbHelper.CreateStoredProcedureCommandAsync("GET_A_USERS_DETAILS");

                _dbHelper.AddInputParameter(command, "v_Id", OracleDbType.Int32, id);

                _dbHelper.AddOutputParameter(command, "O_STATUS", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(command, "O_MESSAGE", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(command, "O_ERR_DETAILS", OracleDbType.Varchar2);

                _dbHelper.AddOutputParameter(command, "v_PresentArea", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(command, "v_PresentPostOffice", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(command, "v_PresentPoliceStation", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(command, "v_PresentPostalCode", OracleDbType.Int32);
                _dbHelper.AddOutputParameter(command, "v_PresentDistrict", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(command, "v_PermanentArea", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(command, "v_PermanentPostOffice", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(command, "v_PermanentPoliceStation", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(command, "v_PermanentPostalCode", OracleDbType.Int32);
                _dbHelper.AddOutputParameter(command, "v_PermanentDistrict", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(command, "v_HomeDistrict", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(command, "v_CareerObjective", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(command, "v_MaritalStatus", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(command, "v_Religion", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(command, "v_FathersName", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(command, "v_MothersName", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(command, "v_Spouse", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(command, "v_HasExperience", OracleDbType.Varchar2);                
                _dbHelper.AddOutputParameter(command, "v_ComputerLiteracy", OracleDbType.Varchar2);

                await _dbHelper.ExecuteNonQueryAsync(command);

                res.Status = command.Parameters["O_STATUS"].Value?.ToString();
                res.Message = command.Parameters["O_MESSAGE"].Value?.ToString();
                res.Details = command.Parameters["O_ERR_DETAILS"].Value?.ToString();

                details.PresentAreaOrVillageOrHouseOrRoad = command.Parameters["v_PresentArea"].Value?.ToString();
                details.PresentPostOffice = command.Parameters["v_PresentPostOffice"].Value?.ToString();
                details.PresentPoliceStation = command.Parameters["v_PresentPoliceStation"].Value?.ToString();
                details.PresentPostalCode = ((OracleDecimal)command.Parameters["v_PresentPostalCode"].Value).ToInt32();
                details.PresentDistrict = command.Parameters["v_PresentDistrict"].Value?.ToString();

                details.PermanentAreaOrVillageOrHouseOrRoad = command.Parameters["v_PermanentArea"].Value?.ToString();
                details.PermanentPostOffice = command.Parameters["v_PermanentPostOffice"].Value?.ToString();
                details.PermanentPoliceStation = command.Parameters["v_PermanentPoliceStation"].Value?.ToString();
                details.PermanentPostalCode = ((OracleDecimal)command.Parameters["v_PermanentPostalCode"].Value).ToInt32();
                details.PermanentDistrict = command.Parameters["v_PermanentDistrict"].Value?.ToString();

                details.HomeDistrict = command.Parameters["v_HomeDistrict"].Value?.ToString();
                details.CareerObjective = command.Parameters["v_CareerObjective"].Value?.ToString();
                details.MaritalStatus = command.Parameters["v_MaritalStatus"].Value?.ToString();
                details.Religion = command.Parameters["v_Religion"].Value?.ToString();
                details.FathersName = command.Parameters["v_FathersName"].Value?.ToString();
                details.MothersName = command.Parameters["v_MothersName"].Value?.ToString();
                details.Spouse = command.Parameters["v_Spouse"].Value?.ToString();
                details.HasExperience = command.Parameters["v_HasExperience"].Value?.ToString();
                details.ComputerLiteracy = command.Parameters["v_ComputerLiteracy"].Value?.ToString();

                command.Dispose();
                //------------------------------------------------------------------------------------------------------
                command = await _dbHelper.CreateStoredProcedureCommandAsync("GET_A_USERS_EDUCATIONAL_QUALIFICATION");

                _dbHelper.AddInputParameter(command, "v_Id", OracleDbType.Int32, id);
                _dbHelper.AddRefCursor(command, "p_cursor");

                using var reader = await _dbHelper.ExecuteReaderAsync(command);

                while (await reader.ReadAsync())
                {
                    edu.Add(new UsersEducationalQualifications
                    {
                        NameOfExamination = reader["NameOfExamination"].ToString(),
                        GroupOrSubject = reader["GroupOrSubject"].ToString(),
                        DivisionOrClassOrGrade = reader["DivisionOrClassOrGrade"].ToString(),
                        GpaOrCgpa = reader["GpaOrCgpa"] != DBNull.Value ? Convert.ToDouble(reader["GpaOrCgpa"]) : -1,
                        GpaOutOf = reader["GpaOutOf"] != DBNull.Value ? Convert.ToDouble(reader["GpaOutOf"]) : -1,
                        RollNo = reader["RollNo"] != DBNull.Value ? reader["RollNo"].ToString() : null,
                        BoardOrUnicersity = reader["BoardOrUnicersity"].ToString(),
                        PassingYear = reader["PassingYear"] != DBNull.Value ? Convert.ToInt32(reader["PassingYear"]) : -1,
                    });
                }

                command.Dispose();
                //------------------------------------------------------------------------------------------------------
                //------------------------------------------------------------------------------------------------------

                command = await _dbHelper.CreateStoredProcedureCommandAsync("GET_A_USERS_PERSONAL_CERTIFICATIONS");

                _dbHelper.AddInputParameter(command, "v_Id", OracleDbType.Int32, id);
                _dbHelper.AddRefCursor(command, "p_cursor");

                using var reader0 = await _dbHelper.ExecuteReaderAsync(command);

                while (await reader0.ReadAsync())
                {
                    cir.Add(new UsersPersonalCertifications
                    {
                        Certification = reader0["Certification"].ToString(),
                        NameOfInstitution = reader0["NameOfInstitution"].ToString(),
                        Location = reader0["Location"].ToString(),
                        FromDate = reader0["FromDate"] != DBNull.Value ? DateOnly.FromDateTime(Convert.ToDateTime(reader0["FromDate"])) : null,
                        ToDate = reader0["ToDate"] != DBNull.Value ? DateOnly.FromDateTime(Convert.ToDateTime(reader0["ToDate"])) : null,
                    });
                }

                command.Dispose();

                //------------------------------------------------------------------------------------------------------




                command = await _dbHelper.CreateStoredProcedureCommandAsync("GET_A_USERS_EXPERIENCES");

                _dbHelper.AddInputParameter(command, "v_Id", OracleDbType.Int32, id);
                _dbHelper.AddRefCursor(command, "p_cursor");

                using var reader1 = await _dbHelper.ExecuteReaderAsync(command);

                while (await reader1.ReadAsync())
                {
                    exp.Add(new UsersExperiences
                    {
                        Designation = reader1["Designation"].ToString(),
                        NameOfOrganization = reader1["NameOfOrganization"].ToString(),
                        JobType = reader1["JobType"].ToString(),
                        Responsibilities = reader1["Responsibilities"].ToString(),
                        JoiningDate = reader1["JoiningDate"] != DBNull.Value ? DateOnly.FromDateTime(Convert.ToDateTime(reader1["JoiningDate"])) : null,
                        ReleaseDate = reader1["ReleaseDate"] != DBNull.Value ? DateOnly.FromDateTime(Convert.ToDateTime(reader1["ReleaseDate"])) : null,
                        Continuing = reader1["Continuing"].ToString()

                    });
                }

                command.Dispose();

                //------------------------------------------------------------------------------------------------------

                command = await _dbHelper.CreateStoredProcedureCommandAsync("GET_A_USERS_LANGUAGE_PROFICIENCIES");

                _dbHelper.AddInputParameter(command, "v_Id", OracleDbType.Int32, id);
                _dbHelper.AddRefCursor(command, "p_cursor");

                using var reader2 = await _dbHelper.ExecuteReaderAsync(command);

                while (await reader2.ReadAsync())
                {
                    lang.Add(new UsersLanguageproficiencies
                    {
                        LanguageName = reader2["LanguageName"].ToString(),
                        Speaking = reader2["Speaking"].ToString(),
                        Reading = reader2["Reading"].ToString(),
                        Listening = reader2["Listening"].ToString(),
                        Writing = reader2["Writing"].ToString(),
                    });
                }

                command.Dispose();
                //------------------------------------------------------------------------------------------------------

                command = await _dbHelper.CreateStoredProcedureCommandAsync("GET_A_USERS_REFERENCES");

                _dbHelper.AddInputParameter(command, "v_Id", OracleDbType.Int32, id);
                _dbHelper.AddRefCursor(command, "p_cursor");

                using var reader3 = await _dbHelper.ExecuteReaderAsync(command);

                while (await reader3.ReadAsync())
                {
                    refr.Add(new UsersReferences
                    {
                        Name = reader3["Name"]?.ToString(),
                        Address = reader3["Address"]?.ToString(),
                        ContactNo = reader3["ContactNo"]?.ToString(),
                        Email = reader3["Email"]?.ToString(),
                    });
                }

                command.Dispose();

                //------------------------------------------------------------------------------------------------------
                command = await _dbHelper.CreateStoredProcedureCommandAsync("GET_A_USERS_PHOTO");

                _dbHelper.AddInputParameter(command, "v_Id", OracleDbType.Int32, id);

                _dbHelper.AddOutputParameter(command, "O_STATUS", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(command, "O_MESSAGE", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(command, "O_ERR_DETAILS", OracleDbType.Varchar2);

                _dbHelper.AddOutputParameter(command, "v_ImageData", OracleDbType.Blob);
                _dbHelper.AddOutputParameter(command, "v_FileName", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(command, "v_ContentType", OracleDbType.Varchar2);

                await _dbHelper.NccExecuteNonQueryAsync(command); //using non close con..


                res.Status = command.Parameters["O_STATUS"].Value?.ToString();
                res.Message = command.Parameters["O_MESSAGE"].Value?.ToString();
                res.Details = command.Parameters["O_ERR_DETAILS"].Value?.ToString();

                OracleBlob v_ImageDataBlob = (OracleBlob)command.Parameters["v_ImageData"].Value;

                byte[] v_ImageData = null;
                if (v_ImageDataBlob != null && v_ImageDataBlob.Length > 0)
                {
                    v_ImageData = v_ImageDataBlob.Value;
                }
                pht.ImageData = v_ImageData;

                pht.FileName = command.Parameters["v_FileName"].Value?.ToString();
                pht.ContentType = command.Parameters["v_ContentType"].Value?.ToString();


                await command.Connection.CloseAsync();
                command.Dispose();
                //------------------------------------------------------------------------------------------------------


                //------------------------------------------------------------------------------------------------------
                command = await _dbHelper.CreateStoredProcedureCommandAsync("GET_A_USERS_SIGNATURE");

                _dbHelper.AddInputParameter(command, "v_Id", OracleDbType.Int32, id);

                _dbHelper.AddOutputParameter(command, "O_STATUS", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(command, "O_MESSAGE", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(command, "O_ERR_DETAILS", OracleDbType.Varchar2);

                _dbHelper.AddOutputParameter(command, "v_ImageData", OracleDbType.Blob);
                _dbHelper.AddOutputParameter(command, "v_FileName", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(command, "v_ContentType", OracleDbType.Varchar2);

                await _dbHelper.NccExecuteNonQueryAsync(command); //using non close con..


                res.Status = command.Parameters["O_STATUS"].Value?.ToString();
                res.Message = command.Parameters["O_MESSAGE"].Value?.ToString();
                res.Details = command.Parameters["O_ERR_DETAILS"].Value?.ToString();

                OracleBlob v_ImageDataBlob1 = (OracleBlob)command.Parameters["v_ImageData"].Value;

                byte[] v_ImageData1 = null;
                if (v_ImageDataBlob1 != null && v_ImageDataBlob1.Length > 0)
                {
                    v_ImageData1 = v_ImageDataBlob1.Value;
                }
                sig.ImageData = v_ImageData1;

                sig.FileName = command.Parameters["v_FileName"].Value?.ToString();
                sig.ContentType = command.Parameters["v_ContentType"].Value?.ToString();


                await command.Connection.CloseAsync();
                command.Dispose();
                //------------------------------------------------------------------------------------------------------


                //------------------------------------------------------------------------------------------------------
                command = await _dbHelper.CreateStoredProcedureCommandAsync("GET_APPLICANTS_APPLICATIONS");

                _dbHelper.AddInputParameter(command, "v_Id", OracleDbType.Int32, id);
                _dbHelper.AddOutputParameter(command, "v_Applications", OracleDbType.Varchar2);

                await _dbHelper.ExecuteNonQueryAsync(command);

                basic.AppliedPosts = command.Parameters["v_Applications"].Value?.ToString();

                command.Dispose();
                //------------------------------------------------------------------------------------------------------
            }
            catch (Exception e)
            {
                res.Status = "FAILED";
                res.Message = "SOME ERROR OCCURED";
            }
            viewModel.UsersBasic = basic;
            viewModel.UsersDetails = details;
            viewModel.UsersEducationalQualifications = edu;
            viewModel.UsersExperiences = exp;
            viewModel.UsersLanguageproficiencies = lang;
            viewModel.UsersPersonalCertifications = cir;
            viewModel.UsersReferences = refr;
            viewModel.UsersPhoto = pht;
            viewModel.UsersSignature = sig;
            viewModel.BaseResponse = res;

            return viewModel;
        }
    }
}
