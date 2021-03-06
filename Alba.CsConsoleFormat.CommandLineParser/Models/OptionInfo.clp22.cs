﻿extern alias CommandLineParser_2_2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using CommandLineParser_2_2::CommandLine;

namespace Alba.CsConsoleFormat.CommandLineParser
{
    public partial class OptionInfo
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        private OptionInfo FromMember22(MemberInfo member, IEnumerable<Attribute> attributes, bool isVerb = false)
        {
            _attributes = attributes.Where(a => a is BaseAttribute || a is VerbAttribute).ToList();
            SourceMember = member;

            var basic = Get<BaseAttribute>();
            var option = Get<OptionAttribute>();
            var value = Get<ValueAttribute>();
            var verb = Get<VerbAttribute>();

            IsPositional = value != null;
            IsRequired = !isVerb && verb == null && (basic?.Required ?? false);
            IsVisible = !(verb?.Hidden == true || basic?.Hidden == true);
            Index = value?.Index ?? -1;
            DefaultValue = basic?.Default;
            HelpText = Nullable(verb?.HelpText) ?? Nullable(basic?.HelpText);
            MetaValue = Nullable(basic?.MetaValue);
            ShortName = Nullable(option?.ShortName);
            Name = Nullable(verb?.Name) ?? Nullable(value?.MetaName) ?? Nullable(option?.LongName)
             ?? (ShortName != null ? null : member?.Name.ToLower());
            SetName = Nullable(option?.SetName);
            OptionKind = isVerb ? OptionKind.Verb : GetValueKind22(option, value, verb);

            return this;
        }

        private static OptionKind GetValueKind22(OptionAttribute option, ValueAttribute value, VerbAttribute verb)
        {
            if (verb != null)
                return OptionKind.Verb;
            if (option != null)
                return option.Separator == '\0' ? OptionKind.Single : OptionKind.List;
            if (value != null)
                return OptionKind.Single;
            return OptionKind.Unknown;
        }
    }
}