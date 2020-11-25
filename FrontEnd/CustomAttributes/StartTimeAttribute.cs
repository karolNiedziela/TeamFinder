using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEnd.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class StartTimeAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var dateTimeValue = Convert.ToDateTime(value);

            if (dateTimeValue <= DateTime.Now.Date || dateTimeValue >= DateTime.Now.AddDays(7))
                return false;

            return true;
        }
    }
}
