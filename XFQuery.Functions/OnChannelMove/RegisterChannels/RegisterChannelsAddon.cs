using System;
using System.Linq;
using System.Threading.Tasks;
using TS3QueryLib.Net.Core.Common.Entities;
using TS3QueryLib.Net.Core.Server.Commands;
using TS3QueryLib.Net.Core.Server.Notification.EventArgs;
using TS3QueryLib.Net.Core.Server.Responses;
using XFQuery.Core;
using XFQuery.Core.Logging;

namespace XFQuery.Functions.OnChannelMove.RegisterChannels
{
    internal class RegisterChannelsFunction
    {
        private readonly RegisterChannels _addon;
        private readonly RegisterChannelsDto _config;
        private readonly ILogger _log = Interface.XfQueryBot.Logger;

        public RegisterChannelsFunction(RegisterChannels addon, RegisterChannelsDto config)
        {
            _addon = addon;
            _config = config;
        }

        private string CheckClientLang(ClientInfoCommandResponse clientinfo)
        {
            return _config.KickMessage.ContainsKey(clientinfo.ClientCountry.ToLower())
                ? clientinfo.ClientCountry.ToLower()
                : _config.DefaultLang;
        }

        public async Task RegisterOnChannel(ClientMovedEventArgs e)
        {
            try
            {
                var clientInfo = new ClientInfoCommand(e.ClientId).ExecuteAsync(Interface.XfQueryBot.QueryClient)
                    .Result;
                if (_config.RegisterChannels.ContainsKey((int) e.TargetChannelId))
                {
                    var channelId = Convert.ToInt16(e.TargetChannelId);
                    if (clientInfo.TotalConnections < _config.RegisterChannels[channelId].ConnectionsNeeded ||
                        clientInfo.ConnectedTime.TotalSeconds < _config.RegisterChannels[channelId].SecondsNeeded ||
                        clientInfo.ServerGroups.Any(x =>
                            _config.RegisterChannels[channelId].RestrictedRanks.Any(y => y == x)))
                    {
                        await new ClientPokeCommand(e.ClientId, _config.KickMessage[CheckClientLang(clientInfo)])
                            .ExecuteAsync(Interface.XfQueryBot.QueryClient);

                        await new ClientKickCommand(e.ClientId, KickReason.Channel,
                                _config.KickMessage[CheckClientLang(clientInfo)])
                            .ExecuteAsync(Interface.XfQueryBot.QueryClient);
                    }
                    else
                    {
                        await new ServerGroupAddClientCommand(
                                (uint) _config.RegisterChannels[(int) e.TargetChannelId].RankId, clientInfo.DatabaseId)
                            .ExecuteAsync(Interface.XfQueryBot.QueryClient);

                        await new ClientPokeCommand(e.ClientId, _config.SucessMessage[CheckClientLang(clientInfo)])
                            .ExecuteAsync(Interface.XfQueryBot.QueryClient);

                        await new ClientKickCommand(e.ClientId, KickReason.Channel,
                                _config.SucessMessage[CheckClientLang(clientInfo)])
                            .ExecuteAsync(Interface.XfQueryBot.QueryClient);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error($"({_addon.Name}) : {ex}");
            }
        }
    }
}