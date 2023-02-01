using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace Ecssr.Demo.Common.Utility
{
    public sealed class SafeType
    {
        public static int SafeInt(object _object)
        {
            int result;
            try
            {
                result = Convert.ToInt32(_object);
            }
            catch
            {
                result = 0;
            }

            return result;
        }
    }

    public static class Extensions
    {
        public static bool IsNullOrEmpty(this string _value)
        {
            if (string.IsNullOrEmpty(_value))
                return true;
            else
                return false;
        }

        public static T FromJsonString<T>(this string rawJsonString)
        {
            dynamic obj = default(T);
            if (rawJsonString.IsNullOrEmpty())
                return obj;

            return JsonConvert.DeserializeObject<T>(rawJsonString);
        }

        public static string? ToJsonString<T>(this T _entity, bool camelCaseRequried = false)
        {
            if (_entity == null)
                return null;

            return JsonConvert.SerializeObject(_entity, new JsonSerializerSettings() 
            { 
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = camelCaseRequried ? new CamelCasePropertyNamesContractResolver() : null
            });
        }

        public static string GetDescription(this Enum _enum)
        {
            Type type = _enum.GetType();
            MemberInfo[] memberInfo = type.GetMember(_enum.ToString());
            if(memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }

            return _enum.ToString();
        }

        public static string ParseException(this Exception exception)
        {
            var stringBuilder = new StringBuilder();

            try
            {
                Exception tempException = exception;
                while(tempException != null)
                {
                    stringBuilder.Append($"Message: { tempException.Message}");
                    stringBuilder.Append($"StackTrace: { tempException.StackTrace}");

                    tempException  = tempException.InnerException;
                }
            }
            catch
            {
                stringBuilder.Append(exception.Message);
            }

            return stringBuilder.ToString();
        }
    }
}
