using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace WindowsFormsApp1
{
    public partial class NoviArtikal : Form
    {
        bool izmena = false;
        Baza db;
        List<Artikal> listaArtikala;
        List<Grupa> listaGrupa;
        Grupa g1;
        Grupa g2;
        ListViewItem lvi;
        public NoviArtikal()
        {
            InitializeComponent();
            db = new Baza();
            listaArtikala = new List<Artikal>();
            listaGrupa = new List<Grupa>();
            lvi = new ListViewItem();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 pocetna = new Form1();
            pocetna.Show();
            this.Close();
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            if (listaArtikala != null && comboBox1.SelectedValue != null)
            {
                foreach (Grupa g in listaGrupa)
                {
                    if (comboBox1.SelectedValue.ToString().Equals(g.Naziv))
                    {
                        g1 = new Grupa(g.Id_grupe, g.Naziv);
                    }
                }
                foreach (Artikal a in listaArtikala)
                {
                    if (a.Id_grupe == g1.Id_grupe)
                    {
                        lvi = new ListViewItem(a.Naziv);
                        lvi.SubItems.Add(a.Cena.ToString());
                        lvi.SubItems.Add(a.Popust.ToString());
                        listView1.Items.Add(lvi);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Grupa g in listaGrupa)
                {
                    if (comboBox1.SelectedValue.ToString().Equals(g.Naziv))
                    {
                        g2 = new Grupa(g.Id_grupe, g.Naziv);
                    }
                }
                int brojac = listView1.Items.Count + 23;
                db.otvoriKonekciju();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = db.Konekcija;
                cmd.CommandText = @"INSERT INTO 
                Artikal(id_artikla,id_grupe,naziv,cena,popust)
                VALUES (@id_artikla,@id_grupe,@naziv,@cena,@popust)";
                cmd.Parameters.AddWithValue("id_artikla", brojac++);
                cmd.Parameters.AddWithValue("id_grupe", g2.Id_grupe);
                cmd.Parameters.AddWithValue("naziv", textBox1.Text);
                cmd.Parameters.AddWithValue("cena", double.Parse(textBox2.Text));
                cmd.Parameters.AddWithValue("popust", double.Parse(textBox3.Text));
                Artikal a = new Artikal(listView1.Items.Count, g2.Id_grupe, textBox1.Text, double.Parse(textBox2.Text), double.Parse(textBox3.Text));
                lvi = new ListViewItem(a.Naziv);
                lvi.SubItems.Add(a.Cena.ToString());
                lvi.SubItems.Add(a.Popust.ToString());
                listView1.Items.Add(lvi);
                int rezultat = cmd.ExecuteNonQuery();
                if (rezultat > 0)
                {
                    MessageBox.Show("Artikal je dodat u ponudu");
                    comboBox1.ResetText();
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                }
                else
                    MessageBox.Show("Greska");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { db.zatvoriKonekciju(); }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                db.otvoriKonekciju();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = db.Konekcija;
                cmd.CommandText = @"DELETE FROM Artikal WHERE [naziv]= @naziv";
                cmd.Parameters.AddWithValue("@naziv", listView1.SelectedItems[0].Text);
                int rezultat = cmd.ExecuteNonQuery();
                if (rezultat > 0)
                {
                    MessageBox.Show("Artikal je obrisan");
                    listView1.Items.Remove(listView1.SelectedItems[0]);
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                }
                else
                    MessageBox.Show("Greska");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { db.zatvoriKonekciju(); }
        }

        private void NoviArtikal_Load(object sender, EventArgs e)
        {
            try
            {
                db.otvoriKonekciju();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = db.Konekcija;
                cmd.CommandText = "SELECT * FROM Grupa";
                OleDbDataReader reader = cmd.ExecuteReader();
                listaGrupa.Clear();
                while (reader.Read())
                {
                    Grupa g = new Grupa();
                    g.Id_grupe = int.Parse(reader["id_grupe"].ToString());
                    g.Naziv = reader["naziv"].ToString();
                    listaGrupa.Add(g);
                }

                comboBox1.DataSource = listaGrupa;
                comboBox1.DisplayMember = "Naziv";
                comboBox1.ValueMember = "Naziv";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { db.zatvoriKonekciju(); }
            try
            {
                db.otvoriKonekciju();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = db.Konekcija;
                cmd.CommandText = "SELECT * FROM Artikal";
                OleDbDataReader reader = cmd.ExecuteReader();
                listaArtikala.Clear();
                while (reader.Read())
                {
                    Artikal a = new Artikal();
                    a.Id = int.Parse(reader["id_artikla"].ToString());
                    a.Id_grupe = int.Parse(reader["id_grupe"].ToString());
                    a.Naziv = reader["naziv"].ToString();
                    a.Cena = (double)reader["cena"];
                    a.Popust = (double)reader["popust"];
                    listaArtikala.Add(a);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { db.zatvoriKonekciju(); }
        }
    }
}
         




