using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace LibraryApi.Domain
{
    public enum ReservationStatus { Pending, Accepted, Rejected}
    //Add to Data context. Update Database (create migration, and update). Auto Mapper
    public class Reservation
    {
        public int Id { get; set; }
        public string For { get; set; }
        public string Items { get; set; }
        public DateTime? AvailableOn { get; set; }
        public ReservationStatus status { get; set; }
    }
}
