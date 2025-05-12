using ERecruitmentSystem02.Common.HelperClasses;
using ERecruitmentSystem02.Models.Domain;
using ERecruitmentSystem02.Models.Shared;
using ERecruitmentSystem02.Models.View;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using System.Transactions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ERecruitmentSystem02.Repository
{
    public class DataInsert : IDataInsert
    {
        private readonly DataBaseHelper _dbHelper;
        private int _userId;

        RegistrationPageViewModel _pageViewModel = new RegistrationPageViewModel();
        BaseResponse _baseResponse = new BaseResponse();

        public DataInsert(IConfiguration config)
        {
            _dbHelper = new DataBaseHelper(config);
        }
        public async Task<BaseResponse> InsertApplicant(RegistrationPageViewModel registrationPageViewModel)
        {
            _pageViewModel = registrationPageViewModel;
            var _applicantsBasics = _pageViewModel.UsersBasic;
            var _applicantsDetails = _pageViewModel.UsersDetails;
            var _applicantsEduQualifications = _pageViewModel.UsersEducationalQualifications;
            var _applicantsExperiences = _pageViewModel.UsersExperiences;
            var _applicantsLangProficiencies = _pageViewModel.UsersLanguageproficiencies;
            var _applicantsPesCertification = _pageViewModel.UsersPersonalCertifications;
            var _applicantsReferences = _pageViewModel.UsersReferences;
            var _applicantsPhoto = _pageViewModel.UsersPhoto;
            var _applicantsSignature = _pageViewModel.UsersSignature;

            using var connection = await _dbHelper.GetConnectionAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                // Insert Basic Information
                var command = await _dbHelper.CreateStoredProcedureCommandAsync("SET_APPLICANTS_BASIC", connection, transaction);

                _dbHelper.AddOutputParameter(command, "v_Id", OracleDbType.Int32);
                _dbHelper.AddOutputParameter(command, "v_Status", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(command, "v_Message", OracleDbType.Varchar2);
                _dbHelper.AddOutputParameter(command, "v_Err_Details", OracleDbType.Varchar2);

                _dbHelper.AddInputParameter(command, "v_Name", OracleDbType.Varchar2, _applicantsBasics.Name);
                _dbHelper.AddInputParameter(command, "v_Sex", OracleDbType.Varchar2, _applicantsBasics.Sex);
                _dbHelper.AddInputParameter(command, "v_DateOfBirth", OracleDbType.Date, _applicantsBasics.DateOfBirth.ToDateTime(TimeOnly.MinValue));
                _dbHelper.AddInputParameter(command, "v_Nationality", OracleDbType.Varchar2, _applicantsBasics.Nationality);
                _dbHelper.AddInputParameter(command, "v_Email", OracleDbType.Varchar2, _applicantsBasics.Email);
                _dbHelper.AddInputParameter(command, "v_NationalIdCardNo", OracleDbType.Varchar2, _applicantsBasics.NationalIdCardNo);
                _dbHelper.AddInputParameter(command, "v_ContactMobileNo", OracleDbType.Varchar2, _applicantsBasics.ContactMobileNo);
                _dbHelper.AddInputParameter(command, "v_Password", OracleDbType.Varchar2, _applicantsBasics.Password);

                await _dbHelper.NccExecuteNonQueryAsync(command);

                _baseResponse.Status = command.Parameters["v_Status"].Value?.ToString();
                _baseResponse.Message = command.Parameters["v_Message"].Value?.ToString();
                _baseResponse.Details = command.Parameters["v_Err_Details"].Value?.ToString();

                if (_baseResponse.Status == "SUCCESS")
                {
                    _userId = ((OracleDecimal)command.Parameters["v_Id"].Value).ToInt32();
                    command.Dispose();
                }
                else
                {
                    command.Dispose();
                    throw new Exception($"Error inserting basic information: {_baseResponse.Message}");
                }

                // Insert Details
                command = await _dbHelper.CreateStoredProcedureCommandAsync("SET_APPLICANTS_DETAILS", connection, transaction);

                _applicantsDetails.UserId = _userId;

                _dbHelper.AddInputParameter(command, "v_PresentAreaOrVillageOrHouseOrRoad", OracleDbType.Varchar2, _applicantsDetails.PresentAreaOrVillageOrHouseOrRoad);
                _dbHelper.AddInputParameter(command, "v_PresentPostOffice", OracleDbType.Varchar2, _applicantsDetails.PresentPostOffice);
                _dbHelper.AddInputParameter(command, "v_PresentPoliceStation", OracleDbType.Varchar2, _applicantsDetails.PresentPoliceStation);
                _dbHelper.AddInputParameter(command, "v_PresentPostalCode", OracleDbType.Int32, _applicantsDetails.PresentPostalCode);
                _dbHelper.AddInputParameter(command, "v_PresentDistrict", OracleDbType.Varchar2, _applicantsDetails.PresentDistrict);
                _dbHelper.AddInputParameter(command, "v_PermanentAreaOrVillageOrHouseOrRoad", OracleDbType.Varchar2, _applicantsDetails.PermanentAreaOrVillageOrHouseOrRoad);
                _dbHelper.AddInputParameter(command, "v_PermanentPostOffice", OracleDbType.Varchar2, _applicantsDetails.PermanentPostOffice);
                _dbHelper.AddInputParameter(command, "v_PermanentPoliceStation", OracleDbType.Varchar2, _applicantsDetails.PermanentPoliceStation);
                _dbHelper.AddInputParameter(command, "v_PermanentPostalCode", OracleDbType.Int32, _applicantsDetails.PermanentPostalCode);
                _dbHelper.AddInputParameter(command, "v_PermanentDistrict", OracleDbType.Varchar2, _applicantsDetails.PermanentDistrict);
                _dbHelper.AddInputParameter(command, "v_HomeDistrict", OracleDbType.Varchar2, _applicantsDetails.HomeDistrict);
                _dbHelper.AddInputParameter(command, "v_CareerObjective", OracleDbType.Varchar2, _applicantsDetails.CareerObjective);
                _dbHelper.AddInputParameter(command, "v_MaritalStatus", OracleDbType.Varchar2, _applicantsDetails.MaritalStatus);
                _dbHelper.AddInputParameter(command, "v_Religion", OracleDbType.Varchar2, _applicantsDetails.Religion);
                _dbHelper.AddInputParameter(command, "v_FathersName", OracleDbType.Varchar2, _applicantsDetails.FathersName);
                _dbHelper.AddInputParameter(command, "v_MothersName", OracleDbType.Varchar2, _applicantsDetails.MothersName);
                _dbHelper.AddInputParameter(command, "v_Spouse", OracleDbType.Varchar2, _applicantsDetails.Spouse);
                _dbHelper.AddInputParameter(command, "v_UserId", OracleDbType.Int32, _applicantsDetails.UserId);
                _dbHelper.AddInputParameter(command, "v_HasExperience", OracleDbType.Varchar2, _applicantsDetails.HasExperience);
                _dbHelper.AddInputParameter(command, "v_ComputerLiteracy", OracleDbType.Varchar2, _applicantsDetails.ComputerLiteracy);

                await _dbHelper.NccExecuteNonQueryAsync(command);
                command.Dispose();

                //Photo Insert 

               

                // Add other inserts (e.g., educational qualifications, experiences, etc.) here, using the same pattern.
                //
                string[] nameOfExamination = new string[_applicantsEduQualifications.Count];
                string[] groupOrSubject = new string[_applicantsEduQualifications.Count];
                string[] divisionOrClassOrGrade = new string[_applicantsEduQualifications.Count];
                double[] gpaOrCgpa = new double[_applicantsEduQualifications.Count];
                double[] gpaOutOf = new double[_applicantsEduQualifications.Count];
                string[] rollNo = new string[_applicantsEduQualifications.Count];
                string[] boardOrUniversity = new string[_applicantsEduQualifications.Count];
                int[] passingYear = new int[_applicantsEduQualifications.Count];
                int[] userId = new int[_applicantsEduQualifications.Count];

                for (int i = 0; i < _applicantsEduQualifications.Count; i++)
                {
                    var edu = _applicantsEduQualifications[i];
                    edu.UserId = _userId;

                    nameOfExamination[i] = edu.NameOfExamination;
                    groupOrSubject[i] = edu.GroupOrSubject;
                    divisionOrClassOrGrade[i] = edu.DivisionOrClassOrGrade;
                    gpaOrCgpa[i] = edu.GpaOrCgpa ?? 0;
                    gpaOutOf[i] = edu.GpaOutOf ?? 0;
                    rollNo[i] = edu.RollNo;
                    boardOrUniversity[i] = edu.BoardOrUnicersity;
                    passingYear[i] = edu.PassingYear ?? 0;
                    userId[i] = edu.UserId;
                }


                command = await _dbHelper.CreateStoredProcedureCommandAsync("SET_APPLICANTS_EDU_QUALIFICATIONS", connection, transaction);

                command.ArrayBindCount = _applicantsEduQualifications.Count;

                        _dbHelper.AddInputParameter(command, "v_NameOfExamination", OracleDbType.Varchar2, nameOfExamination);
                        _dbHelper.AddInputParameter(command, "v_GroupOrSubject", OracleDbType.Varchar2, groupOrSubject);
                        _dbHelper.AddInputParameter(command, "v_DivisionOrClassOrGrade", OracleDbType.Varchar2, divisionOrClassOrGrade);
                        _dbHelper.AddInputParameter(command, "v_GpaOrCgpa", OracleDbType.BinaryDouble, gpaOrCgpa);
                        _dbHelper.AddInputParameter(command, "v_GpaOutOf", OracleDbType.BinaryDouble, gpaOutOf);
                        _dbHelper.AddInputParameter(command, "v_RollNo", OracleDbType.Varchar2, rollNo);
                        _dbHelper.AddInputParameter(command, "v_BoardOrUnicersity", OracleDbType.Varchar2, boardOrUniversity);
                        _dbHelper.AddInputParameter(command, "v_PassingYear", OracleDbType.Int32, passingYear);
                        _dbHelper.AddInputParameter(command, "v_UserId", OracleDbType.Int32, userId);

                        await _dbHelper.NccExecuteNonQueryAsync(command);
                command.Dispose();

                //
                string[] designation = new string[_applicantsExperiences.Count];
                string[] nameOfOrganization = new string[_applicantsExperiences.Count];
                string[] jobType = new string[_applicantsExperiences.Count];
                string[] responsibilities = new string[_applicantsExperiences.Count];
                DateTime?[] joiningDate = new DateTime?[_applicantsExperiences.Count];
                DateTime?[] releaseDate = new DateTime?[_applicantsExperiences.Count];
                int[] userId0 = new int[_applicantsExperiences.Count];
                string[] continuing = new string[_applicantsExperiences.Count];


                for (int i = 0; i < _applicantsExperiences.Count; i++)
                {
                    var experience = _applicantsExperiences[i];
                    experience.UserId = _userId;

                    designation[i] = experience.Designation;
                    nameOfOrganization[i] = experience.NameOfOrganization;
                    jobType[i] = experience.JobType;
                    responsibilities[i] = experience.Responsibilities;
                    joiningDate[i] = experience.JoiningDate?.ToDateTime(TimeOnly.MinValue); // nullable
                    releaseDate[i] = experience.ReleaseDate?.ToDateTime(TimeOnly.MinValue); // nullable
                    userId0[i] = experience.UserId;
                    continuing[i] = experience.Continuing;
                }


                command = await _dbHelper.CreateStoredProcedureCommandAsync("SET_APPLICANTS_EXPERIENCES", connection, transaction);

                        command.ArrayBindCount = _applicantsExperiences.Count;

                        _dbHelper.AddInputParameter(command, "v_Designation", OracleDbType.Varchar2, designation);
                        _dbHelper.AddInputParameter(command, "v_NameOfOrganization", OracleDbType.Varchar2, nameOfOrganization);
                        _dbHelper.AddInputParameter(command, "v_JobType", OracleDbType.Varchar2, jobType);
                        _dbHelper.AddInputParameter(command, "v_Responsibilities", OracleDbType.Varchar2, responsibilities);
                        _dbHelper.AddInputParameter(command, "v_JoiningDate", OracleDbType.Date, joiningDate);
                        _dbHelper.AddInputParameter(command, "v_ReleaseDate", OracleDbType.Date, releaseDate);
                        _dbHelper.AddInputParameter(command, "v_UserId", OracleDbType.Int32, userId0);
                        _dbHelper.AddInputParameter(command, "v_Continuing", OracleDbType.Varchar2, continuing);

                        await _dbHelper.NccExecuteNonQueryAsync(command);
                command.Dispose();


                string[] languageNames = new string[_applicantsLangProficiencies.Count];
                string[] speaking = new string[_applicantsLangProficiencies.Count];
                string[] reading = new string[_applicantsLangProficiencies.Count];
                string[] listening = new string[_applicantsLangProficiencies.Count];
                string[] writing = new string[_applicantsLangProficiencies.Count];
                int[] userIds = new int[_applicantsLangProficiencies.Count];

                for (int i = 0; i < _applicantsLangProficiencies.Count; i++)
                {
                    var langProfeciency = _applicantsLangProficiencies[i];
                    langProfeciency.UserId = _userId;

                    languageNames[i] = langProfeciency.LanguageName;
                    speaking[i] = langProfeciency.Speaking;
                    reading[i] = langProfeciency.Reading;
                    listening[i] = langProfeciency.Listening;
                    writing[i] = langProfeciency.Writing;
                    userIds[i] = langProfeciency.UserId;
                }


                command = await _dbHelper.CreateStoredProcedureCommandAsync("SET_APPLICANTS_LANG_PROFECIENCIES", connection, transaction);
                    
                        command.ArrayBindCount = _applicantsLangProficiencies.Count;

                        _dbHelper.AddInputParameter(command, "v_LanguageName", OracleDbType.Varchar2, languageNames);
                        _dbHelper.AddInputParameter(command, "v_Speaking", OracleDbType.Varchar2, speaking);
                        _dbHelper.AddInputParameter(command, "v_Reading", OracleDbType.Varchar2, reading);
                        _dbHelper.AddInputParameter(command, "v_Listening", OracleDbType.Varchar2, listening);
                        _dbHelper.AddInputParameter(command, "v_Writing", OracleDbType.Varchar2, writing);
                        _dbHelper.AddInputParameter(command, "v_UserId", OracleDbType.Int32, userIds);

                        await _dbHelper.NccExecuteNonQueryAsync(command);
                command.Dispose();


                foreach (var pesCertification in _applicantsPesCertification)
                {
                    pesCertification.UserId = _userId;
                }

                string[] certification = new string[_applicantsPesCertification.Count];
                string[] nameOfInstitution = new string[_applicantsPesCertification.Count];
                string[] location = new string[_applicantsPesCertification.Count];
                DateTime?[] fromDate = new DateTime?[_applicantsPesCertification.Count];
                DateTime?[] toDate = new DateTime?[_applicantsPesCertification.Count];
                int[] userId1 = new int[_applicantsPesCertification.Count];

                for (int i = 0; i < _applicantsPesCertification.Count; i++)
                {
                    certification[i] = _applicantsPesCertification[i].Certification;
                    nameOfInstitution[i] = _applicantsPesCertification[i].NameOfInstitution;
                    location[i] = _applicantsPesCertification[i].Location;
                    fromDate[i] = _applicantsPesCertification[i].FromDate?.ToDateTime(TimeOnly.MinValue);
                    toDate[i] = _applicantsPesCertification[i].ToDate?.ToDateTime(TimeOnly.MinValue);
                    userId1[i] = _applicantsPesCertification[i].UserId;
                }


                command = await _dbHelper.CreateStoredProcedureCommandAsync("SET_APPLICANTS_PRS_CERTIFICATION", connection, transaction);

                    command.ArrayBindCount = _applicantsPesCertification.Count;

                    _dbHelper.AddInputParameter(command, "v_Certification", OracleDbType.Varchar2, certification);
                    _dbHelper.AddInputParameter(command, "v_NameOfInstitution", OracleDbType.Varchar2, nameOfInstitution);
                    _dbHelper.AddInputParameter(command, "v_Location", OracleDbType.Varchar2, location);
                    _dbHelper.AddInputParameter(command, "v_FromDate", OracleDbType.Date, fromDate);
                    _dbHelper.AddInputParameter(command, "v_ToDate", OracleDbType.Date, toDate);
                    _dbHelper.AddInputParameter(command, "v_UserId", OracleDbType.Int32, userId1);

                    await _dbHelper.NccExecuteNonQueryAsync(command);

                command.Dispose();

                foreach (var reference in _applicantsReferences)
                {
                    reference.UserId = _userId;
                }

                string[] name = new string[_applicantsReferences.Count];
                string[] address = new string[_applicantsReferences.Count];
                string[] contactNo = new string[_applicantsReferences.Count];
                string[] email = new string[_applicantsReferences.Count];
                int[] userId2 = new int[_applicantsReferences.Count];

                for (int i = 0; i < _applicantsReferences.Count; i++)
                {
                    name[i] = _applicantsReferences[i].Name;
                    address[i] = _applicantsReferences[i].Address;
                    contactNo[i] = _applicantsReferences[i].ContactNo;
                    email[i] = _applicantsReferences[i].Email;
                    userId2[i] = _applicantsReferences[i].UserId;
                }


                command = await _dbHelper.CreateStoredProcedureCommandAsync("SET_APPLICANTS_REFERENCES", connection, transaction);

                command.ArrayBindCount = _applicantsReferences.Count;

                        _dbHelper.AddInputParameter(command, "v_Name", OracleDbType.Varchar2, name);
                        _dbHelper.AddInputParameter(command, "v_Address", OracleDbType.Varchar2, address);
                        _dbHelper.AddInputParameter(command, "v_ContactNo", OracleDbType.Varchar2, contactNo);
                        _dbHelper.AddInputParameter(command, "v_Email", OracleDbType.Varchar2, email);
                        _dbHelper.AddInputParameter(command, "v_UserId", OracleDbType.Int32, userId2);

                        await _dbHelper.NccExecuteNonQueryAsync(command);
                command.Dispose();
                //
                _applicantsPhoto.UserId = _userId;

                command = await _dbHelper.CreateStoredProcedureCommandAsync("SET_APPLICANTS_PHOTO", connection, transaction);

                _dbHelper.AddInputParameter(command, "v_ImageData", OracleDbType.Blob, _applicantsPhoto.ImageData);
                _dbHelper.AddInputParameter(command, "v_FileName", OracleDbType.Varchar2, _applicantsPhoto.FileName);
                _dbHelper.AddInputParameter(command, "v_ContentType", OracleDbType.Varchar2, _applicantsPhoto.ContentType);
                _dbHelper.AddInputParameter(command, "v_UserId", OracleDbType.Int32, _applicantsPhoto.UserId);

                await _dbHelper.NccExecuteNonQueryAsync(command);
                command.Dispose();

                _applicantsSignature.UserId = _userId;


                command = await _dbHelper.CreateStoredProcedureCommandAsync("SET_APPLICANTS_SIGNATURE", connection, transaction);

                _dbHelper.AddInputParameter(command, "v_ImageData", OracleDbType.Blob, _applicantsSignature.ImageData);
                _dbHelper.AddInputParameter(command, "v_FileName", OracleDbType.Varchar2, _applicantsSignature.FileName);
                _dbHelper.AddInputParameter(command, "v_ContentType", OracleDbType.Varchar2, _applicantsSignature.ContentType);
                _dbHelper.AddInputParameter(command, "v_UserId", OracleDbType.Int32, _applicantsSignature.UserId);

                await _dbHelper.NccExecuteNonQueryAsync(command);
                command.Dispose();
                //
                // Commit the transaction if all operations succeed
                transaction.Commit();
                return _baseResponse;
            }
            catch (Exception ex)
            {
                // Rollback the transaction in case of an error
                transaction.Rollback();
                _baseResponse.Status = "FAILURE";
                _baseResponse.Message = ex.Message;
                return _baseResponse;
            }

                

        }

        public async Task<BaseResponse> ChangeApplicantPassword(ChangePassword changePassword)
        {
            BaseResponse _baseResponse = new BaseResponse();

            var command = await _dbHelper.CreateStoredProcedureCommandAsync("CHANGE_APPLICANTS_PASSWORD");

            _dbHelper.AddOutputParameter(command, "v_Status", OracleDbType.Varchar2);
            _dbHelper.AddOutputParameter(command, "v_Message", OracleDbType.Varchar2);
            _dbHelper.AddOutputParameter(command, "v_Err_Details", OracleDbType.Varchar2);

            _dbHelper.AddInputParameter(command, "v_Id", OracleDbType.Int32, changePassword.Id);
            _dbHelper.AddInputParameter(command, "v_password", OracleDbType.Varchar2, changePassword.OldPassword);
            _dbHelper.AddInputParameter(command, "v_NewPassword", OracleDbType.Varchar2, changePassword.NewPassword);

            await _dbHelper.ExecuteNonQueryAsync(command);

            _baseResponse.Status = command.Parameters["v_Status"].Value?.ToString();
            _baseResponse.Message = command.Parameters["v_Message"].Value?.ToString();
            _baseResponse.Message = command.Parameters["v_Err_Details"].Value?.ToString();

            command.Dispose();

            return _baseResponse;
        }

        public async Task<BaseResponse> ResetPassword(ForgotPassword forgotPassword)
        {
            var command = await _dbHelper.CreateStoredProcedureCommandAsync("SET_NEW_PASSWORD");

            _dbHelper.AddOutputParameter(command, "v_Status", OracleDbType.Varchar2);
            _dbHelper.AddOutputParameter(command, "v_Message", OracleDbType.Varchar2);
            _dbHelper.AddOutputParameter(command, "v_Err_Details", OracleDbType.Varchar2);

            _dbHelper.AddInputParameter(command, "v_Id", OracleDbType.Int32, forgotPassword.Id);
            _dbHelper.AddInputParameter(command, "v_NewPassword", OracleDbType.Varchar2, forgotPassword.NewPassword);

            await _dbHelper.ExecuteNonQueryAsync(command);

            _baseResponse.Status = command.Parameters["v_Status"].Value?.ToString();
            _baseResponse.Message = command.Parameters["v_Message"].Value?.ToString();
            _baseResponse.Message = command.Parameters["v_Err_Details"].Value?.ToString();

            command.Dispose();

            return _baseResponse;
        }

        public async Task<BaseResponse> InsertApplication(JobApply jobApply)
        {
            BaseResponse _baseResponse = new BaseResponse();
            DateTime applyDate = DateTime.Now.Date;

            var command = await _dbHelper.CreateStoredProcedureCommandAsync("SET_APPLICATION");

            _dbHelper.AddOutputParameter(command, "O_STATUS", OracleDbType.Varchar2);
            _dbHelper.AddOutputParameter(command, "O_MESSAGE", OracleDbType.Varchar2);
            _dbHelper.AddOutputParameter(command, "O_ERR_DETAILS", OracleDbType.Varchar2);

            _dbHelper.AddInputParameter(command, "P_JobID", OracleDbType.Int32, jobApply.JobId);
            _dbHelper.AddInputParameter(command, "P_ApplicantID", OracleDbType.Int32, jobApply.ApplicantId);
            _dbHelper.AddInputParameter(command, "P_ApplyDate", OracleDbType.Date, applyDate);

            await _dbHelper.ExecuteReaderAsync(command);


            _baseResponse.Status = command.Parameters["O_STATUS"].Value?.ToString();
            _baseResponse.Message = command.Parameters["O_MESSAGE"].Value?.ToString();
            _baseResponse.Details = command.Parameters["O_ERR_DETAILS"].Value?.ToString();

            command.Dispose();

            return _baseResponse;
        }
        public async Task<BaseResponse> UpdateApplicant(RegistrationPageViewModel reg)
        {
            var _applicantsEduQualifications = reg.UsersEducationalQualifications;
            BaseResponse res = new BaseResponse();

            try
            {
                var command = await _dbHelper.CreateStoredProcedureCommandAsync("DELETE_USER_EDU_QUALIFICATIONS");
                _dbHelper.AddInputParameter(command, "v_UserId", OracleDbType.Int32, reg.UsersBasic.Id);
                await _dbHelper.ExecuteReaderAsync(command);
                command.Dispose();

                //-------------------------------------------------------------------------------------------

                string[] nameOfExamination = new string[_applicantsEduQualifications.Count];
                string[] groupOrSubject = new string[_applicantsEduQualifications.Count];
                string[] divisionOrClassOrGrade = new string[_applicantsEduQualifications.Count];
                double[] gpaOrCgpa = new double[_applicantsEduQualifications.Count];
                double[] gpaOutOf = new double[_applicantsEduQualifications.Count];
                string[] rollNo = new string[_applicantsEduQualifications.Count];
                string[] boardOrUniversity = new string[_applicantsEduQualifications.Count];
                int[] passingYear = new int[_applicantsEduQualifications.Count];
                int[] userId = new int[_applicantsEduQualifications.Count];

                for (int i = 0; i < _applicantsEduQualifications.Count; i++)
                {
                    var edu = _applicantsEduQualifications[i];
                    edu.UserId = reg.UsersBasic.Id;

                    nameOfExamination[i] = edu.NameOfExamination;
                    groupOrSubject[i] = edu.GroupOrSubject;
                    divisionOrClassOrGrade[i] = edu.DivisionOrClassOrGrade;
                    gpaOrCgpa[i] = edu.GpaOrCgpa ?? 0;
                    gpaOutOf[i] = edu.GpaOutOf ?? 0;
                    rollNo[i] = edu.RollNo;
                    boardOrUniversity[i] = edu.BoardOrUnicersity;
                    passingYear[i] = edu.PassingYear ?? 0;
                    userId[i] = edu.UserId;
                }


                command = await _dbHelper.CreateStoredProcedureCommandAsync("SET_APPLICANTS_EDU_QUALIFICATIONS");

                command.ArrayBindCount = _applicantsEduQualifications.Count;

                _dbHelper.AddInputParameter(command, "v_NameOfExamination", OracleDbType.Varchar2, nameOfExamination);
                _dbHelper.AddInputParameter(command, "v_GroupOrSubject", OracleDbType.Varchar2, groupOrSubject);
                _dbHelper.AddInputParameter(command, "v_DivisionOrClassOrGrade", OracleDbType.Varchar2, divisionOrClassOrGrade);
                _dbHelper.AddInputParameter(command, "v_GpaOrCgpa", OracleDbType.BinaryDouble, gpaOrCgpa);
                _dbHelper.AddInputParameter(command, "v_GpaOutOf", OracleDbType.BinaryDouble, gpaOutOf);
                _dbHelper.AddInputParameter(command, "v_RollNo", OracleDbType.Varchar2, rollNo);
                _dbHelper.AddInputParameter(command, "v_BoardOrUnicersity", OracleDbType.Varchar2, boardOrUniversity);
                _dbHelper.AddInputParameter(command, "v_PassingYear", OracleDbType.Int32, passingYear);
                _dbHelper.AddInputParameter(command, "v_UserId", OracleDbType.Int32, userId);

                await _dbHelper.NccExecuteNonQueryAsync(command);
                command.Dispose();

                res.Status = "SUCCESS";
                res.Message = "UPDATED SUCCESSFULLY";

            }
            catch(Exception e)
            {
                res.Status = "FAILED";
                res.Message = "UPDATED FAILED";
            }

            return res;
        }
    }
}
