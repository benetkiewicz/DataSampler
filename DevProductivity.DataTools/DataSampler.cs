namespace DevProductivity.DataTools
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq.Expressions;

    public class DataSampler<T>
    {
        private readonly Dictionary<string, Func<object>> dataGenerators = new Dictionary<string, Func<object>>();
        private readonly List<Type> knownTypes;

        public DataSampler()
        {
            this.knownTypes = new List<Type>();
            this.dataGenerators = new Dictionary<string, Func<object>>();
        }

        public DataSampler<T> AddKnownType(Type type)
        {
            this.knownTypes.Add(type);
            return this;
        }

        public DataSampler<T> AddPropertyConfiguration<TV>(Expression<Func<T, TV>> property, Func<TV> propertyDataGenerator)
        {
            string propName = ReflectionHelper<T>.GetFullPropertyName(property);
            Func<object> f = () => propertyDataGenerator();
            this.dataGenerators.Add(propName, f);
            return this;
        }

        public IList<T> GenerateListOf(int requestedNumberOfItems)
        {
            var resultList = new List<T>();
            for (int i = 0; i < requestedNumberOfItems; i++)
            {
                resultList.Add((T)this.CreateObjectInstance(typeof(T), string.Empty));
            }

            return resultList;
        }

        private object CreateObjectInstance(Type type, string parentPropertyFullName)
        {
            object instance = Activator.CreateInstance(type);
            var propList = type.GetProperties();
            foreach (var propertyInfo in propList)
            {
                string key = string.IsNullOrEmpty(parentPropertyFullName) ? propertyInfo.Name : string.Format(CultureInfo.InvariantCulture, "{0}.{1}", parentPropertyFullName, propertyInfo.Name, CultureInfo.InvariantCulture);
                if (this.dataGenerators.ContainsKey(key))
                {
                    Func<object> typeSampleGenerator = this.dataGenerators[key];
                    propertyInfo.SetValue(instance, typeSampleGenerator(), null);
                }
                else if (this.knownTypes.Contains(propertyInfo.PropertyType))
                {
                    object sub = this.CreateObjectInstance(propertyInfo.PropertyType, key);
                    propertyInfo.SetValue(instance, sub, null);
                }
            }

            return instance;
        }
    }
}