using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TS3QueryLib.Net.Core.Common.Entities;
using TS3QueryLib.Net.Core.Server.Commands;
using TS3QueryLib.Net.Core.Server.Notification.EventArgs;
using TS3QueryLib.Net.Core.Server.Responses;
using XFQuery.Core;
using XFQuery.Core.Logging;

namespace XFQuery.Functions.OnMultipleEvents.NicknameCheck
{
    internal class NicknameCheckFunction
    {
        private readonly NicknameCheckDto _config;
        private readonly NicknameCheck _extension;
        private readonly ILogger _log = Interface.XfQueryBot.Logger;

        public NicknameCheckFunction(NicknameCheck e, NicknameCheckDto config)
        {
            _extension = e;
            _config = config;
        }

        private string CheckClientLang(ClientInfoCommandResponse clientinfo)
        {
            return _config.Reason.ContainsKey(clientinfo.ClientCountry.ToLower())
                ? clientinfo.ClientCountry.ToLower()
                : _config.DefaultLang;
        }

        public async Task CheckOnJoin(ClientJoinedEventArgs client)
        {
            try
            {
                var restrictedwords = _config.BannedWords;
                var regexMatch = string.Join<string>("|", restrictedwords);
                var regex = new Regex($"({regexMatch})", RegexOptions.IgnoreCase);
                var clientinfo = new ClientInfoCommand(client.ClientId).ExecuteAsync(Interface.XfQueryBot.QueryClient)
                    .Result;
                if (!regex.IsMatch(client.Nickname) || !client.ClientType.Equals(0)) return;
                if (_config.PokeOrKick.Equals("Kick", StringComparison.OrdinalIgnoreCase))
                    await new ClientKickCommand(client.ClientId, KickReason.Server,
                            _config.Reason[CheckClientLang(clientinfo)].Replace("[NICKNAME]", client.Nickname))
                        .ExecuteAsync(Interface.XfQueryBot.QueryClient);
                else
                    await new ClientPokeCommand(client.ClientId,
                            _config.Reason[CheckClientLang(clientinfo)].Replace("[NICKNAME]", client.Nickname))
                        .ExecuteAsync(Interface.XfQueryBot.QueryClient);
            }
            catch (Exception ex)
            {
                _log.Error($"({_extension.Name}) : {ex}");
            }
        }

        public void CheckOnInterval()
        {
            try
            {
                var restrictedwords = _config.BannedWords;
                var regexMatch = string.Join<string>("|", restrictedwords);
                var regex = new Regex($"({regexMatch})", RegexOptions.IgnoreCase);
                var clientListCommand = new ClientListCommand().ExecuteAsync(Interface.XfQueryBot.QueryClient);
                var clientList = clientListCommand.Result.Values;
                clientList.ForEach(client =>
                {
                    var clientinfo = new ClientInfoCommand(client.ClientId)
                        .ExecuteAsync(Interface.XfQueryBot.QueryClient)
                        .Result;
                    if (!regex.IsMatch(client.Nickname) || !client.ClientType.Equals(0)) return;
                    if (_config.PokeOrKick.Equals("Kick", StringComparison.OrdinalIgnoreCase))
                        new ClientKickCommand(client.ClientId, KickReason.Server,
                                _config.Reason[CheckClientLang(clientinfo)].Replace("[NICKNAME]", client.Nickname))
                            .ExecuteAsync(Interface.XfQueryBot.QueryClient);
                    else
                        new ClientPokeCommand(client.ClientId,
                                _config.Reason[CheckClientLang(clientinfo)].Replace("[NICKNAME]", client.Nickname))
                            .ExecuteAsync(Interface.XfQueryBot.QueryClient);
                });
            }
            catch (Exception ex)
            {
                _log.Error($"({_extension.Name}) : {ex}");
            }
        }
    }
}