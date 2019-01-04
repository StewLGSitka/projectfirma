using System.Collections.Generic;
using System.Linq;

namespace ProjectFirma.Web.Models
{
    public partial class PerformanceMeasureType
    {
        public static List<PerformanceMeasureType> GetAllUserSelectable()
        {
            return All.Where(x => x.IsUserSelectable).ToList();
        }
    }
}