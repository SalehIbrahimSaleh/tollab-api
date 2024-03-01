using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Models;
using SuperMatjar.UseCases.StockMans;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SuperKotob.Admin.Web.Models
{
    public class StockManGridModel
    {
        public class PropertyPath
        {
            IList<PropertyInfo> Properties = new List<PropertyInfo>();
            public void AddProperty(PropertyInfo p)
            {
                this.Properties.Add(p);
            }
            public string GetValue(object item)
            {
                if (this.Properties == null || this.Properties.Count == 0 || this.Properties[0] == null)
                    return null;

                object value = this.Properties.First().GetValue(item);
                for (var i = 1; i < this.Properties.Count; i++)
                {
                    var p = this.Properties[i];
                    if (value != null)
                        value = p.GetValue(value);
                }
                return Convert.ToString(value);
            }
        }
        public string PluralName { get; set; }
        public string SingleName { get; set; }

        public IList<string> Errors { get; set; }
        public PagingMeta PagingMeta { get; set; }
        IList<GridField> _Fields;
        public IList<GridField> Fields
        {
            get
            {
                return _Fields;
            }
            set
            {
                _Fields = value;
                SetPropertiesDictionary();
            }
        }

        IList<StockMan> _Items;
        Dictionary<GridField, PropertyPath> PropertiesDictionary { get; set; } = new Dictionary<GridField, PropertyPath>();

        public IList<StockMan> Items
        {
            get
            {
                return _Items;
            }
            set
            {
                _Items = value;
                SetPropertiesDictionary();
            }
        }

        private void SetPropertiesDictionary()
        {
            if (_Items == null || _Items.Count == 0)
            {
                PropertiesDictionary.Clear();
                return;
            }

            if (_Items[0] == null)
                throw new Exception("Can not set items, first item can not be null");

            if (Fields == null)
                return;

            var properties = TypeDescriptor.GetProperties(_Items[0]);
            foreach (var f in Fields)
            {
                if (string.IsNullOrWhiteSpace(f.TextProperty))
                    continue;

                SetPath(f, _Items[0]);
            }
        }

        private void SetPath(GridField f, object item)
        {
            var path = new PropertyPath();
            var pathSteps = f.TextProperty.Split('.');
            Type target = item.GetType();
            foreach (var step in pathSteps)
            {
                var properties = target.GetProperties();
                var property = properties.FirstOrDefault(p => p.Name == step);
                if (property == null)
                    continue;
                path.AddProperty(property);
                target = property.PropertyType;
            }
            PropertiesDictionary[f] = path;
        }

        public string GetItemValue(object item, GridField field)
        {
            PropertyPath path;
            PropertiesDictionary.TryGetValue(field, out path);
            if (path == null)
                return null;

            var value = path.GetValue(item);
            if (field.Formatter != null)
                value = field.Formatter(value);
            return value;
        }
    }
}