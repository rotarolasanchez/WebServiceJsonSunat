using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using tessnet2;


namespace PRUEBA
{
    public partial class SUNAT : Form
    {

        string captcha = "";
        CookieContainer cokkie = new CookieContainer();
        string[] nrosRuc = new string[] { };
        public SUNAT()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void genCaptcha() {

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.sunat.gob.pe/cl-ti-itmrconsruc/captcha?accion=image");
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                request.CookieContainer = cokkie;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();

                var image = new Bitmap(responseStream);
                var ocr = new Tesseract();
                ocr.Init(@"C:\Users\rotarola\Desktop\SUNAT\SUNAT\PRUEBA\Content\tessdata", "eng", false);

                var result = ocr.DoOCR(image, Rectangle.Empty);
                foreach (Word word in result)
                {
                    captcha = word.Text;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cayo en TRY GETCATPCHa");
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                genCaptcha();

                string myurl = @"http://www.sunat.gob.pe/cl-ti-itmrconsruc/jcrS00Alias?accion=consPorRuc&nroRuc="+txtNroDoc.Text.Trim()+"&codigo="+captcha.Trim().ToUpper() +"&tipdoc=1";
                HttpWebRequest myWebRequest = (HttpWebRequest)WebRequest.Create(myurl);
                myWebRequest.CookieContainer = cokkie;
                 ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

                HttpWebResponse myhttpWebResponse = (HttpWebResponse)myWebRequest.GetResponse();
                Stream myStream = myhttpWebResponse.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myStream);
                string xDat = ""; int pos = 0;
                while (!myStreamReader.EndOfStream) {
                    txtnroRucValue.Text = txtNroDoc.Text;
                    xDat = myStreamReader.ReadLine();
                    pos++;

                    switch (pos) {
                        case 126:
                            txtTipoContribuyente.Text = getDatafromXML(xDat, 25);
                            break;
                        case 131:
                            txtNombreComercial.Text = getDatafromXML(xDat, 25);
                            break;
                        case 136:
                            txtFechaInscripcion.Text = getDatafromXML(xDat, 25);
                            break;
                        case 138:
                            txtFechaInicioActividad.Text = getDatafromXML(xDat, 25);
                            break;
                        case 142:
                            txtEstadoContribuyente.Text = getDatafromXML(xDat, 25);
                            break;
                        case 152:
                            txtCondicionContribuyente.Text = getDatafromXML(xDat, 0);
                            break;
                        case 158:
                            txtDireccion.Text = getDatafromXML(xDat, 25);
                            break;
                        case 176:
                            txtAvtividadComercioEx.Text = getDatafromXML(xDat, 25);
                            break;
                    }

                }
            }
            catch (Exception ex)
            {
                genCaptcha();
            }
        }


        private string getDatafromXML(string lineRead,int len=0) {

            try
            {
                lineRead = lineRead.Trim();
                lineRead = lineRead.Remove(0, len);
                lineRead = lineRead.Replace("</td>", "");
                while (lineRead.Contains("  ")) {
                  lineRead =   lineRead.Replace("  ", " ");
                }
                return lineRead;
            }
            catch (Exception ex)
            {
                return "NO SE ENCONTRO RESULTADO";
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (button2.Text == "IMPORTAR")
                {
                    button2.Text = "EJECUTAR";
                    string archivoLeer = "";
                    OpenFileDialog filedialog = new OpenFileDialog();
                    filedialog.Filter = "Solo Archivos de Texto|*.txt";
                    filedialog.Title = "Seleccione un Archivo de texto";
                    if (filedialog.ShowDialog() == DialogResult.OK)
                    {
                        nrosRuc = new string[] { };
                        archivoLeer = filedialog.FileName;
                        lblArchivo.Text = archivoLeer;
                        StreamReader streaReader = new StreamReader(archivoLeer);
                        string cadena = streaReader.ReadToEnd();
                        nrosRuc = cadena.Split(',');
                    } 
                }
                else {
                    button2.Text = "IMPORTAR";

                    List<Dictionary<string, string>> obj = new List<Dictionary<string, string>>();

                    for(int i=0;i<nrosRuc.Length;i++) {
                        obj.Add(GetDistribuyentes(nrosRuc[i]));
                    }

                    frmSunatMultiple frmsunat = new frmSunatMultiple(obj);
                    frmsunat.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private Dictionary<string,string> GetDistribuyentes(string nroDoc) {
            try
            {
                genCaptcha();

                string myurl = @"http://www.sunat.gob.pe/cl-ti-itmrconsruc/jcrS00Alias?accion=consPorRuc&nroRuc=" + nroDoc.Trim() + "&codigo=" + captcha.Trim().ToUpper() + "&tipdoc=1";
                HttpWebRequest myWebRequest = (HttpWebRequest)WebRequest.Create(myurl);
                myWebRequest.CookieContainer = cokkie;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

                HttpWebResponse myhttpWebResponse = (HttpWebResponse)myWebRequest.GetResponse();
                Stream myStream = myhttpWebResponse.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myStream);
                string xDat = ""; int pos = 0;

                Dictionary<string, string> datos = new Dictionary<string, string>();

              //  txtnroRucValue.Text = txtNroDoc.Text;
                datos.Add("RucContribuyente", nroDoc);
                while (!myStreamReader.EndOfStream)
                {
                    xDat = myStreamReader.ReadLine();
                    pos++;

                    switch (pos)
                    {
                        case 126:
                            datos.Add("TipoContribuyente", getDatafromXML(xDat, 25));
                            break;
                        case 131:
                            datos.Add("NombreComercial", getDatafromXML(xDat, 25));
                            break;
                        case 136:
                            datos.Add("FechaInscripcion", getDatafromXML(xDat, 25));
                            break;
                        case 138:
                            datos.Add("FechaInicioActividad", getDatafromXML(xDat, 25));
                            break;
                        case 142:
                            datos.Add("EstadoContribuyente", getDatafromXML(xDat, 25));
                            break;
                        case 152:
                            datos.Add("CondicionContribuyente", getDatafromXML(xDat, 0));
                            break;
                        case 158:
                            datos.Add("DireccionContribuyente", getDatafromXML(xDat, 25));
                            break;
                        case 176:
                            datos.Add("AvtividadComercioEx", getDatafromXML(xDat, 25));
                            break;
                    }
                }
                return datos;
            }
            catch (Exception ex)
            {
                return new Dictionary<string, string>();
            }
        }




    }
}
