﻿using System;
using System.ComponentModel;
using System.Globalization;
using Alba.CsConsoleFormat.Framework.Text;

// ReSharper disable CanBeReplacedWithTryCastAndCheckForNull
namespace Alba.CsConsoleFormat
{
    /// <summary>
    /// Converts between a sequence of <see cref="string">Strings</see> and <see cref="Thickness"/>:
    /// "1"/<c>{ 1, 1, 1, 1 }</c>, "1 2"/<c>{ 1, 2, 1, 2 }</c>, "1 2 3 4"/<c>{ 1, 2, 3, 4 }</c> (left, top, right, bottom).
    /// Separator can be " " or ",".
    /// </summary>
    public class ThicknessConverter : TypeConverter
    {
        public override bool CanConvertFrom (ITypeDescriptorContext context, Type sourceType)
        {
            TypeCode type = Type.GetTypeCode(sourceType);
            return type == TypeCode.String || TypeCode.Int16 <= type && type <= TypeCode.Decimal || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom (ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null)
                throw GetConvertFromException(null);
            else if (value is string)
                return FromString((string)value);
            else {
                TypeCode type = Type.GetTypeCode(value.GetType());
                if (TypeCode.Int16 <= type && type <= TypeCode.Decimal)
                    return new Thickness(Convert.ToInt32(value, CultureInfo.InvariantCulture));
            }
            return base.ConvertFrom(context, culture, value);
        }

        private static Thickness FromString (string str)
        {
            string[] parts = str.Split(new[] { ' ', ',' }, 4, StringSplitOptions.RemoveEmptyEntries);
            switch (parts.Length) {
                case 1:
                    return new Thickness(GetWidth(parts[0]));
                case 2:
                    return new Thickness(GetWidth(parts[0]), GetWidth(parts[1]), GetWidth(parts[0]), GetWidth(parts[1]));
                case 4:
                    return new Thickness(GetWidth(parts[0]), GetWidth(parts[1]), GetWidth(parts[2]), GetWidth(parts[3]));
                default:
                    throw new FormatException("Invalid Thickness format: \"{0}\"".Fmt(str));
            }
        }

        private static int GetWidth (string str)
        {
            return int.Parse(str, CultureInfo.InvariantCulture);
        }
    }
}