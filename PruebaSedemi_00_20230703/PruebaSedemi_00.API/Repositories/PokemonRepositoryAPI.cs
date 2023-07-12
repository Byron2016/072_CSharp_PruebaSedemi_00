using Newtonsoft.Json;
using PruebaSedemi_00.API.Enums;
using PruebaSedemi_00.API.Models;
using System.Net;
using System.Reflection.PortableExecutable;

namespace PruebaSedemi_00.API.Repositories
{
    public class PokemonRepositoryAPI : IPokemonRepositoryAPI
    {
        public PokemonRecordAPI GetItems(string url)
        {
            // Create a request for the URL. 
            var request = (HttpWebRequest)WebRequest.Create(url);
            // If required by the server, set the credentials.
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";

            ErrorAPI errorStatus = new ErrorAPI();
            PokemonRecordAPI pokemonRecordAPI = new PokemonRecordAPI();

            try
            {
                if (url.Length == 0 || url == null)
                {
                    return ErrorManger(0);
                }

                // Get the response.
                using (WebResponse response = request.GetResponse())
                {
                    // Display the status.
                    // Console.WriteLine(response.StatusDescription);
                    // Get the stream containing content returned by the server.
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null)
                        {
                            return ErrorManger(2);
                        }
                        // Open the stream using a StreamReader for easy access.
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            // Read the content.
                            string responseBody = objReader.ReadToEnd();

                            if (responseBody != null && responseBody.Length >= 0)
                            {
                                errorStatus.ErrorCode = ErrorsEnumAPI.NoError;
                                pokemonRecordAPI.ErrorStatus = errorStatus;

                                pokemonRecordAPI = JsonConvert.DeserializeObject<PokemonRecordAPI>(responseBody);


                                // Cleanup the streams and the response.
                                // objReader.Close();
                                // strReader.Close();
                                // strReader.Close();

                                return pokemonRecordAPI;
                            }

                            return ErrorManger(2);

                        }
                    }
                }

            }
            catch (WebException e)
            {
                errorStatus.ErrorCode = ErrorsEnumAPI.CatchError;
                errorStatus.ErrorMsg = e.Message;

                pokemonRecordAPI.ErrorStatus = errorStatus;

                return pokemonRecordAPI;
            }
        }

        private PokemonRecordAPI ErrorManger(int errorCase)
        {
            ErrorAPI errorStatus = new ErrorAPI();
            PokemonRecordAPI pokemonRecordAPI = new PokemonRecordAPI();

            switch (errorCase)
            {
                case 0:
                    errorStatus.ErrorCode = ErrorsEnumAPI.NoUrl;
                    errorStatus.ErrorMsg = "Without a valid URL";
                    pokemonRecordAPI.ErrorStatus = errorStatus;
                    break;

                case 2:
                    errorStatus.ErrorCode = ErrorsEnumAPI.NoData;
                    errorStatus.ErrorMsg = "Without Data";
                    pokemonRecordAPI.ErrorStatus = errorStatus;
                    break;

                default:
                    errorStatus.ErrorCode = ErrorsEnumAPI.OtherError;
                    errorStatus.ErrorMsg = "Other Error";
                    pokemonRecordAPI.ErrorStatus = errorStatus;
                    break;
            }

            return pokemonRecordAPI;

        }
    }
}