using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace dotnet
{
    public class EnhancedObservableCollection<T> : ObservableCollection<T> where T : class
    {
        public EnhancedObservableCollection() : base() { }
        public EnhancedObservableCollection(IEnumerable<T> collection) : base(collection) { }

        // Rekurencyjne sortowanie
        public void SortRecursiveByProperty<K>(
            Expression<Func<T, K>> propertySelector,
            bool ascending = true,
            Action<T, IEnumerable<T>> setSubordinates = null)
            where K : IComparable
        {
            var propertyName = GetPropertyName(propertySelector);
            var propertyInfo = typeof(T).GetProperty(propertyName);

            ValidateProperty(propertyInfo, propertyName);

            // Sortuj bieżący poziom
            SortLevelByProperty(propertyInfo, ascending);

            // Sortuj rekurencyjnie podwładnych
            if (setSubordinates != null)
            {
                foreach (var item in this)
                {
                    var subordinates = GetSubordinates(item);
                    if (subordinates != null && subordinates.Any())
                    {
                        var sortedSubordinates = ascending
                            ? subordinates.OrderBy(x => propertyInfo.GetValue(x))
                            : subordinates.OrderByDescending(x => propertyInfo.GetValue(x));

                        var enhancedCollection = new EnhancedObservableCollection<T>(sortedSubordinates);
                        enhancedCollection.SortRecursiveByProperty(propertySelector, ascending, setSubordinates);
                        setSubordinates(item, enhancedCollection);
                    }
                }
            }
        }

        // Rekurencyjne wyszukiwanie
        public List<T> FindRecursiveByProperty<K>(
            Expression<Func<T, K>> propertySelector,
            K value,
            Func<T, IEnumerable<T>> getSubordinates)
        {
            var propertyName = GetPropertyName(propertySelector);
            var propertyInfo = typeof(T).GetProperty(propertyName);

            ValidateSearchProperty(propertyInfo, propertyName);

            var results = new List<T>();

            foreach (var item in this)
            {
                var propValue = propertyInfo.GetValue(item);
                if (propValue != null && propValue.Equals(value))
                {
                    results.Add(item);
                }

                if (getSubordinates != null)
                {
                    var subordinates = getSubordinates(item);
                    if (subordinates != null && subordinates.Any())
                    {
                        var nestedCollection = new EnhancedObservableCollection<T>(subordinates);
                        results.AddRange(nestedCollection.FindRecursiveByProperty(
                            propertySelector, value, getSubordinates));
                    }
                }
            }

            return results;
        }

        #region Helper Methods
        private IEnumerable<T> GetSubordinates(T item)
        {
            if (item is Pracownik pracownik)
                return pracownik.Podwladni as IEnumerable<T>;
            return null;
        }

        private void SortLevelByProperty(PropertyInfo propertyInfo, bool ascending)
        {
            var sorted = ascending
                ? this.OrderBy(x => propertyInfo.GetValue(x)).ToList()
                : this.OrderByDescending(x => propertyInfo.GetValue(x)).ToList();

            ApplySorting(sorted);
        }

        private void ApplySorting(List<T> sortedItems)
        {
            for (int i = 0; i < sortedItems.Count; i++)
            {
                var currentIndex = IndexOf(sortedItems[i]);
                if (currentIndex != i)
                {
                    Move(currentIndex, i);
                }
            }
        }

        private void ValidateProperty(PropertyInfo propertyInfo, string propertyName)
        {
            if (propertyInfo == null)
                throw new ArgumentException($"Property {propertyName} not found");

            if (!IsIComparable(propertyInfo.PropertyType))
                throw new ArgumentException($"Property {propertyName} must implement IComparable");
        }

        private void ValidateSearchProperty(PropertyInfo propertyInfo, string propertyName)
        {
            if (propertyInfo == null)
                throw new ArgumentException($"Property {propertyName} not found");

            if (propertyInfo.PropertyType != typeof(string) && propertyInfo.PropertyType != typeof(int))
                throw new ArgumentException("Search supports only string and int properties");
        }

        private bool IsIComparable(Type type)
        {
            return type.GetInterface("IComparable") != null;
        }

        private string GetPropertyName<K>(Expression<Func<T, K>> propertySelector)
        {
            if (propertySelector.Body is MemberExpression member)
                return member.Member.Name;

            throw new ArgumentException("Invalid property selector");
        }
        #endregion
    }
}