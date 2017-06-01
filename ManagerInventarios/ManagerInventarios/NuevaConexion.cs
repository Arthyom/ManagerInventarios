using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;


namespace ManagerInventarios
{
    /*
     * Representa el modelo de la aplicacion,  permite 
     * realizar tranzacciones con los origenes de datos.
     * Contiene:
     *      - datasets(conjuntos de datos)
     *      - dataadapters (adaptadores para origenes de datos)
     *      - conexiones(objetos puros OLEDB)
     *      
     */

    public class NuevaConexion
    {
        // *********************************************************************************** //
        // ************************ propiedades publicos de la clase  ************************ //
        // *********************************************************************************** //
        OleDbConnection         OLDB_CnActual;
        DataSet                 OLDB_ConjntDatos;
        OleDbDataAdapter        OLDB_AdaptdrTabla;
        DataColumnCollection    OLDB_ColumnasEncab;

        // conexion completa con los origenes de datos (provedores de conexion, nombres)
        string          Str_CadConexion;
        List<string>    Lst_StrConsults = new List<string>();
        string          Str_NombreRutaArchivo;
        string          Str_HojaDestino;


     
        // macros para la clase 
        private  const String StrConst_CnxCorrecta = "Conexion Correcta";
        private  const String StrConst_CnxError   = "Conexion fallida";

        // "provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + archivo + "';Extended Properties=Excel 12.0;";


        // *********************************************************************************** //
        // ************************ metodos publicos de la clase  **************************** //
        // *********************************************************************************** //
        public          NuevaConexion()
        {
            this.OLDB_CnActual = null;
            this.Str_CadConexion = "";
        }

        public          NuevaConexion(string Ent_StrArchivoNombre)
        {

            // si se produce un error al conectar con los origenes de datos               
            this.Str_NombreRutaArchivo = Ent_StrArchivoNombre;
            this.Str_CadConexion = "Provider = MicroSoft.Jet.OLEDB.4.0; Data Source=" + Ent_StrArchivoNombre + "; Extended Properties =\"Excel 8.0; HDR=Yes;IMEX =1\";";
           
            try
            {                
                this.OLDB_CnActual = new OleDbConnection(this.Str_CadConexion);
                Console.WriteLine(StrConst_CnxCorrecta);
            }
            catch (Exception ex)
            {
                Console.WriteLine(StrConst_CnxError);
            }
        }

        public          NuevaConexion(string Ent_StrArchivoNombre,string Ent_HojaDestino)
        {
            // si se produce un error al conectar con los origenes de datos               
            this.Str_NombreRutaArchivo = Ent_StrArchivoNombre;
            this.Str_HojaDestino = Ent_HojaDestino;
            this.Str_CadConexion = "Provider = MicroSoft.Jet.OLEDB.4.0; Data Source=" + Ent_StrArchivoNombre + "; Extended Properties =\"Excel 8.0; HDR=YES; IMEX=1\";";
            // "provider=Microsoft.ACE.OLEDB.12.0;Data Source=' " + Ent_StrArchivoNombre + " ';Extended Properties=Excel 12.0;";

            try
            {
                this.OLDB_CnActual = new OleDbConnection(this.Str_CadConexion);               
                Console.WriteLine(StrConst_CnxCorrecta);
            }
            catch (Exception ex)
            {
                Console.WriteLine(StrConst_CnxError);
            }
        }


        public   void   AbrirConexion()
        {
            this.OLDB_CnActual.Open();
        }

        public   void   CerrarConexion()
        {
            this.OLDB_CnActual.Close();
        }


        // realizar una consulta SELECT al origen de datos, se indica la ruta de la hoja y se supone una conexion exitosa y abierta
        public  DataTable        ListarRegistos( )
        {
            string SqlComando = "SELECT * FROM [" + Str_HojaDestino + "$]";

            // crear 1 comando para ejecutar en la hoja de excel
            OleDbCommand cm = new OleDbCommand(SqlComando, this.OLDB_CnActual);
           
            // crear una datetable para cargarla con la consulta
            DataTable tb = new DataTable();
            OleDbDataReader rd = cm.ExecuteReader();

            // conseguir el nombre de las columnas
            this.OLDB_ColumnasEncab = tb.Columns;
            

            tb.Load(rd);
            return tb;
        }

        // conseguir un conjunto de datos para enlazarlo a un control
        private DataSet          ConseguirRegis(DataTable Ent_OldTabla)
        {
            DataSet cd = new DataSet();
      
            return cd;
        }

        public  DataSet          ConseguirOrigenDatos()
        {
            DataSet r = this.ConseguirRegis(this.ListarRegistos());
            return r;
        }

        // insertar los nuevos valores en el archivo seleccionado
        public void InsertarFila(DataGridViewSelectedCellCollection Ent_FilaNueva)
        {
            string SqlComandInsert = "INSERT INTO [" + this.Str_HojaDestino + "$]";
            for (int i = 0; i < this.OLDB_ColumnasEncab.Count; i++)
            {
                string c = this.OLDB_ColumnasEncab[i].Caption;
                string contenido = Convert.ToString( Ent_FilaNueva[i].Value );
                string SqlDinamico = string.Format(" ({0}) VALUES ({1})", c, contenido);



            }

            // crear un nuevo comando 
            OleDbCommand cmdInsert = this.OLDB_CnActual.CreateCommand();


            cmdInsert.CommandText = "INSERT INTO [" + this.Str_HojaDestino + "$] (c1,c2,c3,c4,c5,c6) VALUES ('0','0',0,0,0,0)";
            cmdInsert.ExecuteNonQuery();
        }





    }
}
