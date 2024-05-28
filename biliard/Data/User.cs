namespace biliard.Data
{
    public class User
    {
        public string FullName { get { return $"{FirstName} {FamilyName}"; } }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
        public string Phone { get; set; }

        public virtual List<Reservation> Reservations { get; set; }
        public User()
        {
            Reservations  = new List<Reservation>();
        }
    }
}
