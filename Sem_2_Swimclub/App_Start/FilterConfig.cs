using System.Web;
using System.Web.Mvc;

namespace Sem_2_Swimclub
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
