using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Deneme
{
    class Helper
    {
        private static readonly HttpClient client = new HttpClient();
        private string url = "http://postajansdemo.com/genmot/apiv1/services/";

        public async Task<string> getProduct( string gmCode )
        {
            try
            {
                var values = new Dictionary<string, string>
                {
                    {"gm", gmCode}
                };

                var content = new FormUrlEncodedContent( values );
                var response = await client.PostAsync( url + "getOurProducts", content );
                var responseString = await response.Content.ReadAsStringAsync();
                return responseString.ToString();
            }
            catch ( Exception err )
            {
                MessageBox.Show(err + ""); 
                return err.ToString();
            }
        }

        public async Task<string> saveProduct( string id, string qr )
        {
            try
            {
                var values = new Dictionary<string, string>
                {
                    {"product_id", id},
                    {"qr", qr}
                };

                var content = new FormUrlEncodedContent( values );
                var response = await client.PostAsync( url + "saveProduct", content );
                var responseString = await response.Content.ReadAsStringAsync();
                return responseString.ToString();
            }
            catch ( Exception err )
            {
                MessageBox.Show( err + "" );
                return err.ToString();
            }
        }

        public async Task<string> login(string pass)
        {
            try
            {
                var values = new Dictionary<string, string>
                {
                    {"login", pass}
                };

                var content = new FormUrlEncodedContent(values);
                var response = await client.PostAsync(url + "loginMe", content);
                var responseString = await response.Content.ReadAsStringAsync();
                return responseString.ToString();
            }
            catch (Exception err)
            {
                MessageBox.Show(err + "");
                return err.ToString();
            }
        }

    }
}
