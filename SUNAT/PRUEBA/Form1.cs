using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Windows.Forms;
namespace PRUEBA
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            MessageBox.Show("Aquitoy");
            this.btnConsultar.Click += new System.EventHandler(this.btnConsultar_Click);
            //btnConsultar_Click();
        }

        public class Result
        {
            public string RUC { get; set; }
            public string Estado { get; set; }
            public string RazonSocial { get; set; }

        }
        public class RooObject
        {
            public bool success { get; set; }
            public Result result { get; set; }
        }
        public class DataContribuyente
        {
            public bool success { get; set; }
            public string mensaje { get; set; }
            public string  RUC { get; set; }
            public string Estado { get; set; }
            public string RazonSocial { get; set; }
        }
               
        private void  btnConsultar_Click(object sender, System.EventArgs e)
        {
            try
            {

                MessageBox.Show("Inicio");
                DataContribuyente getDataContribuyente = new DataContribuyente();
                string NroDoc = Convert.ToString(txtNroDoc.Text);
                //string ruta = "https://ruc.com.pe/api/v1/ruc";
                //string token = "cf6015ec-f9b6-4f6c-bc9f-cd45c63e2aca-5b537a0c-6fab-41e2-91c6-3be288afc839";
                //string url = @"https://www.sunatelectronica.com/facturacion/controller/ws_consulta_rucdni.php?documento=RUC&nro_documento=" + NroDoc;
                string url = @"http://www.ingedes.com/getsunat/getsunat.php?ruc="+NroDoc+"&token=98erdsew9uo2w";
                //string url = ruta + token;
                MessageBox.Show(url);
                var web_request = (HttpWebRequest)WebRequest.Create(url);
                using (var response = web_request.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string resultado = reader.ReadToEnd();
                    string jsonRes = Convert.ToString(resultado);
                    var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var ObjetoAnonimo = jss.Deserialize(resultado);
                    RooObject respuesta = JsonConvert.DeserializeObject<RooObject>(jsonRes);

                    //MessageBox.Show(respuesta,"Soy tu JSON");
                    //MessageBox.Show(respuesta.success);
                    if (respuesta.success)
                    {
                        getDataContribuyente.success = true;
                        
                        getDataContribuyente.mensaje = "Peticion Completa";
                        getDataContribuyente.RUC = respuesta.result.RUC;
                        getDataContribuyente.Estado = respuesta.result.Estado;
                        getDataContribuyente.RazonSocial = respuesta.result.RazonSocial;

                    }
                    else
                    {
                        getDataContribuyente.success = false;
                        getDataContribuyente.mensaje = "Nro de Ruc no valido";
                    }
                    MessageBox.Show(getDataContribuyente.RUC);
                    MessageBox.Show(getDataContribuyente.Estado);
                    MessageBox.Show(getDataContribuyente.RazonSocial);

                    txtNroDocRes.Text = getDataContribuyente.RUC;
                    txtEstDocRes.Text = getDataContribuyente.Estado;
                    txtNomDocRes.Text = getDataContribuyente.RazonSocial;
                }
                MessageBox.Show("Fin");
            }
            catch (Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex));
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }



    }
}
