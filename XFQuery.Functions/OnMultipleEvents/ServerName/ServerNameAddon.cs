using System;
using System.Threading.Tasks;
using TS3QueryLib.Net.Core.Server.Commands;
using TS3QueryLib.Net.Core.Server.Entitities;
using XFQuery.Core;
using XFQuery.Core.Logging;

namespace XFQuery.Functions.OnMultipleEvents.ServerName
{
    internal class ServerNameFunction
    {
        private readonly ServerNameDto _config;
        private readonly ServerName _extension;
        private readonly ILogger _log = Interface.XfQueryBot.Logger;

        public ServerNameFunction(ServerName e, ServerNameDto config)
        {
            _extension = e;
            _config = config;
        }

        public async Task ServerName()
        {
            try
            {
                var serverInfoCommand = new ServerInfoCommand().ExecuteAsync(Interface.XfQueryBot.QueryClient);
                var serverInfoCommandResponse = serverInfoCommand.Result;
                var onlineUsers = serverInfoCommandResponse.NumberOfClientsOnline;
                var onlineQuery = serverInfoCommandResponse.NumberOfQueryClientsOnline;
                var realClients = onlineUsers - onlineQuery;
                var maxClients = serverInfoCommandResponse.MaximumClientsAllowed;
                var percent = (int) Math.Round((double) (100 * realClients) / maxClients);
                var virtualServerModification = new VirtualServerModification
                {
                    Name = _config.ServerName
                        .Replace("[ONLINEUSERS]", realClients.ToString())
                        .Replace("[MAXUSERS]", maxClients.ToString())
                        .Replace("[ONLINEPERCENT]", percent.ToString())
                        .Replace("[ONLINEQUERY]", onlineQuery.ToString())
                };
                await new ServerEditCommand(virtualServerModification).ExecuteAsync(Interface.XfQueryBot.QueryClient);
            }
            catch (Exception ex)
            {
                _log.Error($"({_extension.Name}) : {ex}");
            }
        }
    }
}