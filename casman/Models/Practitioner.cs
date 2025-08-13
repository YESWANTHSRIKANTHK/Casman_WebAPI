namespace casman.Models
{
    public class Practitioner
    {
        public int Id { get; set; }
        public string PracNumber { get; set; }
        public string Surname { get; set; }
        public string Forename { get; set; }
        public string Initials { get; set; }
        public string Sex { get; set; }
        public string Warnings { get; set; }
        public string Indemnifier { get; set; }
        public string SpecialityAtDOI { get; set; }
    }

}
