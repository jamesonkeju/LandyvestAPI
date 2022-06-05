using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Services
{
    public class Exam
    {
        public string Surname { get; set; }
        public IFormFile file { get; set; }
    }
}
