using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Artikal
    {
        int id;
        string naziv;
        double cena;
        int id_grupe;
        double popust;
        public string Naziv
        {
            get { return naziv; }
            set { naziv = value; }
        }
        public double Cena
        {
            get { return cena; }
            set { cena = value; }
        }
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public int Id_grupe
        {
            get { return id_grupe; }
            set { id_grupe = value; }
        }

        public double Popust { get => popust; set => popust = value; }
        public Artikal()
        {

        }
        public Artikal(int id_artikla, int id_grupe, string naziv, double cena, double popust)
        {
            this.Id = id_artikla;
            this.id_grupe = id_grupe;
            this.naziv = naziv;
            this.cena = cena;
            this.popust = popust;
        }

        public override string ToString()
        {
            return naziv + " " + cena + " " + popust+" ";
        }
    }
}
