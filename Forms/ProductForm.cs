using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleEchoBot.Forms
{
    public enum SortOrder
    {
        Ascending,
        Descending
    }

    [Serializable]
    public class ProductForm
    {
        [Prompt("Which product do you want to order")]
        public string ProductName { get; set; }
        [Prompt("How many products do you want to display?")]
        public int NumberOfResults { get; set; }
        
        [Prompt("How do you want to sort the results? {||}")]
        public SortOrder? SortOrder { get; set; }

        public static IForm<ProductForm> BuildForm()
        {
            return new FormBuilder<ProductForm>()
                .Message("Please enter the data, necessary for product search")
                .AddRemainingFields()
                .Build();
        }
    }
}