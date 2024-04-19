using System;
using System.Collections.Generic;
using System.Text;

namespace language_app.Models
{
    public class DB_Course
    {
        public int ID { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public int Actual_lesson { get; set; }
        public int General_lesson { get; set; }
    }
}
