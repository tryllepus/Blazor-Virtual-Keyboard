using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorVirtualKeyboard.Models
{
    public class KeyboardLayout
    {
        public string Name { get; set; }
        public List<List<string>>? Rows { get; set; }
        public List<List<List<string>>>? ColumnRows { get; set; }

        public KeyboardLayout()
        {
            Name = "";
        }
    }
}
