using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Models;
using SuperKotob.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SuperKotob.Admin.Web.Models
{
    public enum GridFieldFilterType
    {
        Default,
        Select,
        HyperLink,
        Photo,
        Ajax,
        WithoutSearch,
        Sound,
        ChooseFromMultibleFields
    }
    public class GridFieldFilter
    {
        public GridFieldFilterType Type { get; set; } = GridFieldFilterType.Default;
        public IList<LookupItem> Options = new List<LookupItem>();

        public string _ajaxSource;
        public string AjaxSource
        {
            get
            {
                if (_ajaxSource == null)
                {
                    return $"/{Parent?.Parent?.PluralName}/autocomplete";
                }
                return _ajaxSource;
            }
            set
            {
                _ajaxSource = value;
            }
        }
        public string PreSelectedIds { get; set; }
        public string PreSelectedNames { get; set; }

        public string FilterPath { get; set; }
        public GridField Parent { get; internal set; }
    }
    public class FieldChoosing
    {
        public string ValueProperty { get; set; }
        public string PhotolinkPattern { get; set; }
        public string HyperLinkTitle { get; set; }
        public bool IsHyper { get; set; }
        public GridFieldFilter Filter { get; set; }
    }
    public class GridField
    {
        public IEnumerable<GridField> FieldChoosings { get; set; }
        public GridField(string textProperty)
        {
            this.TextProperty = textProperty;
            this.ValueProperty = textProperty;
            Filter = new GridFieldFilter();
        }
        public bool PreventRenderWhenNull { get; set; }
        public GridField(string displayName, string valueProperty, string hyperLinkTitle, string hyperlinkPattern, Boolean isHyper = true)
             : this(valueProperty)
        {
            this.DisplayName = displayName;
            this.HyperLinkTitle = hyperLinkTitle;
            this.HyperLinkPattern = hyperlinkPattern;
            Filter = new GridFieldFilter();
        }

        public GridField(string displayName, string valueProperty, string hyperLinkTitle, string hyperlinkPattern, bool preventRenderWhenNull, Boolean isHyper = true)
             : this(valueProperty)
        {
            this.DisplayName = displayName;
            this.HyperLinkTitle = hyperLinkTitle;
            this.HyperLinkPattern = hyperlinkPattern;
            this.PreventRenderWhenNull = preventRenderWhenNull;
            Filter = new GridFieldFilter();
        }

        public GridField(string displayName, string photolinkPattern, string valueProperty, int isHyper = 1)
             : this(valueProperty)
        {
            this.DisplayName = displayName;
            this.PhotolinkPattern = photolinkPattern;
            Filter = new GridFieldFilter();
        }

        //
        public GridField(string displayName, IEnumerable<GridField> fieldChoosings, string valueProperty)
             : this(valueProperty)
        {
            this.DisplayName = displayName;
            this.FieldChoosings = fieldChoosings;
            Filter = new GridFieldFilter();
        }

        public GridField(string displayName, string photolinkPattern, string valueProperty, bool preventRenderWhenNull, int isHyper = 1)
             : this(valueProperty)
        {
            this.DisplayName = displayName;
            this.PhotolinkPattern = photolinkPattern;
            this.PreventRenderWhenNull = preventRenderWhenNull;
            Filter = new GridFieldFilter();
        }

        public GridField(string textProperty, string valueProperty, string displayName)
             : this(textProperty)
        {
            this.HyperLinkPattern = string.Empty;
            this.ValueProperty = valueProperty;
            this.DisplayName = displayName;
        }
        public GridField(string textProperty, string valueProperty, string displayName, Func<string, string> formatter)
            : this(textProperty, valueProperty, displayName)
        {
            this.Formatter = formatter;
        }

        GridFieldFilter _filter;
        public GridFieldFilter Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                _filter = value;
                _filter.Parent = this;
            }
        }

        public string TextProperty { get; set; }
        string _DisplayName;


        public string DisplayName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_DisplayName))
                    return TextProperty;
                return _DisplayName;
            }
            set
            {
                _DisplayName = value;
            }
        }

        public Func<string, string> Formatter { get; private set; }
        public string ValueProperty { get; private set; }
        public GridModel Parent { get; internal set; }
        private string _HyperLinkTitle;
        private string _HyperLinkPattern;
        private string _PhotolinkPattern;
        private string _WithoutSearch;
        private string _Sound;

        public string HyperLinkTitle
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_HyperLinkTitle))
                    return TextProperty;
                return _HyperLinkTitle;
            }
            set
            {
                _HyperLinkTitle = value;
            }
        }

        public string HyperLinkPattern
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_HyperLinkTitle))
                    return TextProperty;
                return _HyperLinkPattern;
            }
            set
            {
                _HyperLinkPattern = value;
            }
        }

        public string PhotolinkPattern
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_PhotolinkPattern))
                    return TextProperty;
                return _PhotolinkPattern;
            }
            set
            {
                _PhotolinkPattern = value;
            }
        }
        public string WithoutSearch
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_WithoutSearch))
                    return TextProperty;
                return _WithoutSearch;
            }
            set
            {
                _WithoutSearch = value;
            }
        }
        public string Sound
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_Sound))
                    return TextProperty;
                return _Sound;
            }
            set
            {
                _Sound = value;
            }
        }

    }
    public class GridModel
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
        public string BackText { get; set; }
        public string BackUrl { get; set; }
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
                foreach (var f in _Fields)
                    f.Parent = this;
                SetPropertiesDictionary();
            }
        }

        IList<DataModel> _Items;
        Dictionary<GridField, PropertyPath> PropertiesDictionary { get; set; } = new Dictionary<GridField, PropertyPath>();

        public IList<DataModel> Items
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

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }
}