using System;

namespace BeanlancerAPI2.DTOs.Applies

{
    public partial class AppliesResponse
    {
     
        public int username { get; set; }
        public string fullname { get; set; }

        public string idDesigner { get; set; }
        public DateTime time { get; set; }
    }
}
