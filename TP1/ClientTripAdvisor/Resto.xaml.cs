using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace ClientTripAdvisor
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private String GetFromUrl(String Url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

            // Set some reasonable limits on resources used by this request 
            request.MaximumAutomaticRedirections = 4;
            request.MaximumResponseHeadersLength = 4;
            // Set credentials to use for this request. 
            request.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Console.WriteLine("Content length is {0}", response.ContentLength);
            Console.WriteLine("Content type is {0}", response.ContentType);

            // Get the stream associated with the response. 
            Stream receiveStream = response.GetResponseStream();

            // Pipes the stream to a higher level stream reader with the required encoding format.  
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

            String result = readStream.ReadToEnd();
            response.Close();
            readStream.Close();
            return result;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            resultTextbox.Text = GetFromUrl("http://localhost:1918/api/Restaurant/");
        }

        private void fillCombo_Click(object sender, RoutedEventArgs e)
        {
    
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(resultTextbox.Text, new DataTableConverter());
            if (dt != null)
            {
                dataGridListResto.DataContext = dt.DefaultView;
                ListRestaurateurCombo.DataContext = dt.DefaultView;
                ListRestaurateurToEditCombo.DataContext = dt.DefaultView;
                ListRestaurateurToDeleteCombo.DataContext = dt.DefaultView;
            }
        }

        private void afficherUnRestButt_Click(object sender, RoutedEventArgs e)
        {
            if (ListRestaurateurCombo.SelectedItem != null)
            {
                DataRow row = (ListRestaurateurCombo.SelectedItem as DataRowView).Row;
                prixRestoTxt.Text = row.Field<string>("PRX_PRIX");
                nomRestoTxt.Text = row.Field<string>("RES_NOM");
                categPrixRestoTxt.Text = row.Field<string>("res_categorieprix");
                descRestoTxt.Text = row.Field<string>("RES_DESCRIPTION");
                add1RestoTxt.Text = row.Field<string>("res_adr");
                add2RestoTxt.Text = row.Field<string>("RES_CP");
                add3RestoTxt.Text = row.Field<string>("RES_VILLE");
                add4RestoTxt.Text = row.Field<string>("RES_PAYS");
                indicatifTxt.Text = ((Int64) row["ind_indicatif"]).ToString();
                telTxt.Text = row.Field<string>("res_tel");
                mailTxt.Text = row.Field<string>("res_mel");
                webTxt.Text = row.Field<string>("res_siteweb");

            }
        }

        private void ajouterResBut_Click(object sender, RoutedEventArgs e)
        {
            JObject objet= new JObject(new JProperty("PRX_PRIX", NewPrixRestoTxt.Text),
                new JProperty("RES_NOM", NewNomRestoTxt.Text),
                new JProperty("res_categorieprix", NewCategPrixRestoTxt.Text),
                new JProperty("RES_DESCRIPTION", NewDescRestoTxt.Text),
                new JProperty("res_adr", NewAdd1RestoTxt.Text),
                new JProperty("RES_CP", NewAdd2RestoTxt.Text),
                new JProperty("RES_VILLE", NewAdd3RestoTxt.Text),
                new JProperty("RES_PAYS", NewAdd4RestoTxt.Text),
                new JProperty("ind_indicatif", Int64.Parse(NewIndicatifTxt.Text)),
                new JProperty("res_tel", NewTelTxt.Text),
                new JProperty("res_mel", NewMailTxt.Text),
                new JProperty("res_siteweb", NewWebTxt.Text));

            PostFromUri("http://localhost:1918/api/Restaurant/", objet);
           
        }

        private void ModifierResBut_Click(object sender, RoutedEventArgs e)
        {
            JObject objet = new JObject(
              new JProperty("RES_ID", ListRestaurateurToEditCombo.SelectedValue),
              new JProperty("PRX_PRIX", EditPrixRestoTxt.Text),
              new JProperty("RES_NOM", EditNomRestoTxt.Text),
              new JProperty("res_categorieprix", EditCategPrixRestoTxt.Text),
              new JProperty("RES_DESCRIPTION", EditDescRestoTxt.Text),
              new JProperty("res_adr", EditAdd1RestoTxt.Text),
              new JProperty("RES_CP", EditAdd2RestoTxt.Text),
              new JProperty("RES_VILLE", EditAdd3RestoTxt.Text),
              new JProperty("RES_PAYS", EditAdd4RestoTxt.Text),
              new JProperty("ind_indicatif", Int64.Parse(EditIndicatifTxt.Text)),
              new JProperty("res_tel", EditTelTxt.Text),
              new JProperty("res_mel", EditMailTxt.Text),
              new JProperty("res_siteweb", EditWebTxt.Text));

            UpdateFromUri("http://localhost:1918/api/Restaurant/"+ ListRestaurateurToEditCombo.SelectedValue, objet);
        }

        private void SupprimerUnRestToEditButt_Click(object sender, RoutedEventArgs e)
        {
            DeleteFromUri("http://localhost:1918/api/Restaurant/" + ListRestaurateurToDeleteCombo.SelectedValue);
        }


        private void AfficherUnRestToEditButt_Click(object sender, RoutedEventArgs e)
        {
            if (ListRestaurateurToEditCombo.SelectedItem != null)
            {
                DataRow row = (ListRestaurateurToEditCombo.SelectedItem as DataRowView).Row;
                EditPrixRestoTxt.Text = row.Field<string>("PRX_PRIX");
                EditNomRestoTxt.Text = row.Field<string>("RES_NOM");
                EditCategPrixRestoTxt.Text = row.Field<string>("res_categorieprix");
                EditDescRestoTxt.Text = row.Field<string>("RES_DESCRIPTION");
                EditAdd1RestoTxt.Text = row.Field<string>("res_adr");
                EditAdd2RestoTxt.Text = row.Field<string>("RES_CP");
                EditAdd3RestoTxt.Text = row.Field<string>("RES_VILLE");
                EditAdd4RestoTxt.Text = row.Field<string>("RES_PAYS");
                EditIndicatifTxt.Text = ((Int64)row["ind_indicatif"]).ToString();
                EditTelTxt.Text = row.Field<string>("res_tel");
                EditMailTxt.Text = row.Field<string>("res_mel");
                EditWebTxt.Text = row.Field<string>("res_siteweb");
            }
        }



        private void UpdateFromUri(string url, JObject objet)
        {
            byte[] postBytes = Encoding.UTF8.GetBytes(objet.ToString());

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "PUT";
            request.ContentType = "application/json";
            request.ContentLength = postBytes.Length;

            request.MaximumAutomaticRedirections = 4;
            request.MaximumResponseHeadersLength = 4;

            // Set credentials to use for this request. 
            request.Credentials = CredentialCache.DefaultCredentials;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(postBytes, 0, postBytes.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            Console.WriteLine(responseString);
        }


        private void DeleteFromUri(string url)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "DELETE";


            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            Console.WriteLine(responseString);
        }

        private void PostFromUri(string url, JObject objet)
        {
            byte[] postBytes = Encoding.UTF8.GetBytes(objet.ToString());

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = postBytes.Length;

            request.MaximumAutomaticRedirections = 4;
            request.MaximumResponseHeadersLength = 4;
            // Set credentials to use for this request. 
            request.Credentials = CredentialCache.DefaultCredentials;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(postBytes, 0, postBytes.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            Console.WriteLine(responseString);
        }

        private void RechercherButton_Click(object sender, RoutedEventArgs e)
        {
            if (NomRestaurantText.Text != String.Empty)
            {
                String result = GetFromUrl("http://localhost:1918/api/Restaurant?name="+ NomRestaurantText.Text+"&exact="+ ExactCheckBox.IsChecked);
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(result, new DataTableConverter());
                if (dt != null)
                {
                    DataSearchGridListResto.DataContext = dt.DefaultView;
                }
            }
        }
    }
}
