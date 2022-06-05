using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Utilities.Common
{
    public class CommonUtility
    {

        public static string FormatDate(string date)
        {
            string[] breakUp = date.Split(' ');
            string formatDate = breakUp[0];
            string[] breakDate = formatDate.Split('/');
            string dd = "";
            string mm = "";
            string newdate = "";
            if (breakDate[0].Length == 1)
            {
                dd = "0" + breakDate[0];
            }
            else
            {
                dd = breakDate[0];
            }
            if (breakDate[1].Length == 1)
            {
                mm = "0" + breakDate[1];
            }
            else
            {
                mm = breakDate[1];
            }
            newdate = breakDate[2] + "-" + dd + "-" + mm;
            return newdate.ToString().Replace(" 12:00:00 AM", "");
        }


        public static string FormatDateByBackSlash(string date)
        {
            string formatDate = date.Replace(" 12:00:00 AM", "");
            string[] breakDate = formatDate.Split('/');
            string dd = "";
            string mm = "";
            string newdate = "";
            if (breakDate[0].Length == 1)
            {
                dd = "0" + breakDate[0];
            }
            else
            {
                dd = breakDate[0];
            }
            if (breakDate[1].Length == 1)
            {
                mm = "0" + breakDate[1];
            }
            else
            {
                mm = breakDate[1];
            }
            newdate = mm + "/" + dd + "/" + breakDate[2];
            return newdate.ToString().Replace(" 12:00:00 AM", "");
        }


        public static string FormatDateByLowerSlash(string date)
        {
            string formatDate = date;
            string[] breakDate = formatDate.Split('-');
            string dd = "";
            string mm = "";
            string newdate = "";
            if (breakDate[1].Length == 1)
            {
                dd = "0" + breakDate[1];
            }
            else
            {
                dd = breakDate[1];
            }
            if (breakDate[2].Length == 1)
            {
                mm = "0" + breakDate[2];
            }
            else
            {
                mm = breakDate[2];
            }
            newdate = breakDate[0] + "/" + mm + "/" + dd;
            return newdate;
        }
        public static string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public static string GetSHA512(string text)
        {
            ASCIIEncoding UE = new ASCIIEncoding();
            byte[] data = UE.GetBytes(text);
            SHA512 hashString = new SHA512Managed();
            byte[] hashValue = hashString.ComputeHash(data);
            string hex = ByteToString(hashValue);
            return hex;
        }
        public static string ByteToString(byte[] buff)
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary);
        }

        public static string Tokenize(string amount)
        {
            return (amount.Split('.')[0].Replace(",", ""));
        }
    }
    public class StoredProcedureName
    {
        public const string FetchUserPermissionAndRole = "FetchUserPermissionAndRole";
        public const string GenerateRequestNumber = "GenerateRequestNumber";
    }

    public class ProductList
    {
        public const int Cable = 1;
        public const int Data = 2;
        public const int Airtime = 3;
        public const int Electrcity = 4;
        public const int Bet = 5;
        public const int Toll = 6;
        public const int Insurance = 7;
    }
    public class CommonResponseMessage
    {

        public const string Systemunable = "System unable to process your request, please try again";
        public const string RequiredData = "Kindly supply all request before proceeding";
        public const string InsufficientFund = "Insufficient fund, kindly top up and try again";
        public const string InsufficientFundTransaction = "Insufficient fund of {0} to complete your transaction. Kindly top up and try again.";

        public const string minimumVending = "The minimum amount you can vend is  {0} to complete your transaction. Kindly top up and try again.";
        public const string AgentUpdate = "Agent update was successful";
        public const string IncompleteRequirement = "You are required to supply all the important field(s)";
        public const string PasswordNotMatch = "Password and confirm password must match";
        public const string FetchUserPermissionAndRole = "FetchUserPermissionAndRole";

        public const string PasswordChanged = "Password change was successful, please logIn with your new detail";
        public const string LogInSuccessful = "You have logged in successfully";
        public const string DeletedAccount = "Your account is turn off on this portal, kindly contact the system adminstrator";
        public const string disabledAccount = "Your account is disabled on this portal, kindly contact the system adminstrator";
        public const string PendingEmailConfirmation = "You are yet to confirm your email, kindly check your email for the confirmation email";
        public const string InvalidAccount = "Invalid UserName or Password";
        public const string SuccessfulRegistration = "Your registration was successful, kindly login with your credentails";
        public const string ExistingSubjectDescription = "Exist Subject Description was found,please try again";

        public const string LoginNotAllowed = "Your are not allowed to use the web. Kindly log in using the mobile";
        public const string u = "For security reasons, you would be required to change your system generated password";
        public const string InvalidOperation = "An invalid operation was attempted, please try again";
        public const string IllegalOperation = "You are not permitted to perform this operation";
        public const string OperationFailed = "The operation was not successful.";
        public const string DisableAccount = "Your account is not enabled to carry out this operation, please try again";
        public const string minimumPayment = "You operation was aborted, the minimum payable amount is {0}. Kindly increase the amount and try again";

        public const string ForgetPassword = "Please check your email to reset your password";
        public const string SystemPasswordNotMatch = "Your old password didn't match, please try again";
        public const string PreviousPasswordError = "Your can't use your previous password";
        public const string ChangePasswordSuccessful = "Your password changed was successful, a copy of your credentails was sent to your registered email";
        public const string ProfileUpdate = "Your profile update was successfully";

        public const string PasswordMatchError = "Password doesn't match.";
        public const string ExistingPassword = "You can't use any of your old password(s), please try with a new password";
        public const string ControllerRequested = "Please select controller";
        public const string EvidenceComment = "Your evidence comment is required to upload this file";
        public const string ExpiredActivationLink = "Your activation link has expired, please contact the administrator";
        public const string ExpiredResetPasswordLink = "Your password reset link has expired, kindly regenerate any link and try again";
        public const string ValidateAccount = "Your account was activated successful, kindly check your register email for your login credentails";
        public const string Errorconfirming = "Error while confirming your email!";
        public const string LogoutSuccessful = "You have logged out successfully";

        public const string RecordExistBefore = "The {0} record you are trying to create already exists on the system.";
        public const string RecordExistBeforeWithoutParameter = "The record you are trying to create already exists on the system.";
        public const string RecordExistBeforeII = "The {0} record you are trying to {1} already exists on the system.";
        public const string RecordExistBeforeDeactivated = "The {0} record you are trying to create already exists but deactivated on the system. Contact your administrator.";

        public const string AccountCreation = "User account creation was successful";
        public const string PASS = "PASS";
        public const string FAIL = "FAIL";
        public const long ZERO = 0;
        public const string TakenRecord = "{0} : {1} is already taken";
        public const string SelectRecord = "Please select {0} from the dropdown list";
        public const string CannotCreateMoreThanOneRecord = "You cannot create more than one record on this system";
        public const string RecordNotSaved = "Operation Failed. We encountered an error saving your data on the system";
        public const string ExistingCode = "Existing Code was found,please try again";
        public const string RecordSaved = "1";
        public const string RecordNotFetched = "Operation Failed. We encountered an error fetching your data on the system";
        public const string LoginUsernamePasswordNotEmpy = "Username or Password cannot be empty";
        public const string DateRange = "Select start and end date";
        public const string SuccessfulAndUpdated = "1";
        public const string SuccessfulRequest = "100";
        public const string InValidTimeSetUp = "Please Ensure Your EndDate is Greater than your StartDate";
        public const string GenericNullRecord = "The operation was aborted, record model is null";
        public const string GenericEmptyFile = "The operation was aborted, no file(s) to upload";
        public const string GenericRecordCreate = "Your record was saved successfully";
        public const string RecordCreate = "{0} record was saved successfully";
        public const string AccessRequest = "{0} approved successfully";
        public const string GenericRecordUpdate = "Your record was updated successfully";
        public const string RecordUpdate = "{0} record was updated successfully";
        public const string RequestSuccessful = "Your request was saved successfully";

        public const string SuccessfulTransaction = "&#128077; Your transaction was Successful.";

        public const string DealerCreated = "Dealer was created successfully";

        public const string GenericRecordFail = "Your record failed to save";
        public const string RecordFail = "{0} record failed to save";

        public const string GenericOperationAborted = "The operation was aborted, {0} operation was not succesful";
        public const string GenericRecordExisting = "The operation was aborted, record exist";
        public const string RecordExisting = "The operation was aborted, {0} record exist";
        public const string RecordNotExisting = "The operation was aborted, {0} not found";
        public const string RecordNotFound = "Record not found";
        public const string FileNotFound = "File not found";

        public const string RecordUpdateFail = "{0} record failed to update";
        public const string GenericRecordUpdateFail = "Your record failed to update";

        public const string RecordDelete = "{0} record was deleted successfully";
        public const string GenericRecordDelete = "Your record was deleted";
        public const string RecordFailDelete = "{0} record was not deleted";
        public const string LoginSuccessMessage = "Login successful.";
        public const string LoginFailedMessage = "Login unsuccessful.";
        public const string FetchSuccessMessage = "Record fetched successful.";
        public const string FetchFileSuccessMessage = "File fetched successful.";

        public const string FetchFailedMessage = "Record fetch unsuccessful.";
        public const string NoDebit = "Your operation was not successful and account not debited";
        public const string DataNotLoad = "Unable to load package plans at the moment, please try again";

        public const string FileUploadFailedMessage = "File upload unsuccessful.";
        public const string FileUploadSuccessMessage = "File upload successful.";
        public const string FileFreezeAlert = "File has been freezed";
        public const string InternalError = "An inner error occurred while {0} your record {1}. [{2}]  ";
        public const string MissingUser = "The user information was not found or account deactivated.";
        public const string WalletIssue = "The user wallet is not  activated.";

        public const string InvalidRequest = "Your request is invalid, please try again";
        public const string InvalidEffectiveDate = "Your request is invalid, please check your effective date and try again";
        public const string InvalidApprovedAmount = "Invalid Approved Amount, please check your amount and try again";
        public const string InValidLoanType = "Invalid loan type, please check your and try again";
        public const string InValidPaySlip = "Invalid Prior Payslip, please check  and try again";
        public const string InvalidPartRepayment = "Invalid Part Repayment Amount";



        public static string Unauthorized = "Unauthorized access denied";
        public const string VirtualAcctError = "Error occured while creating your virtual account";

        public static string DealerRejected = "Rejected Successfully";
        public static string DealerApproved = "Approved Successfully";
        public static string MailSendingFail = "Mail Could Not Be Sent";

        public static string InvaildSelection = "The Selected value is invaild for the operation";

        // Notifications for FILE UPLOAD
        public const string ChangeSystemGeneratedPassword = "Kindly change your password";
        public const string FILE_UPLOAD_FAILED = "FAIL|";
        public const string NO_FILE_UPLOADED = FILE_UPLOAD_FAILED + "No File Uploaded";
        public const string FILE_SIZE_EXCEEDED = FILE_UPLOAD_FAILED + "File size is larger than the accepted maximum size. Maximum file size is ";
        public const string NO_FILE_CONTENT = FILE_UPLOAD_FAILED + "File contain no content!";
        public const string INVALID_FILE_EXTENSION = FILE_UPLOAD_FAILED + "File Extension Is InValid - Only Upload ";
        public const string FILE_SIZE_UNIT = " MB";
        // END Notifications for FILE UPLOAD

        //Data
        public static string DataSuccessful = " Data bundle {0} valid for {1} at ₦{2} was Successful";
        public static string AirtimeSuccessful = "Airtime Topup of ₦{0} was Successful";
        public static string BetSuccessful = "Bet Account Topup of ₦{0} was Successful for {1}";
        public static string PostpaidSuccessful = " Postpaid Transaction with amount of ₦{0} was Successful";
        public static string PrepaidSuccessful = " Prepaid Transaction with amount of ₦{0} was Successful with token {1}";
        public static string InsuranceSuccessful = "Insurance payment for {0} at  ₦{1} was Successful with {2}";

        //Cable

        public static string CableSuccessful = " Cable Subscription {0} valid for {1} month at ₦{2} was Successful";
        public static string StarTimesSuccessful = " Cable Subscription {0} at ₦{1} was Successful";

        //Toll
        public static string TollSuccessful = "Toll Payment of ₦{0} was Successful";
        public const string DataRequired = "Please select a validate start and end date";
        public const string DatePeriodCheck = "Your start date can't be greater than your end date";

        //Reversal

        public static string ReversalSuccessful = "Dealer Reversal was successful";




        //  Sending of Emails
        public const string _smtpusername = "SMTPNAME";
        public const string _smtpHost = "SMTPHOST";
        public const string _emailFrom = "SMTPFROM";
        public const string _smtpEnableSsl = "SMTPENABLESSL";
        public const string _smtppassword = "SMTPPASSWORD";
        public const string _smtpPort = "SMTPPORT";
        public const string _SenderId = "SMTPSENDER";
        public const string SMTP_LOOKUP = "SMTP";


        public const string Failed = "300";

        public const int PermissionParentId = 0;
    }


    public class ShagoPayServiceCode
    {
        public const string BET = "BEV";
        public const string BET_VEND = "BEP";
        public const string INSURANCE = "LUL";
        public const string INSURANCE_VEND = "LUP";
        public const string AIRTIME = "QAB";
        public const string DATA = "VDA";
        public const string DATA_CODE = "BDA";
        public const string TOLL = "LEV";
        public const string TOLL_TYPE = "LCC";
        public const string TOLL_TYPE_LEP = "LEP";
        public const string AIRTIME_VTU = "VTU";
        public const string ELECTRCITY = "AOV";
        public const string CABLE_CODE = "GDS";
        public const string CABLE = "GDB";
        public const string DISCO_VEND = "AOB";

    }
    public static class DefineRole
    {
        public const string Administrator = "3951b0f5-cf16-4007-be43-b260171f8797";
        public const string Super_Administrator = "f893c609-4974-4078-a5a7-2efdbafebd54";
        public const string Customer = "2c0935dc-e157-44cc-900b-57c51a9ac505";
        public const string Dealer = "5507d0a9-4dbc-41f6-83ae-832939b1bf4d";
        public const string Sub_Dealer = "68a1511c-0b83-4fa8-ba16-08fdb8d2e876";


    }


    public static class DefineRoleNames
    {
        public const string Administrator = "Administrator";
        public const string Account_Manager = "Account Manager";
        public const string Business_Manager = "Business Manager";
        public const string Auditor = "Auditor";



    }

    public class applicationStatus {

        public static string checkStatus (string Id)
        {
             if (Id == "1")
            {
                return "Pending";
            };

            if (Id == "2")
            {
                return "Business Manager";
            };
            if (Id == "3")
            {
                return "Head Manager Review";
            };

            if (Id == "4")
            {
                return "Approved";
            };
            if (Id == "5")
            {
                return "Rejected";
            };

            if (Id == "6")
            {
                return "Failed";
            };

            return "N/A";
        }
    }
    public class OperationReqObj
    {
        private long _id;
        public OperationReqObj(long id)
        {
            _id = id;
        }

        public string operation_type => _id > 0 ? OperationType.Update : OperationType.Save;
        public string fail_request_type => _id > 0 ? RequestTypes.UpdateRequest_Fail : RequestTypes.CreatedRequest_Fail;
        public string created_request_type => _id > 0 ? RequestTypes.UpdateRequest : RequestTypes.CreateRequest;
        public string fail_record => _id > 0 ? CommonResponseMessage.RecordUpdateFail : CommonResponseMessage.RecordFail;
        public string save_record => _id > 0 ? CommonResponseMessage.RecordUpdate : CommonResponseMessage.RecordCreate;
        public string delete_record => _id > 0 ? CommonResponseMessage.RecordDelete : CommonResponseMessage.RecordDelete;
    }
    #region Add Activity Log Description
    public class RequestTypes
    {
        public const string CreateRequest = "Created a new request.";
        public const string CreatedRequest_Fail = "Unable to create a new request.";

        public const string UpdateRequest = "Updated an existing request.";
        public const string UpdateRequest_Fail = "Unable to update an existing request.";

        public const string DeleteRequest = "Deleted an existing request.";
        public const string DeleteRequest_Fail = "Unable to delete  an existing request.";

        public const string ViewRequest = "Viewed  an existing request";
        public const string FileUploadCollaboration = "Upload file.";

        public const string LoginRequest = "Login In User request.";
        public const string LoginRequest_Fail = "Unable to login a user request.";


        public const string CreateUpdateRequest = "Create/Update requests.";
        public const string CreateUpdateRequest_Fail = "Unable to Create/Update requests.";

        public const string DocumentUpload = "Document file upload.";

    }
    #endregion
    public class OperationType
    {
        public const string Reset_Password = "Reset Password";
        public const string Save = "Save";
        public const string Authenticate = "Authenticate";
        public const string Create = "Create";
        public const string Update = "Update";
        public const string Delete = "Delete";
        public const string Fetch = "Fetch";
        public const string Pending = "Pending";
        public const long RecordEqualToOne = 1;
        public const long RecordEqualToZero = 0;
        public const string Logging_And_Updating = "Logging and Updating";
        public const string Toggle = "Toggle";

    }

    public class DefineRoles
    {
       
        public const string CustomerRole = "2c0935dc-e157-44cc-900b-57c51a9ac505";
        public const string SuperAdministratorRole = "3951b0f5-cf16-4007-be43-b260171f8797";
        public const string DealerRole = "5507d0a9-4dbc-41f6-83ae-832939b1bf4d";
        public const string SubDealerRole = "68a1511c-0b83-4fa8-ba16-08fdb8d2e876";
        public const string AuditorRole = "f893c609-4974-4078-a5a7";
        public const string AccountManagerRole = "f893c609-4974-4078-a5a7-2efdbafeb23";
        public const string FinanceRole = "f893c609-4974-4078-a5a7-2efdbafebd23";
        public const string AdministratorRole = "f893c609-4974-4078-a5a7-2efdbafebd54";
        public const string HeadAccountManagerRole = "f893c609-4974-4078-a5a7-2efdbafebd23";

        public const string SuperAdmin = "Super Administrator";
        public const string Administratror = "Administrator";
        public const string Auditor = "Auditor";
        public const string AccountManager = "Account Manager";
        public const string Finance = "Finance";
        public const string HeadAccountManager = "Head Account Manager";
        public const string Dealer = "Dealer";
        public const string SubDealer = "Sub Dealer";
        public const string Customer = "CUSTOMER";
    }

    public class ShagoPaymentType
    {
        public static string TOLL_TYPE = "LCC";
        public static string TOLL_SERVICE_CODE = "LEV";
        public static string INSURANCE_SERVICE_CODE = "LUL";


    }
    public class DISCOS
    {
        public static string EEDC = "EEDC";
        public static string EKEDC = "EKEDC";
        public static string IKEDC = "IKEDC";
        public static string IBEDC = "IBEDC";

    }
  
    public class ACCOUNTTYPE
    {
        public static string POST_PAID = "2";
        public static string PRE_PAID = "1";
    }

    public class DealerCategory
    {
        public static string Individual = "Individual";
        public static string Cooperate = "Cooperate";

    }

    public class DebitLocation
    {
        public static string Wallet = "1";
        public static string Commission = "2";
    }

    public class MonnifyTransactionResponses
    {
        public static string SUCCESSFUL_TRANSACTION = "SUCCESSFUL_TRANSACTION";
        public static string FAILED_TRANSACTION = "FAILED_TRANSACTION";
    }



}
