using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Core
{
    public class RequestInputs
    {
        IDictionary<string, string> _requestInputs { get; set; }
        public RequestInputs()
            : this(new Dictionary<string, string>())
        {
        }
        public RequestInputs(IDictionary<string, string> dictionary)
        {
            var newDictionary = new Dictionary<string, string>();
            foreach (var pair in dictionary)
                newDictionary.Add(pair.Key.ToLower(), pair.Value);

            this._requestInputs = newDictionary;
        }
        public string this[string name]
        {
            get
            {
                string value;
                _requestInputs.TryGetValue(NormalizeKey(name), out value);
                return value;
            }
            set
            {
                _requestInputs[NormalizeKey(name)] = value;
            }
        }
        public bool ContainsKey(string key)
        {
            return _requestInputs.ContainsKey(NormalizeKey(key));
        }

        public T GetValue<T>(string key)
        {
            if (_requestInputs == null || !_requestInputs.ContainsKey(NormalizeKey(key)))
                return default(T);

            string value = InternalGet(key);
            T valueCasted = default(T);
            TryCast(value, ref valueCasted);
            return valueCasted;

        }
        private string NormalizeKey(string key)
        {
            return key.ToLower();
        }
        private string InternalGet(string key)
        {
            key = key.ToLower();
            return _requestInputs[key];
        }

        public bool TryGetValue<T>(string key, ref T value)
        {
            if (_requestInputs == null || !_requestInputs.ContainsKey(NormalizeKey(key)))
                return false;

            string strValue = InternalGet(key);
            return TryCast(strValue, ref value);

        }
        public T GetValue<T>(string key, T defaultValue)
        {
            if (_requestInputs == null || !_requestInputs.ContainsKey(NormalizeKey(key)))
                return defaultValue;

            string value = InternalGet(key);
            T valueCasted = default(T);
            TryCast(value, ref valueCasted);

            return valueCasted;
        }
        public IList<T> GetValue<T>(string key, IList<T> defaultValue, Func<T, bool> filterBy = null)
        {
            if (_requestInputs == null || !_requestInputs.ContainsKey(NormalizeKey(key)))
                return defaultValue;

            string value = InternalGet(key);
            if (!string.IsNullOrWhiteSpace(value) && value.Contains(","))
            {
                var values = value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                var list = new List<T>();
                foreach (var v in values)
                {
                    T vCasted = default(T);
                    if (TryCast(v, ref vCasted))
                        if (filterBy == null || filterBy(vCasted))
                            list.Add(vCasted);
                }
                return list;
            }
            else
            {
                T valueCasted = default(T);
                TryCast(value, ref valueCasted);
                return new List<T>() { valueCasted };
            }
        }

        public bool IsEmpty()
        {
            return _requestInputs.Count == 0;
        }

        bool IsNullable(Type t)
        {
            return !t.IsValueType || Nullable.GetUnderlyingType(t) != null;
        }
        bool TryCast<T>(object value, ref T result)
        {
            var type = typeof(T);

            // If the type is nullable and the result should be null, set a null value.
            if (IsNullable(type) && (value == null || value == DBNull.Value))
            {
                result = default(T);
                return true;
            }

            // Convert.ChangeType fails on Nullable<T> types.  We want to try to cast to the underlying type anyway.
            var underlyingType = Nullable.GetUnderlyingType(type) ?? type;

            try
            {
                // Just one edge case you might want to handle.
                if (underlyingType == typeof(Guid))
                {
                    if (value is string)
                    {
                        value = new Guid(value as string);
                    }
                    if (value is byte[])
                    {
                        value = new Guid(value as byte[]);
                    }

                    result = (T)Convert.ChangeType(value, underlyingType);
                    return true;
                }

                result = (T)Convert.ChangeType(value, underlyingType);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IDictionary<string, string> ToDictionary()
        {
            return _requestInputs;
        }

        public RequestInputs Set(string key, string value)
        {
            _requestInputs[key] = value;
            return this;
        }

        public void Remove(string key)
        {
            _requestInputs.Remove(key);
        }

        public void Add(string key, string value)
        {
            _requestInputs.Add(key.ToLower(), value);
        }
    }
}
