using Microsoft.Bot.Builder.Dialogs;
using SimpleEchoBot.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using SimpleEchoBot.Services;
using SimpleEchoBot.Forms;
using System.Threading;
using Microsoft.Bot.Builder.FormFlow;

namespace SimpleEchoBot.Dialogs
{
    [Serializable]
    public class ProductDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(ShowProductForm);
        }

        private async Task ShowProductForm(IDialogContext context, IAwaitable<IMessageActivity> message)
        {
            var result = await message;
            await context.Forward(FormDialog.FromForm(ProductForm.BuildForm), ShowProductListAndSelect, result);
        }

        private async Task ShowProductListAndSelect(IDialogContext context, IAwaitable<object> result)
        {
            var message = ((ProductForm)await result);
            var search = new AzureSearch(FluentSearchClient.CreateSearchIndexClient());
            var products = search.RunQuery(message.ProductName);
            switch(message.SortOrder)
            {
                case SortOrder.Ascending:
                    {
                        products = products.OrderBy(p => p.Name);
                        break;
                    }
                case SortOrder.Descending:
                    {
                        products = products.OrderByDescending(p => p.Name);
                        break;
                    }
            }
            products = products.Take(message.NumberOfResults);

            PromptDialog.Choice(context, AfterProductSelected, products, "Please select a product to add it to the cart.");
        }

        private async Task AfterProductSelected(IDialogContext context, IAwaitable<Product> result)
        {
            var message = await result;
            context.Done((Product)message);
        }
    }
}