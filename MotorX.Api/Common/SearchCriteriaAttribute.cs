﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorX.Common
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=false)]
    public class SearchCriteriaAttribute : Attribute
    {}
}
