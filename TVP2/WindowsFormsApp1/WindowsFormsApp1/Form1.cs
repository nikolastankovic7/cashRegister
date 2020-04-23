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
    public partial class Form1 : Form
    { Baza db;
        Grupa g1;
        ListViewItem lvi = new ListViewItem();
        List<Grupa> Lgrupa;
        List<Artikal> LArtikala;
        double pomocna;
        public Form1()
        {
            InitializeComponent();
            db = new Baza();
            Lgrupa = new List<Grupa>();
            LArtikala = new List<Artikal>();
            timer1.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
         
            try
            {
                db.otvoriKonekciju();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = db.Konekcija;
                cmd.CommandText = "Select * FROM Grupa";
                OleDbDataReader citac = cmd.ExecuteReader();
                Lgrupa.Clear();
                while(citac.Read())
                {
                    Grupa g = new Grupa();
                    g.Id_grupe = int.Parse(citac["id_grupe"].ToString());
                    g.Naziv = citac["naziv"].ToString();
                    Lgrupa.Add(g);

                }
                comboBox1.DataSource = Lgrupa;
                comboBox1.DisplayMember = "Naziv grupe";
                comboBox1.ValueMember = "Naziv grupe";
                

                
            }
            catch(Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            finally { db.zatvoriKonekciju(); }
            try
            {
                db.otvoriKonekciju();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = db.Konekcija;
                cmd.CommandText = "Select * From Artikal";
                OleDbDataReader citac = cmd.ExecuteReader();
                LArtikala.Clear();
                while(citac.Read())
                {
                    Artikal a = new Artikal();
                    a.Id = int.Parse(citac["id_artikla"].ToString());
                    a.Id_grupe = int.Parse(citac["id_grupe"].ToString());
                    a.Naziv = citac["naziv"].ToString();
                    a.Cena = double.Parse(citac["cena"].ToString());
                    a.Popust =double.Parse(citac["popust"].ToString());
                    a.Popust =double.Parse(citac["popust"].ToString());
                    a.Popust =double.Parse(citac["popust"].ToString());
                    LArtikala.Add(a);

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { db.zatvoriKonekciju(); }
            listView1.Items.Clear();
           
            if (LArtikala!=null)
            {
                foreach(Artikal a in LArtikala)
                {
                    lvi = new ListViewItem(a.Naziv);
                    lvi.SubItems.Add(a.Cena.ToString());
                    lvi.SubItems.Add(a.Popust.ToString());
                    listView1.Items.Add(lvi);
                }
            }
        }
        
        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            if (LArtikala != null && comboBox1.SelectedValue != null)
            {
                foreach (Grupa g in Lgrupa)
                {
                    if (comboBox1.SelectedValue.ToString().Equals(g.Naziv))
                    {
                        g1 = new Grupa(g.Id_grupe, g.Naziv);
                    }
                }
                foreach (Artikal a in LArtikala)
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
                if(numericUpDown1.Value>=1)
                {
                    lvi = new ListViewItem(listView1.SelectedItems[0].Text);
                    double broj1=double.Parse(listView1.SelectedItems[0].SubItems[1].Text)- double.Parse(listView1.SelectedItems[0].SubItems[2].Text);
                    lvi.SubItems.Add(broj1.ToString());
                    lvi.SubItems.Add(numericUpDown1.Value.ToString());
                    listView2.Items.Add(lvi);
                    double num = (double.Parse(listView1.SelectedItems[0].SubItems[1].Text) - double.Parse(listView1.SelectedItems[0].SubItems[2].Text)) * double.Parse(numericUpDown1.Value.ToString());
                    pomocna += num;
                    textBox1.Text = pomocna.ToString();
                }
            }
            catch(Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                numericUpDown1.ResetText();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if(listView2.Items.Count==0)
                {
                    MessageBox.Show("Ne možete da stornirate prazan račun!");
                    return;
                }
                double num = 0;
                pomocna = num;
                listView2.Items.Clear();
                textBox1.Text = pomocna.ToString();
                textBox1.Clear();
                MessageBox.Show("Račun je storniran");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView2.Items.Count == 0)
                {
                    MessageBox.Show("Greška, morate da dodate barem jedan artikal na račun");
                    return;
                }
                double num = double.Parse(listView2.SelectedItems[0].SubItems[1].Text) * int.Parse(listView2.SelectedItems[0].SubItems[2].Text);
                pomocna -= num;
                listView2.Items.Remove(listView2.SelectedItems[0]);
                textBox1.Text = pomocna.ToString();
               
                MessageBox.Show("Artikal je uklonjen");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView2.Items.Count == 0)
                {
                    MessageBox.Show("Ne možete da kreirate prazan račun!");
                    return;
                }
                db.otvoriKonekciju();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = db.Konekcija;
                cmd.CommandText = @"Insert into Racun(cena,datum,vreme) values(@cena,@datum,@vreme)";
                cmd.Parameters.AddWithValue("cena", double.Parse(textBox1.Text));
                cmd.Parameters.AddWithValue("datum", DateTime.Parse(DateTime.Now.ToShortDateString()));
                cmd.Parameters.AddWithValue("vreme", DateTime.Parse(DateTime.Now.ToShortTimeString()));
                int rezultat = cmd.ExecuteNonQuery();
                if (rezultat > 0)
                {
                    MessageBox.Show("Uspešno ste kreirali račun");
                    textBox1.Clear();
                    pomocna = 0;
                    numericUpDown1.ResetText();
                    listView2.Items.Clear();
                }
                else
                    MessageBox.Show("Greska");
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            finally { db.zatvoriKonekciju(); }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            NoviArtikal dodaj = new NoviArtikal();
            this.Hide();
            dodaj.Show();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            PrikazRacuna racuni = new PrikazRacuna();
            this.Hide();
            racuni.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            this.label6.Text = dateTime.ToString();
        }
    }
    }
