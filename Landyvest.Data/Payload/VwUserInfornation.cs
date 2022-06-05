using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Landyvest.Utilities.Common;

namespace Landyvest.Data.Payload
{
    public class VwUserInfornation : MessageOut
    {
        public long user_id { get; set; }
        public long employee_id { get; set; }
        public string employee_number { get; set; }
        public long employee_contract_id { get; set; }
        //public CompanyDTO CompanyProfile { get; set; }
        public long? grade_id { get; set; }
        public DateTime? confirmation_date { get; set; }
        public string serial_no { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string full_name => last_name + " " + first_name;
        public string other_name { get; set; }
        public string department { get; set; }
        public long department_id { get; set; }
        public string email { get; set; }
        public string phone_number { get; set; }
        public bool email_confirmed { get; set; }
        public string session_token { get; set; }
        public string jwt_token { get; set; }
        public string user_token { get; set; }
        public int company_id { get; set; }
        public int LicenseUsuage { get; set; }
        public int LicenseCount { get; set; }
        public string company_name { get; set; }
        public int sub_id { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsSuperAdmin { get; set; }
        public bool IsTenantAdmin { get; set; }
        public bool IsActiveBySysOrAdmin { get; set; }
        public List<string> lstPermissions { get; set; }

    }
}
