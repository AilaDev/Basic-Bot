using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Basic_Bot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            if (activity.Type == ActivityTypes.ConversationUpdate)
            {
                if (activity.MembersAdded != null && activity.MembersAdded.Any())
                {
                    string messageText = "";
                    foreach (var member in activity.MembersAdded){
                        if (member.Id != activity.Recipient.Id)
                        {
                            messageText = $"Welcome to the server, {member.Name}! <br/>" +
                                   $"Please type /help for a list of instructions! :)";
                        }
                        else
                        {
                            messageText = $"Hi, my name is {member.Name}. I'm a bot here to help you!";
                        }
                    }
                    await context.PostAsync(messageText);
                }
            }else if (activity.Text == "/help")
            {
                await context.PostAsync("THERE IS NO HELP FOR YOU!!!");
            }
            //The Fallback Call if the bot can't do anything about the message
            else
            {
                //Apologies
                await context.PostAsync($"My apologies {activity.From.Name} I have not been taught how to deal with that yet. <br/>" +
                                        $"You can use /help to see what I can do :)");
            }
            context.Wait(MessageReceivedAsync);
        }
    }
}