﻿namespace Planing.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public int  Numero { get; set; }
        public int FaculteId { get; set; }
        public Faculte Faculte { get; set; }
    }
}
