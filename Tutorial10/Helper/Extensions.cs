using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tutorial10.Helper
{
    public static class Extensions
    {

        public static int CaclculateAge(this DateTime birthday)
        {
            var age = DateTime.Today.Year - birthday.Year;
            if (birthday.AddYears(age) != DateTime.Today)
                age--;

            return age;
        }
    }
}
