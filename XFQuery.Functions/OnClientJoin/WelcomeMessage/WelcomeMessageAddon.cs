using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using TS3QueryLib.Net.Core.Common.CommandHandling;
using TS3QueryLib.Net.Core.Server.Commands;
using TS3QueryLib.Net.Core.Server.Notification.EventArgs;
using TS3QueryLib.Net.Core.Server.Responses;
using XFQuery.Core;
using XFQuery.Core.Logging;

namespace XFQuery.Functions.OnClientJoin.WelcomeMessage
{
    internal class WelcomeMessageFunction
    {
        private readonly WelcomeMessage _addon;
        private readonly WelcomeMessageDto _config;
        private readonly ILogger _log = Interface.XfQueryBot.Logger;

        public WelcomeMessageFunction(WelcomeMessage addon, WelcomeMessageDto config)
        {
            _addon = addon;
            _config = config;
        }

        public async Task WelcomeMessage(ClientJoinedEventArgs e)
        {
            try
            {
                var clientInfo =
                    await new ClientInfoCommand(e.ClientId).ExecuteAsync(Interface.XfQueryBot.QueryClient);
                if (clientInfo.TotalConnections <= 1)
                    _config.FirstConnectionMessage[clientInfo.ClientCountry.ToLower()].ForEach(msg =>
                        new SendTextMessageCommand(MessageTarget.Client, e.ClientId, MessageBuilder(msg, clientInfo))
                            .ExecuteAsync(Interface.XfQueryBot.QueryClient));
                else
                    _config.ConnectionMessage[clientInfo.ClientCountry.ToLower()].ForEach(msg =>
                        new SendTextMessageCommand(MessageTarget.Client, e.ClientId, MessageBuilder(msg, clientInfo))
                            .ExecuteAsync(Interface.XfQueryBot.QueryClient));
            }
            catch (KeyNotFoundException)
            {
                var clientInfo = await new ClientInfoCommand(e.ClientId).ExecuteAsync(Interface.XfQueryBot.QueryClient);
                if (clientInfo.TotalConnections <= 1)
                    _config.FirstConnectionMessage[_config.DefaultLang].ForEach(msg =>
                        new SendTextMessageCommand(MessageTarget.Client, e.ClientId, MessageBuilder(msg, clientInfo))
                            .ExecuteAsync(Interface.XfQueryBot.QueryClient));
                else
                    _config.ConnectionMessage[_config.DefaultLang].ForEach(msg =>
                        new SendTextMessageCommand(MessageTarget.Client, e.ClientId, MessageBuilder(msg, clientInfo))
                            .ExecuteAsync(Interface.XfQueryBot.QueryClient));
            }
            catch (Exception ex)
            {
                _log.Error($"({_addon.Name}) : {ex}");
            }

            string MessageBuilder(string msg, ClientInfoCommandResponse clientInfo)
            {
                var replaced = msg.Replace("[Nickname]", clientInfo.Nickname)
                    .Replace("[DatabaseId]", clientInfo.DatabaseId.ToString())
                    .Replace("[TotalConnectionCount]", clientInfo.TotalConnections.ToString())
                    .Replace("[LastConnected]", clientInfo.LastConnected.ToString(CultureInfo.InvariantCulture))
                    .Replace("[Created]", clientInfo.Created.ToString(CultureInfo.InvariantCulture))
                    .Replace("[Platform]", clientInfo.Platform)
                    .Replace("[ConnectionIp]", clientInfo.ClientIP)
                    .Replace("[Version]", clientInfo.Version)
                    .Replace("[UniqueIdentifier]", clientInfo.UniqueId);
                return replaced;
            }
        }
    }
}