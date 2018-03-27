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
using AdaptiveCards;

namespace Microsoft.Bot.Sample.SimpleEchoBot
{
    [Serializable]
    public class AdaptiveCardsEchoDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await ShowProductForm(context);
            context.Wait(GetProductSelection);
        }

        private async Task GetProductSelection(IDialogContext context, IAwaitable<object> result)
        {
            context.Done(new Product());
        }

        private async Task ShowProductForm(IDialogContext context)
        {
            var productMessage = context.MakeMessage();
            productMessage.Attachments.Add(new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = BuildQueryCard()
            });
            await context.PostAsync(productMessage);
        }

        private AdaptiveCard BuildQueryCard()
        {
            AdaptiveCard card = new AdaptiveCard()
            {
                Title = "Please enter search parameters",
                Body = new List<CardElement>()
                {
                    
                    new Container()
                    {
                        Speak = "Please enter search parameters for products",
                        Items = new List<CardElement>()
                        {
                            
                        }
                    }
                }
            };
            return card;
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