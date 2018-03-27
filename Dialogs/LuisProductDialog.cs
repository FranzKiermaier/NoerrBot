using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using SimpleEchoBot.Domain;
using SimpleEchoBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Microsoft.Bot.Sample.SimpleEchoBot
{
    [Serializable]
    public class LuisProductDialog : LuisDialog<object>
    {
        public LuisProductDialog() : base(new LuisService(new LuisModelAttribute(
            "7f03df2e-35eb-4f69-bf7c-d3cb0e172ee6",
            "d9815e6348914ffe972fe402e51a818e", 
            domain: "westeurope.api.cognitive.microsoft.com")))
        {
        }

        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        [LuisIntent("FindProduct")]
        public async Task FindProductIntent(IDialogContext context, LuisResult result)
        {
            var color = result.Entities.Where(e => e.Type.CompareTo("Color") == 0).FirstOrDefault();
            var product = result.Entities.Where(e => e.Type.CompareTo("Product") == 0).FirstOrDefault();
            var search = new AzureSearch(FluentSearchClient.CreateSearchIndexClient());

            IQueryable<Product> products = new List<Product>().AsQueryable();
            if(product != null)
            {
                if(color != null)
                {
                    products = search.RunQuery(product.Entity, color.Entity);
                }
                else
                {
                    products = search.RunQuery(product.Entity);
                }
            }
            if(products.Any())
            {
                PromptDialog.Choice(context, AfterProductSelected, products, "Please select a product to add it to the cart.");
            }
            else
            {
                await context.PostAsync("No results found. Please try again...");
                context.Done(new Product());
            }

        }

        private async Task AfterProductSelected(IDialogContext context, IAwaitable<Product> result)
        {
            var message = await result;
            context.Done((Product)message);
        }

        private async Task ShowLuisResult(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"You have reached {result.Intents[0].Intent}. You said: {result.Query}");
            context.Done(new Product());
        }
    }
}