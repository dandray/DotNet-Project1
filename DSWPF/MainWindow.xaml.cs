using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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

namespace DSWPF
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataTable musicDataTable;
        private DataTable avisDataTable;

        public MainWindow()
        {
            InitializeComponent();

            string listMusicFlux = GetFromUrl("http://localhost:49580/api/T_E_MUSIQUE_MUS/");
            string listAvisFlux = GetFromUrl("http://localhost:49580/api/T_E_AVIS_AVI/");

            musicDataTable = JsonConvert.DeserializeObject<DataTable>(listMusicFlux, new DataTableConverter());
            avisDataTable = JsonConvert.DeserializeObject<DataTable>(listAvisFlux, new DataTableConverter());

            //Load music
            LoadAllMusic();

        }

        private void LoadAllMusic()
        {
            if (musicDataTable != null)
            {
                dataGridListMusiques.DataContext = musicDataTable.DefaultView;
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

        private void AfficherUneMusique_Click(object sender, RoutedEventArgs e)
        {
            string MusicFlux = GetFromUrl("http://localhost:49580/api/T_E_MUSIQUE_MUS?titre=" +titreMusique.Text);
            DataTable musicData = JsonConvert.DeserializeObject<DataTable>(MusicFlux, new DataTableConverter());
            dataGridListMusique.DataContext = musicData.DefaultView;
        }
    }
}

