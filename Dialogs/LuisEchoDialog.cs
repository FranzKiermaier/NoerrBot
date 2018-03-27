using System;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;
using SimpleEchoBot.Dialogs;
using System.Threading;
using SimpleEchoBot.Domain;

namespace Microsoft.Bot.Sample.SimpleEchoBot
{
    [Serializable]
    public class LuisEchoDialog : IDialog<object>
    {
        protected int count = 1;

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            await context.Forward(new LuisProductDialog(), AfterProductSelectionDone, message, CancellationToken.None);
        }

        //private static async Task ShowRootOptions(IDialogContext context)
        //{
        //    await context.PostAsync("Welcome, how can I help you?");
        //    await context.PostAsync("Looking for products? Managing your Basket? Or want to check out?");
        //}

        //private async Task GetUsersRootOptionSelection(IDialogContext context, IAwaitable<IMessageActivity> result)
        //{
        //    var message = await result;
        //    if (message.Text.ToLower().Contains("products"))
        //    {
        //        await context.Forward(new ProductDialog(), AfterProductSelectionDone, message, CancellationToken.None);
        //    }
        //    else
        //    {
        //        await ShowRootOptions(context);
        //    }
        //}

        private async Task AfterProductSelectionDone(IDialogContext context, IAwaitable<object> result)
        {
            var messageObject = await result;
            var product = (Product)messageObject;
            if(!string.IsNullOrEmpty(product.ProductID))
            {
                await context.PostAsync($"you have ordered {product.Name}");
                await context.PostAsync("Looking for products? Managing your Basket? Or want to check out?");
            }
            else
            {
                await context.PostAsync($"No product selected");
            }
        }

    }
}