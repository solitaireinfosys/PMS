using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMS.APP.Helper
{
    public static class EmptyOrNullValidation
    {
        public static bool HasEmptyFields(List<string> _list)
        {
            if (_list.Count > 0)
            {
                foreach (var s in _list)
                    if (HasEmptyFields(s))
                        return true;
            }

            return false;
        }

        public static bool HasEmptyFields(string s)
        {
            return String.IsNullOrWhiteSpace(s);
        }

        public static bool IsAnyNullOrEmpty(object obj)
        {
            if (Object.ReferenceEquals(obj, null))
                return false;

            return obj.GetType().GetProperties()
                .Any(x => IsNullOrEmpty(x.GetValue(obj)));
        }

        private static bool IsNullOrEmpty(object value)
        {
            if (Object.ReferenceEquals(value, null))
                return false;

            //var type = value.GetType();
            //bool _return =  type.IsValueType
            //    && Object.Equals(value, Activator.CreateInstance(type));
            return true;
        }
    }
}
