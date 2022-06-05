using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Utilities.Enums
{
    public enum Roles
    {
        [Display(Name = "Super Administrator")]
        SuperAdmin,
        [Display(Name = "Administrator")]
        Administratror,
        [Display(Name = "Auditor")]
        Auditor,
        [Display(Name = "Head Account Manager")]
        AccountManager,
        [Display(Name = "Finance")]
        Finance,
        [Display(Name = "Head Account Manager")]
        HeadAccountManager
    }
}
