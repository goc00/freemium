using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Wach.Exceptions;

namespace Wach
{
    public class WachResult<T>
    {
        //mensaje de respuesta
        HttpResponseMessage ResponseMessage { get; set; }

        //objeto de respuesta
        public T Result { get; set; }
    }

    public class WachHelper
    {
        private HttpClient client { get; set; }

        public WachHelper(string urlbase)
        {
            this.client = new HttpClient();
            client.BaseAddress = new Uri(urlbase);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public T Get<T>(string url)
        {
            try {
                if (this.client == null)
                {
                    //excepcion aqui
                    throw new WachNoClientConfiguredException("No se seteó la url del servidor.", null);
                }

                //todo ok
                HttpResponseMessage response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<T>().Result;
                }
                else
                {
                    //excepcion aqui
                    throw new WachServerErrorException("Error del servidor", null);
                }

                
            } 
            catch (Exception ex)
            {
                Exception temp_ex = ex;
                do
                {
                    if (temp_ex is Newtonsoft.Json.JsonSerializationException)
                        throw new WachSerializationException("No se logró Serializar el objeto. Puede ser que el objeto retornado sea de un tipo distinto al esperado", ex);

                    if (temp_ex is System.Net.WebException)
                        throw new WachConnectionException("No se logró conectar a la API o a su metodo.", ex);

                    temp_ex = temp_ex.InnerException;
                } while (temp_ex!=null);

                throw new WachException("Excepción desconocida.", ex);
            }
            
        }

        public T Post<T>(string url, object body)
        {
            try
            {
                if (this.client == null)
                {
                    //excepcion aqui
                    throw new WachNoClientConfiguredException("No se seteó la url del servidor.", null);
                }

                //todo ok
                HttpResponseMessage response = client.PostAsJsonAsync(url, body).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<T>().Result;
                }
                else
                {
                    //excepcion aqui
                    throw new WachServerErrorException("Error del servidor", null);
                }


            }
            catch (Exception ex)
            {
                Exception temp_ex = ex;
                do
                {
                    if (temp_ex is Newtonsoft.Json.JsonSerializationException)
                        throw new WachSerializationException("No se logró Serializar el objeto. Puede ser que el objeto retornado sea de un tipo distinto al esperado", ex);

                    if (temp_ex is System.Net.WebException)
                        throw new WachConnectionException("No se logró conectar a la API o a su metodo.", ex);

                    temp_ex = temp_ex.InnerException;
                } while (temp_ex != null);

                throw new WachException("Excepción desconocida.", ex);
            }
        }

        public T PostSimple<T>(string url, HttpContent body)
        {
            try
            {
                if (this.client == null)
                {
                    //excepcion aqui
                    throw new WachNoClientConfiguredException("No se seteó la url del servidor.", null);
                }

                //todo ok
                HttpResponseMessage response = client.PostAsync(url, body).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<T>().Result;
                }
                else
                {
                    //excepcion aqui
                    throw new WachServerErrorException("Error del servidor", null);
                }


            }
            catch (Exception ex)
            {
                Exception temp_ex = ex;
                do
                {
                    if (temp_ex is Newtonsoft.Json.JsonSerializationException)
                        throw new WachSerializationException("No se logró Serializar el objeto. Puede ser que el objeto retornado sea de un tipo distinto al esperado", ex);

                    if (temp_ex is System.Net.WebException)
                        throw new WachConnectionException("No se logró conectar a la API o a su metodo.", ex);

                    temp_ex = temp_ex.InnerException;
                } while (temp_ex != null);

                throw new WachException("Excepción desconocida.", ex);
            }
        }

        public T Put<T>(string url, object body)
        {
            try
            {
                if (this.client == null)
                {
                    //excepcion aqui
                    throw new WachNoClientConfiguredException("No se seteó la url del servidor.", null);
                }

                //todo ok
                HttpResponseMessage response = client.PutAsJsonAsync(url, body).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<T>().Result;
                }
                else
                {
                    //excepcion aqui
                    throw new WachServerErrorException("Error del servidor", null);
                }


            }
            catch (Exception ex)
            {
                Exception temp_ex = ex;
                do
                {
                    if (temp_ex is Newtonsoft.Json.JsonSerializationException)
                        throw new WachSerializationException("No se logró Serializar el objeto. Puede ser que el objeto retornado sea de un tipo distinto al esperado", ex);

                    if (temp_ex is System.Net.WebException)
                        throw new WachConnectionException("No se logró conectar a la API o a su metodo.", ex);

                    temp_ex = temp_ex.InnerException;
                } while (temp_ex != null);

                throw new WachException("Excepción desconocida.", ex);
            }
        }

    }
}
