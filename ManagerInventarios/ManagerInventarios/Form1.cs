using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ManagerInventarios
{
    public partial class Form_Principal : Form
    {
        NuevaConexion Cn_ConexionGlobal;

        public Form_Principal()
        {
            InitializeComponent();
        }

        private void Form_Principal_Load(object sender, EventArgs e)
        {
            string r = "Libro1.xls";
            NuevaConexion nc = new NuevaConexion(r, "Hoja1");
            this.Cn_ConexionGlobal = nc;
            nc.AbrirConexion();

            // enlazar datos 
            this.dataGridView1.DataSource = nc.ListarRegistos();
            nc.CerrarConexion();

           

            
        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cn_ConexionGlobal.AbrirConexion();
            this.Cn_ConexionGlobal.InsertarFila(dataGridView1.SelectedCells);
        }
    }
}
