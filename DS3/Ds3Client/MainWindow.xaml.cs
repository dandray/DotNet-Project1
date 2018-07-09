using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Ds3Client
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataTable _dtEquipe;
        private DataTable _dtCoureur;

        public MainWindow()
        {
            InitializeComponent();

            OnLoadingVue();
        }

        private void OnLoadingVue()
        {
            // Récupére la liste des coureurs et equipes
            String resultListEquipes = GetFromUrl("http://localhost:1654/api/equipes/");
            String resultListCoureur = GetFromUrl("http://localhost:1654/api/coureurs/");

            // Créer les dataDable 
            _dtEquipe = JsonConvert.DeserializeObject<DataTable>(resultListEquipes, new DataTableConverter());
            _dtCoureur = JsonConvert.DeserializeObject<DataTable>(resultListCoureur, new DataTableConverter());

            // Charger la liste des coureurs
            if (_dtEquipe != null)
            {
                DataGridListEquipes.DataContext = _dtEquipe.DefaultView;
            }
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

            // Get the stream associated with the response. 
            Stream receiveStream = response.GetResponseStream();

            // Pipes the stream to a higher level stream reader with the required encoding format.  
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

            String result = readStream.ReadToEnd();
            response.Close();
            readStream.Close();
            return result;
        }

        private void RechercherButton_Click(object sender, RoutedEventArgs e)
        {
            if (NomEquipeRecherche.Text != String.Empty)
            {
                String resultEquipe = GetFromUrl("http://localhost:1654/api/equipes?name=" + NomEquipeRecherche.Text);
                // Transformer en Json object
                JObject equipeJObject = JObject.Parse(resultEquipe);
                
                // Si trouvé
                if (equipeJObject.Count > 0)
                {
                    TextCodeEquipe.Content = equipeJObject.GetValue("code");
                }

                //DataTable dt = JsonConvert.DeserializeObject<DataTable>(result, new DataTableConverter());
                //if (dt != null)
                //{
                //    DataSearchGridListResto.DataContext = dt.DefaultView;
                //}
            }
        }
    }
}
