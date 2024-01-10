using App.Application.Interfaces.Shared;
using System.Reflection;

namespace App.Infrastructure.Shared
{
    public class CompareObjectService : ICompareObject
    {
        public bool Compare<T>(T o1, T o2)
        {
            bool result = true;
            foreach (PropertyInfo pi1 in o1.GetType().GetProperties())
            {
                PropertyInfo pi2 = o2.GetType().GetProperty(pi1.Name);

                var pt1 = pi1.PropertyType;
                var pt2 = pi2.PropertyType;

                if (!pt1.Equals(pt2))
                {
                    result = false;
                    break;
                }

                var t = Nullable.GetUnderlyingType(pt1) ?? pt1;

                var v1 = pi1.GetValue(o1, null);
                var v2 = pi2.GetValue(o2, null);

                var sv1 = (v1 == null) ? null : Convert.ChangeType(v1, t);
                var sv2 = (v2 == null) ? null : Convert.ChangeType(v2, t);

                if (sv1 == null | sv2 == null)
                {
                    if (sv1 != sv2)
                    {
                        result = false;
                        break;
                    }
                }
                else
                {
                    if (!sv1.Equals(sv2))
                    {
                        result = false;
                        break;
                    }
                }
            }
            return result;
        }
    }
}
