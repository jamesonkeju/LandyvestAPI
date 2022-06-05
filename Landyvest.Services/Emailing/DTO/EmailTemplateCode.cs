using System;
using System.Collections.Generic;
using System.Text;

namespace Landyvest.Services.Emailing.DTO
{
    public static class EmailTemplateCode
    {
        public const string Landyvest_TOKEN = "Landyvest_TOKEN";
        public const string ACCOUNT_CREATION = "ACCOUNT_CREATION";
        public const string ACCOUNT_VERIFICATION = "ACCOUNT_VERIFICATION";
        public const string ACCOUNT_CREATION_CHANGE_PASSWORD = "ACCOUNT_CREATION_CHANGE_PASSWORD";
        public const string PASSWORD_UPDATE = "PASSWORD_UPDATE";
        public const string FORGOT_PASSWORD = "FORGOT_PASSWORD";
        public const string AIRTIME_PURCAHSE = "AIRTIME_PURCAHSE";
        public const string ELECTRICTY_PURCHASE = "ELECTRICTY_PURCHASE";
        public const string DATA_PURCAHSE = "DATA_PURCAHSE";
        public const string BET_PURCAHSE = "BET_PURCAHSE";
        public const string TOLL_PURCAHSE = "TOLL_PURCAHSE";
        public const string USER_CHANGE_PASSWORD = "USER_CHANGE_PASSWORD";
        public const string EMAIL_NOFITICATION = "EMAIL_NOFITICATION";
        public const string RESPONDER_EMAIL_NOTIFICATION = "RESPONDER_EMAIL_NOTIFICATION";
        public const string REPORTERS_INCIDENT_UPDATE_NOFITICATION = "REPORTERS_INCIDENT_UPDATE_NOFITICATION";
        public const string NEW_MAIL_REPORTING_NOTIFICATION = "NEW_MAIL_REPORTING_NOTIFICATION";
        public const string RESPONDER_REPORT_NOTIFICATION = "RESPONDER_REPORT_NOTIFICATION";
        public const string MAIL_REPORTING_UPDATE = "MAIL_REPORTING_UPDATE";
        public const string RESPONDER_ACCEPT_REQUEST = "RESPONDER_ACCEPT_REQUEST";
        public const string RESPONDER_REJECT_REQUEST = "RESPONDER_REJECT_REQUEST";
        public const string RESOLUTION_EMAIL_NOTIFICATION = "RESOLUTION_EMAIL_NOTIFICATION";
        public const string ESCALATE_PENDING_INCIDENT_TO_EXECUTIVE_AND_SITURATION_MANAGER = "ESCALATE_PENDING_INCIDENT_TO_EXECUTIVE_AND_SITURATION_MANAGER";
        public const string ESCALATE_PENDING_UNACKNOWLEDGED_INCIDENT_TO_EXECUTIVE_AND_SITURATION_MANAGER = "ESCALATE_PENDING_UNACKNOWLEDGED_INCIDENT_TO_EXECUTIVE_AND_SITURATION_MANAGER";
        public const string CLOSED_EMAIL_NOTIFICATION = "CLOSED_EMAIL_NOTIFICATION";
        public const string AUTHORIZER_EMAIL_NOTIFICATION = "AUTHORIZER_EMAIL_NOTIFICATION";
        public const string REPORTER_NOTIFICATION = "REPORTER_NOTIFICATION";
        public const string EXECUTIVE_COMMENT_EMAIL_NOTIFICATION = "EXECUTIVE_COMMENT_EMAIL_NOTIFICATION";
        public const string EMAIL_BROADCAST_ALERT = "EMAIL_BROADCAST_ALERT";
        public const string NEW_SUBSCRIBER_EMAIL_NOTIFICATION = "NEW_SUBSCRIBER_EMAIL_NOTIFICATION";
        public const string NEW_DEALER_REQUEST_NOTIFICATION = "NEW_DEALER_REQUEST_NOTIFICATION";
        public const string APPROVER_REQUEST_NOTIFICATION = "APPROVER_REQUEST_NOTIFICATION";
        public const string DEALER_SUCCESSFULLY_CREATED = "DEALER_SUCCESSFULLY_CREATED";
        public const string DECLINE_DEALER_REQUEST_NOTIFICATION = "DECLINE_DEALER_REQUEST_NOTIFICATION";
        public const string CABLE_SUBSCRIPTION_PURCAHSE = "CABLE_SUBSCRIPTION_PURCAHSE";
        public const string FUND_DEALER_REQUEST = "FUND_DEALER_REQUEST";
        public const string FUND_DEALER_REQUEST_APPROVED = "FUND_DEALER_REQUEST_APPROVED";

        public const string MONNIFY_SUCCESSFUL_TRANSACTION = "MONNIFY_SUCCESSFUL_TRANSACTION";


    }

    //public class EmailTokenViewModel
    //{
    //    public string TokenName { get; set; }
    //    public string Token { get; set; }
    //    public string TokenValue { get; set; }

    //}

    public class EmailTokenConstants
    {

        /// <summary>
        /// User details constants
        /// </summary>
        public const string USERNAME = "#Username";
        public const string TOKEN = "#Token";
        public const string TransactionDate = "#TransactionDate";
        public const string Reference = "#Reference";
        public const string ServiceType = "#ServiceType";
        public const string ServiceDescription = "#ServiceDescription";
        public const string Amount = "#Amount";
        public const string AgentName = "#AgentName";
        public const string Total = "#Total";
        public const string SubscriptionType = "#SubscriptionType";
        public const string SmartCard = "#SmartCard";

        public const string MeterNo = "#MeterNo";
        public const string Token = "#Token";
        public const string Unit = "#Unit";
        public const string MeterType = "#MeterType";
        public const string Customer = "#Customer";
        public const string Sender = "#Sender";
        public const string BeneficiaryAccountNo = "#BeneficiaryAccountNo";
        public const string BeneficiaryBank = "#BeneficiaryBank";
        public const string BeneficiaryName = "#BeneficiaryName";
        public const string SourceAccountNo = "#SourceAccountNo";
        public const string SourceBank = "#SourceBank";




        public const string RequestAmount = "#RequestAmount";
        public const string RequestedBy = "#RequestedBy";
        public const string DateRequested = "#DateRequested";

        public const string AuditorComment = "#AuditorComment";
        public const string Approved = "#Approved";
        public const string ApprovedAmount = "#ApprovedAmount";
        public const string DateApproved = "#DateApproved";



        public const string EMAIL = "#Email";
        public const string SUBMISSION_DATE = "#SubmittedDate";
        public const string PASSWORD = "#Password";
        public const string PORTALNAME = "#PortalName";
        public const string FULLNAME = "#Name";
        public const string STATUS = "#Status";
        public const string DURATION = "#Duration";
        public const string LogoURL = "#LogoUrl";
        public const string ACCOUNT_VERIFICATION_LINK = "#Link";
        public const string CASE_NUMBER = "#CaseNumber";
        public const string LGA = "#LGA";
        public const string LOCATION = "#LOCATION";
        public const string INCIDENTTYPE = "#IncidentType";
        public const string RESPONDERNAME = "#Responder";
        public const string PICKUPDATE = "#PickupDate";
        public const string AlertExecutive = "#AlertExecutive";
        public const string Now = "#Now";
        public const string COMMENT = "#Comment";
        public const string INCIDENTSTATUS = "#IncidentStatus";
        public const string MESSAGE = "#Message";
        public const string REQUESTNO = "#RequestNo";
        // Forgot Password
        public const string URL = "#URL";


        /// <summary>
        /// Ticket details constants
        /// </summary>
        public const string REQUESTID = "[[REQUESTID]]";
        public const string DEPARTMENT = "[[DEPARTMENT]]";
        public const string DATECREATED = "[[DATECREATED]]";
        public const string REQUESTERNAME = "[[REQUESTERNAME]]";
        public const string DATERESOLVED = "[[DATERESOLVED]]";
        public const string DATEPUTONHOLD = "[[DATEPUTONHOLD]]";
        public const string RESPONSE = "[[RESPONSE]]";
        public const string FEEDBACK = "[[FEEDBACK]]";


    }


}
