using System;
using System.Collections.Generic;
using System.Text;

namespace GenericRepository.Specifications
{
    public class BaseSpecialParameters
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Sort { get; set; }
        //public string? Search { get; set; }
    }
}
