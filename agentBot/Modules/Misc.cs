using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace agentBot.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        // Echo's a message a user sends.
        [Command("echo")]
        public async Task Echo([Remainder]string message)
        {
            var embed = new EmbedBuilder();
            embed.WithTitle("Message by " + Context.User.Username);
            embed.WithDescription(message);
            embed.WithColor(new Color(0, 255, 0));

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        // return a random option EG: !random option1/option2/option3
        [Command("random")]
        public async Task Random([Remainder]string message)
        {
            string[] options = message.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            Random r = new Random();
            string selection = options[r.Next(0, options.Length)];

            var embed = new EmbedBuilder();
            embed.WithTitle(Context.User.Username + ", I will pick something for you..");
            embed.WithDescription(selection);
            embed.WithColor(new Color(255, 255, 0));
            embed.WithThumbnailUrl("https://i.pinimg.com/originals/9e/d5/a0/9ed5a02fc7df5927cbdca1c8b0ed0ecd.jpg");

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        //This is a test command to test permissions, and send a DM message.
        [Command("secret")]
        public async Task Secret([Remainder] string arg = "")
        {
            if (!UserHaveRole((SocketGuildUser)Context.User))
            {
                await Context.Channel.SendMessageAsync(Context.User.Mention + "You do not have permission to use this command.");
                return;
            }
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
            await dmChannel.SendMessageAsync(Utilities.GetAlert("This is a secret message!"));
        }

        // Check if a user has a role, by name.
        private bool UserHaveRole(SocketGuildUser user)
        {
            string targetRoleName = "Developer";
            var result = from r in user.Guild.Roles
                         where r.Name == targetRoleName
                         select r.Id;
            ulong roleID = result.FirstOrDefault();
            if (roleID == 0) return false;
            var targetRole = user.Guild.GetRole(roleID);
            return user.Roles.Contains(targetRole);
        }
    }
}
