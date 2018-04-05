using System;
using System.Collections.Generic;

namespace AuthApp.Models
{
    public class GroupedList<T> : List<T> 
    {
        public string Heading { get; set; }
        public string HeadingWithCount { get => $"{Count} - {Heading}"; }
        public IList<T> Items => this;

        public GroupedList()
        {
            
        }

        public GroupedList(IList<T> items)
        {
            this.AddRange(items);
        }
    }
}
