using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MeslekOdalari.Entity.Entities
{
    public class Feature:BaseEntity
    {
        public string ImageUrl { get; set; }
        public string MembershipStatus { get; set; }
        public int NumberofMembers { get; set; }
        public int NumberofProjects { get; set; }
        public string AreasofExpertise { get; set; }

    }
}
