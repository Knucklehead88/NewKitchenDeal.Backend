using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class BaseSpecParams
    {
        protected const int MaxPageSize = 50;
        public int PageIndex { get; set; } = 1;

        protected int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public string Sort { get; set; }
        protected string _search;
        public string Search
        {
            get => _search;
            set => _search = value?.ToLower();
        }
    }
}
