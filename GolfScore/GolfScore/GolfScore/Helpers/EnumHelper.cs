using System;
using System.Collections.Generic;
using System.Text;

namespace TeeScore.Helpers
{
    public class EnumHelper<T>
    {
        public static List<EnumNameValue<T>> GetNames()
        {
            var values = Enum.GetValues(typeof(T));
            var result = new List<EnumNameValue<T>>();
            foreach (T value in values)
            {
                result.Add(new EnumNameValue<T>(value));
            }

            return result;
        }
    }
}
