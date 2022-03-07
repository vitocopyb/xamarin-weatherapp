using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherApp.Models;
using Xamarin.Forms;

namespace WeatherApp.ViewModels
{
    public class WeatherPageViewModel : INotifyPropertyChanged
    {
        private WeatherData data;
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        protected virtual void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        public WeatherData Data
        {
            get => data;
            set
            {
                data = value;
                OnPropertyChanged();
            }
        }

        public ICommand SearchCommand { get; set; }

        public WeatherPageViewModel()
        {
            SearchCommand = new Command(async (searchTerm) =>
            {
                //await GetData("https://api.weatherbit.io/v2.0/current?lat=-33.0487793&lon=-71.4360809&key=acd95631f74941bb815088efd837af1b&lang=es");

                string term = (string)searchTerm;
                string[] datos;

                if (term == "kiev")
                {
                    term = "50.401699,30.2525103";
                }
                else if (term == "pta")
                {
                    term = "-53.1438105,-70.9656302";
                }
                else if (term == "arica")
                {
                    term = "-18.4980624,-70.3505407";
                }
                else if (term == "paris")
                {
                    term = "48.845,2.333";
                }
                else
                {
                    datos = term.Split(',');
                    if (datos.Length == 1)
                    {
                        // por defecto quilpue
                        term = "-33.0487793,-71.4360809";
                    }
                }

                // separa los datos lat, lon
                datos = term.Split(',');
                string lat = datos[0].Trim();
                string lon = datos[1].Trim();
                string latlon = $"lat={lat}&lon={lon}";
                string url = $"https://api.weatherbit.io/v2.0/current?key=acd95631f74941bb815088efd837af1b&lang=es&{latlon}";
                await GetData(url);
            });
        }


        private async Task GetData(string url)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var jsonResult = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<WeatherData>(jsonResult);
            Data = result;
        }
    }
}
