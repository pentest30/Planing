﻿namespace Planing.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        //public string Type { get; set; }
        public int SpecialiteId { get; set; }
        public int AnneeId { get; set; }
        public Annee Annee { get; set; }
        public Specialite Specialite { get; set; }
    }
}