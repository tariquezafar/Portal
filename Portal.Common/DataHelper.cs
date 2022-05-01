using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Common
{
    public interface IModel
    {
    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class NameMapper : Attribute
    {
        public NameMapper(string PropertyName)
        {
            this.PropertyName = PropertyName;
        }

        public string PropertyName { get; set; }
    }
    public static class DataHelper
    {
        public static void CopyTo<T>(this IModel model, T destination)
        {
            if (model != null)
            {
                Type modelType = model.GetType();
                Type sourceType = destination.GetType();
                var modelProperties = modelType.GetProperties().ToList();
                var sourceProperties = sourceType.GetProperties().ToList();
                string propName = "";

                modelProperties.ForEach(prop =>
                {
                    var nameAttr = prop.GetCustomAttributes(typeof(NameMapper), true);
                    if (nameAttr != null && nameAttr.Length > 0)
                    {
                        propName = (nameAttr[0] as NameMapper).PropertyName.ToLower();
                    }
                    else
                    {
                        propName = prop.Name.ToLower();
                    }
                    var sourceProp = sourceProperties.Where(o => o.Name.ToLower() == propName && o.CanWrite).FirstOrDefault();
                    Type baseType;
                    Type newType;
                    object objValue = null;
                    if (sourceProp != null)
                    {
                        try
                        {
                            newType = sourceProp.PropertyType;
                            if (sourceProp.PropertyType.IsGenericType &&
                                sourceProp.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                newType = sourceProp.PropertyType.GetGenericArguments()[0];
                            }

                            if (prop.PropertyType.IsGenericType &&
                                prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                baseType = prop.PropertyType.GetGenericArguments()[0];
                                objValue = prop.GetValue(model, null);
                                if (objValue != null)
                                    objValue = Convert.ChangeType(objValue, baseType);
                            }
                            else
                            {
                                objValue = prop.GetValue(model, null);
                            }

                            if (objValue != null)
                            {
                                objValue = Convert.ChangeType(objValue, newType);
                                sourceProp.SetValue(destination, objValue, null);
                            }
                            else if (prop.PropertyType.IsGenericType &&
                                prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                sourceProp.SetValue(destination, null, null);
                            }
                        }
                        catch
                        {
                        }
                    }
                });
            }
        }

        public static void CopyFrom<T>(this IModel model, T source)
        {
            if (model != null)
            {
                Type modelType = model.GetType();
                Type sourceType = source.GetType();
                var modelProperties = modelType.GetProperties().ToList();
                var sourceProperties = sourceType.GetProperties().ToList();
                Type baseType;
                Type newType;
                object objValue = null;
                sourceProperties.ForEach(prop =>
                {
                    var modelProp = modelProperties.Where(o => o.Name.ToLower().Equals(prop.Name.ToLower()) && o.CanWrite).FirstOrDefault();

                    if (modelProp == null) // Try Searching Using Attribute
                    {
                        modelProp = modelProperties.Find(item =>
                        {
                            var nameAttr = item.GetCustomAttributes(typeof(NameMapper), true);
                            return nameAttr != null && nameAttr.Length > 0 && (nameAttr[0] as NameMapper).PropertyName.ToLower() == prop.Name.ToLower();
                        });
                    }

                    if (modelProp != null)
                    {
                        newType = modelProp.PropertyType;
                        if (modelProp.PropertyType.IsGenericType &&
                            modelProp.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            newType = modelProp.PropertyType.GetGenericArguments()[0];
                        }

                        if (prop.PropertyType.IsGenericType &&
                                   prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            baseType = prop.PropertyType.GetGenericArguments()[0];
                            objValue = prop.GetValue(source, null);
                            if (objValue != null)
                                objValue = Convert.ChangeType(objValue, baseType);
                        }
                        else
                        {
                            objValue = prop.GetValue(source, null);
                        }

                        if (objValue != null)
                        {
                            objValue = Convert.ChangeType(objValue, newType);
                            modelProp.SetValue(model, objValue, null);
                        }
                    }
                });
            }
        }
    }
}
