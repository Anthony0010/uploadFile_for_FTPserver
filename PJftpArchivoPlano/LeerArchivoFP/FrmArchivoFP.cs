using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.Odbc;
using System.Net;
using System.Threading;



namespace PJftpArchivoPlano.LeerArchivoFP
{
    public partial class FrmArchivoFP : Form
    {

        private WebFile upFile = new WebFile();
        
       
        public FrmArchivoFP()
        {
            InitializeComponent();
        }

         void buscarDatos()
        {
            try
            {

                //conexion para la busqueda en una base de datos FOXPro
                OdbcConnection conn = new OdbcConnection(@"Dsn=restaurant;sourcedb=C:\softrestaurant\dataBase.dbc;sourcetype=DBC;exclusive=No;backgroundfetch=Yes;collate=Machine;null=Yes;deleted=Yes");
                conn.Open();

                //Sentcia sql con conexion odbc
                OdbcDataAdapter adapter = new OdbcDataAdapter("Select field from table.dbf ", conn);
                //Creacion de data Set para llenar el la tabla
                DataSet ds = new DataSet();

                adapter.Fill(ds);           

                dataGridView1.DataSource = ds.Tables[0];
            }

            catch (OdbcException ex)
            {


                MessageBox.Show(ex.Message);

            }
           
        }


        void filtrarDatosBuscados()
        {
           
            try
            {
                //Este metodo es para el filtrado de datos segun el renglon de fecha delimitados por coma ","
                listBox1.Items.Add("ID_TRANSACION," + "NUMSERIE," + "FECHA," + "HORA," + "TOTALART," + "TOTALTRASVENTA," + "TASA," + "TOTALBRUTO," + "TOTALIMPUESTOS," + "TOTALNETO");
              
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {

                    if (dtFecha.Text == row.Cells[2].Value.ToString().Replace(",", ".").Trim().Substring(0, 10))
                    {
                        listBox1.Items.Add(row.Cells[0].Value.ToString() + ",G68" + "," + row.Cells[2].Value.ToString().Replace(",", ".").Trim().Substring(0, 10) + "," + row.Cells[2].Value.ToString().Replace(":", "").Trim().Substring(11, 5) + "," + row.Cells[1].Value.ToString().Replace(",", ".").Trim() + ",00,1," + row.Cells[5].Value.ToString().Replace(",", ".").Trim() + "," + row.Cells[3].Value.ToString().Replace(",", ".").Trim() + "," + row.Cells[4].Value.ToString().Replace(",", ".").Trim() + "");
                    }

                }


            }
            catch (NullReferenceException ex)
            {
             
            }
          
        
        }
        private void button1_Click(object sender, EventArgs e)
        {
            buscarDatos();
        } 
        private void button2_Click(object sender, EventArgs e)

        {
            //Aqui es donde se escribe el archivo que se creara de no existir o se re-escribira de si exite para luego ser subido
            StreamWriter escribir = new StreamWriter("C:\\folderName\\fileName" + dtFecha.Text.Replace("/","") + ".txt");
            for (int i = 0; i <= listBox1.Items.Count -1; i++)
            {
                escribir.WriteLine(listBox1.Items[i].ToString());

            }

            escribir.Close();         
            
            //Aqui invocamos el  metodo que contiene las credenciales y el nombre de archivo a subir
            upFile.SubirArchivoWeb("ftp://ftp.url.com", "user", "passwor", "fileName" + dtFecha.Text.Replace("/", "") + ".txt");

           
   
        }

      

      

        
       
      

        private void lblcontador_Click(object sender, EventArgs e)
        { 
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

       
           

        private void FrmArchivoFP_Load(object sender, EventArgs e)
        {
            //Aqui se maneja la fecha restandole un dia a la fecha actual
            dtFecha.Value = DateTime.Now.Date.AddDays(-1);
            buscarDatos();
            filtrarDatosBuscados();
            button2_Click(sender, e);
            Application.Exit();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            filtrarDatosBuscados();
        }
    }
}
