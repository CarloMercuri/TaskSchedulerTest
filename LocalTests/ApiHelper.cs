using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalTests
{
    public static class ApiHelper
    {

        private static string apiKey = "wSmewivOVUmWSvnOHHXJviUYobByYQYe";

        private static string s = "";

        public static string[] RemoveUnusedParameters(string query, string[] parameters)
        {
            List<string> parameterList = new List<string>();

            foreach (string parameter in parameters)
            {
                if (query.Contains(parameter, StringComparison.CurrentCultureIgnoreCase))
                    parameterList.Add(parameter);
            }

            return parameterList.ToArray();
        }

     
    }
}
