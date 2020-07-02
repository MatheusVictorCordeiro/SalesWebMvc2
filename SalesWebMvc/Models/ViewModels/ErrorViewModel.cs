using System;

namespace SalesWebMvc2.Models.ViewModels
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }


        //esse atributo foi criado manualmente para podermos adcionar uma mensagem de erro
        public string Message { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}