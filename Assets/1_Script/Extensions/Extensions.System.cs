using System;

namespace TPM
{
    public static class ObjectExtensions
    {
        public static string ToIndentedJsonString(this System.Object obj)
        {
            return JsonUtil.SerializeObjectWithIndentation(obj);
        }
    }

    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string @this)
        {
            return string.IsNullOrEmpty(@this);
        }
    }
    
    public static class StringArrayExtensions
    {
        public static bool Contains(this string[] @this, string item)
        {
            foreach (var subItem in @this)
            {
                if (string.Equals(subItem, item)) return true;
            }
            return false;
        }
    }
    
    public static class ArrayExtensions
    {
        public static bool Contains<TItem>(this TItem[] @this, TItem item) where TItem : IComparable<TItem>
        {
            foreach (var subItem in @this)
            {
                if (subItem.CompareTo(item) == 0) return true;
            }
            return false;
        }
    }
}

