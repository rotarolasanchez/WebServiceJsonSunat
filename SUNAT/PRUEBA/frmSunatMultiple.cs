using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace PRUEBA
{
    public partial class frmSunatMultiple : Form
    {

        private List<Dictionary<string, string>> obj = new List<Dictionary<string, string>>();

        public frmSunatMultiple(List<Dictionary<string,string>> datos)
        {
            InitializeComponent();
            obj = datos;
        }

        private void frmSunatMultiple_Load(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Nro RUC", typeof(string)));
            dt.Columns.Add(new DataColumn("Nombre Comercial", typeof(string)));
            dt.Columns.Add(new DataColumn("Tipo Contribuyente", typeof(string)));
            dt.Columns.Add(new DataColumn("Fecha de Inscripcion", typeof(string)));
            dt.Columns.Add(new DataColumn("Fecha de Inicio de Actividades", typeof(string)));
            dt.Columns.Add(new DataColumn("Estado Contribuyente", typeof(string)));
            dt.Columns.Add(new DataColumn("Condicion del Contribuyente", typeof(string)));
            dt.Columns.Add(new DataColumn("Dirección del Domicilio Fiscal", typeof(string)));
            dt.Columns.Add(new DataColumn("Actividad de Comercio Exterior", typeof(string)));

            foreach (Dictionary<string, string> iter in obj) {
                DataRow row = dt.NewRow();
                try
                {
                   
                    row[0] = iter["RucContribuyente"];
                    row[1] = iter["NombreComercial"];
                    row[2] = iter["TipoContribuyente"];
                    row[3] = iter["FechaInscripcion"];
                    row[4] = iter["FechaInicioActividad"];
                    row[5] = iter["EstadoContribuyente"];
                    row[6] = iter["CondicionContribuyente"];
                    row[7] = iter["DireccionContribuyente"];
                    row[8] = iter["AvtividadComercioEx"];
                    dt.Rows.Add(row);
                }
                catch (Exception ex)
                {
                    row[1] = "SIN RESULTADOS";
                    row[2] = "SIN RESULTADOS";
                    row[3] = "SIN RESULTADOS";
                    row[4] = "SIN RESULTADOS";
                    row[5] = "SIN RESULTADOS";
                    row[6] = "SIN RESULTADOS";
                    row[7] = "SIN RESULTADOS";
                    row[8] = "SIN RESULTADOS";
                    dt.Rows.Add(row);
                }
            }

            dgvResSunat.DataSource = dt;
            dgvResSunat.Columns[0].Width = 100;
            dgvResSunat.Columns[1].Width = 200;
            dgvResSunat.Columns[2].Width = 200;
            dgvResSunat.Columns[3].Width = 100;
            dgvResSunat.Columns[4].Width = 100;
            dgvResSunat.Columns[5].Width = 100;
            dgvResSunat.Columns[6].Width = 100;
            dgvResSunat.Columns[7].Width = 250;
            dgvResSunat.Columns[8].Width = 200;
        }
    }
}
