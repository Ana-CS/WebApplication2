using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace WebApplication2.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            var client = new RestClient("http://copafilmes.azurewebsites.net");
            var request = new RestRequest("/api/filmes", Method.GET);
            var queryResult = client.Execute<List<Dictionary<string, string>>>(request).Data;

            int id = 0;
            foreach (Dictionary<string, string> x in queryResult)
            {
                string titulo = "";

                foreach (KeyValuePair<string, string> item in x)
                {
                    if (item.Key == "titulo")
                    {
                        titulo = item.Value;
                    }

                }

                TempData[(id).ToString()] = titulo;

                id += 1;
            }

            List<string> vencedor = Chave(queryResult);

            Console.Write($"O vencedor é {vencedor[0]} O vice é {vencedor[1]}");

        }

        public static List<string> Chaveamento(Dictionary<string, string> dicio, List<string> list_titulo)
        {
            int j = 0;
            int len_lista = list_titulo.Count - 1;

            List<string> final = new List<string>();

            List<string> list_melhores = new List<string>();

            if (len_lista > 2)
            {
                while (j <= (list_titulo.Count - 1) / 2)
                {

                    string melhor = "";

                    if (float.Parse(dicio[list_titulo[j]]) > float.Parse(dicio[list_titulo[len_lista]]))
                    {
                        melhor = list_titulo[j];
                    }
                    else if (float.Parse(dicio[list_titulo[j]]) < float.Parse(dicio[list_titulo[len_lista]]))
                    {
                        melhor = list_titulo[len_lista];
                    }
                    else if (float.Parse(dicio[list_titulo[j]]) == float.Parse(dicio[list_titulo[len_lista]]))
                    {
                        int cont = 0;
                        string filme1 = list_titulo[j];
                        string filme2 = list_titulo[len_lista];

                        while (cont != -1)
                        {
                            if (filme1[cont] > filme2[cont])
                            {
                                cont = -1;
                                melhor = filme2;
                            }
                            else if (filme1[cont] < filme2[cont])
                            {
                                cont = -1;
                                melhor = filme1;
                            }
                            else
                            {
                                cont += 1;
                            }
                        }
                    }

                    list_melhores.Add(melhor);

                    j += 1;
                    len_lista -= 1;


                }
                return list_melhores;
            }
            else
            {
                string melhor = "";
                string segundo = "";

                if (float.Parse(dicio[list_titulo[j]]) > float.Parse(dicio[list_titulo[len_lista]]))
                {
                    melhor = list_titulo[j];
                    segundo = list_titulo[len_lista];
                }
                else if (float.Parse(dicio[list_titulo[j]]) < float.Parse(dicio[list_titulo[len_lista]]))
                {
                    melhor = list_titulo[len_lista];
                    segundo = list_titulo[j];
                }
                else if (float.Parse(dicio[list_titulo[j]]) == float.Parse(dicio[list_titulo[len_lista]]))
                {
                    int cont = 0;
                    string filme1 = list_titulo[j];
                    string filme2 = list_titulo[len_lista];

                    while (cont != -1)
                    {
                        if (filme1[cont] > filme2[cont])
                        {
                            cont = -1;
                            melhor = filme2;
                            segundo = filme1;
                        }
                        else if (filme1[cont] < filme2[cont])
                        {
                            cont = -1;
                            melhor = filme1;
                            segundo = filme2;
                        }
                        else
                        {
                            cont += 1;
                        }
                    }
                }

                final.Add(melhor);
                final.Add(segundo);

                return final;
            }


        }

        public static List<string> Chave(List<Dictionary<string, string>> dic_filme)
        {

            Dictionary<string, string> dic_titulos = new Dictionary<string, string>();
            List<string> str_titulos = new List<string>();

            foreach (Dictionary<string, string> x in dic_filme)
            {
                string titulo = "";
                string nota = "";

                foreach (KeyValuePair<string, string> item in x)
                {
                    if (item.Key == "titulo")
                    {
                        titulo = item.Value;
                    }
                    else if (item.Key == "nota")
                    {
                        nota = item.Value;
                    }


                }
                str_titulos.Add(titulo);
                dic_titulos.Add(titulo, nota);
            }

            str_titulos.Sort();
            Dictionary<string, string> titulos_orderned = new Dictionary<string, string>();

            int i = 0;

            while (i < str_titulos.Count)
            {
                string tit = str_titulos[i];

                titulos_orderned.Add(tit, dic_titulos[tit]);

                i += 1;
            }

            List<string> vencedores = new List<string>();

            vencedores = Chaveamento(titulos_orderned, str_titulos);

            while (vencedores.Count > 2)
            {
                vencedores = Chaveamento(titulos_orderned, vencedores);
            }

            return vencedores;


        }



    }
}
