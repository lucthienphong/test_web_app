using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SweetSoft.APEM.WebApp.Common
{
    public static class ObjectJsonHelper
    {
        public static T DictionaryToObject<T>(IDictionary<string, object> dict) where T : new()
        {
            T t = new T();
            PropertyInfo[] properties = t.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (!dict.Any(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase)))
                    continue;
                KeyValuePair<string, object> item = dict.First(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase));
                Type tPropertyType = t.GetType().GetProperty(property.Name).PropertyType;
                Type newT = Nullable.GetUnderlyingType(tPropertyType) ?? tPropertyType;
                if (item.Value != null && item.Value.GetType() != typeof(Newtonsoft.Json.Linq.JArray)
                    && property.CanWrite == true)
                {
                    try
                    {
                        object newA = Convert.ChangeType(item.Value, newT);
                        t.GetType().GetProperty(property.Name).SetValue(t, newA, null);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            return t;
        }

        public static object DictionaryToObjectFromFile(string jsonFile, Type typeClass)
        {
            if (File.Exists(jsonFile) == false)
                return null;
            try
            {
                return DictionaryToObject(File.ReadAllText(jsonFile), typeClass);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static object DictionaryToObject(string jsonObject, Type typeClass)
        {
            if (string.IsNullOrEmpty(jsonObject))
                return null;
            try
            {
                Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonObject);
                return DictionaryToObject(dic, typeClass);

            }
            catch (Exception ex)
            {
                try
                {
                    List<KeyValuePair<string, object>> dic = JsonConvert.DeserializeObject<List<KeyValuePair<string, object>>>(jsonObject);
                    return DictionaryToObject(dic.ToDictionary(x => x.Key, x => x.Value), typeClass);
                }
                catch (Exception ex2)
                {
                    return null;
                }
            }
        }

        public static object DictionaryToObject(List<KeyValuePair<string, object>> dic, Type typeClass)
        {
            return DictionaryToObject(dic.ToDictionary(x => x.Key, x => x.Value), typeClass);
        }

        public static object DictionaryToObject(IDictionary<string, object> dict, Type typeClass)
        {
            object t = Activator.CreateInstance(typeClass);
            PropertyInfo[] properties = t.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (!dict.Any(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase)))
                    continue;
                KeyValuePair<string, object> item = dict.First(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase));
                Type tPropertyType = t.GetType().GetProperty(property.Name).PropertyType;
                Type newT = Nullable.GetUnderlyingType(tPropertyType) ?? tPropertyType;
                if (item.Value != null && item.Value.GetType() != typeof(Newtonsoft.Json.Linq.JArray)
                    && property.CanWrite == true)
                {
                    try
                    {
                        if (newT == typeof(Guid))
                        {
                            object newA = new Guid(item.Value.ToString());
                            t.GetType().GetProperty(property.Name).SetValue(t, newA, null);
                        }
                        else
                        {
                            object newA = Convert.ChangeType(item.Value, newT);
                            t.GetType().GetProperty(property.Name).SetValue(t, newA, null);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            return t;
        }

        public static string ToJSONString(this object obj)
        {
            StringBuilder sb = new StringBuilder();
            Type t = obj.GetType();
            if (t.BaseType != null && t.BaseType.FullName.ToLower().StartsWith("subsonic") == false
                && t.BaseType.FullName.ToLower().Contains("dataaccess"))
                t = t.BaseType;

            var filter = t.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                //.Where(x => x.PropertyType.FullName.ToLower().Contains("dataaccess") == false)
                .Select(p => new KeyValuePair<string, object>(p.Name,
                    p.PropertyType.FullName.ToLower().Contains("dataaccess") ? null : p.GetValue(obj, null)));

            //dung key đầu tiên đê xac dinh ten bang
            List<KeyValuePair<string, object>> lst = filter.ToList();
            string fullClassName = obj.GetType().FullName;
            string[] arr = fullClassName.Split('.');
            lst.Insert(0, new KeyValuePair<string, object>("TableName", arr[arr.Length - 1]));

            sb.Append(JsonConvert.SerializeObject(lst, Formatting.Indented, new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            }));
            return sb.ToString();
        }

        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr =
                           Attribute.GetCustomAttribute(field,
                             typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }
    }
}
