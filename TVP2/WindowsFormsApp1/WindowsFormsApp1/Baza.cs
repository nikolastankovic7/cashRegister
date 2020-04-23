using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
namespace WindowsFormsApp1
{
    class Baza
    {
        OleDbConnection konekcija;
        public Baza()
        {
            this.konekcija = new OleDbConnection();
            konekcija.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source ='D:\TVP2\Prodavnica3617.accdb'";

        }
        public void otvoriKonekciju()
        {
            if(konekcija!=null)
            {
                konekcija.Open();
            }
        }
        public void zatvoriKonekciju()
        {
            if(konekcija!=null)
            {
                konekcija.Close();
            }
        }
        public OleDbConnection Konekcija { get => konekcija; set => konekcija = value; }


    }
}
