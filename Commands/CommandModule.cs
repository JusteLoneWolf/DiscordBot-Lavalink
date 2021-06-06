using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;

namespace DiscordBot.Commands
{
    public class CMD : BaseCommandModule
    {
        [Command("join")]
            public async Task Join(CommandContext ctx)
            {
                var lava = ctx.Client.GetLavalink();
                
                if (!lava.ConnectedNodes.Any())
                {
                    await ctx.RespondAsync("La connection n'est pas etablie");
                    return;
                }
                var vs = ctx.Member.VoiceState;
                var chn = vs.Channel;
                var node = lava.ConnectedNodes.Values.First();

                if (chn.Type != ChannelType.Voice)
                {
                    await ctx.RespondAsync("Ce n'est pas un channel vocal valide.");
                    return;
                }

                await node.ConnectAsync(chn);
                await ctx.Channel.SendMessageAsync($"Rejoin {chn.Name}!");

            }

            [Command("leave")]
            public async Task Leave(CommandContext ctx)
            {
                var lava = ctx.Client.GetLavalink();
                if (!lava.ConnectedNodes.Any())
                {
                    await ctx.RespondAsync("La connection n'est pas etablie");
                    return;
                }
                var vs = ctx.Member.VoiceState;
                var chn = vs.Channel;
                var node = lava.ConnectedNodes.Values.First();

                if (chn.Type != ChannelType.Voice)
                {
                    await ctx.RespondAsync("Ce n'est pas un channel vocal valide.");
                    return;
                }

                var conn = node.GetGuildConnection(chn.Guild);

                if (conn == null)
                {
                    await ctx.RespondAsync("Lavalink n'est pas connecter.");
                    return;
                }

                await conn.DisconnectAsync();
                await ctx.Channel.SendMessageAsync($"Quitte {chn.Name}!");

            }
            
            [Command ("play")]
            public async Task Play(CommandContext ctx, [RemainingText] string search)
            {
                var lava = ctx.Client.GetLavalink();
                
                if (!lava.ConnectedNodes.Any())
                {
                    await ctx.RespondAsync("La connection n'est pas etablie");
                    return;
                }
                var vs = ctx.Member.VoiceState;
                var chn = vs.Channel;
                var node = lava.ConnectedNodes.Values.First();
                if (chn.Type != ChannelType.Voice)
                {
                    await ctx.RespondAsync("Ce n'est pas un channel vocal valide.");
                    return;
                }

                await node.ConnectAsync(chn);
                await ctx.RespondAsync($"Rejoin {chn.Name}!");
                await node.ConnectAsync(chn);
                var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);
                if (conn == null)
                {
                    await ctx.RespondAsync("Lavalink n'est pas connecter.");
                    return;
                }
                var loadResult = await node.Rest.GetTracksAsync(search);
                if (loadResult.LoadResultType == LavalinkLoadResultType.LoadFailed 
                    || loadResult.LoadResultType == LavalinkLoadResultType.NoMatches)
                {
                    await ctx.RespondAsync($"Recherche echoué pour: {search}.");
                    return;
                }

                var track = loadResult.Tracks.First();

                await conn.PlayAsync(track);

                await ctx.Channel.SendMessageAsync($"Joue actuellement {track.Title}!");
            }
            
            [Command ("pause")]

            public async Task Pause(CommandContext ctx)
            {
                if (ctx.Member.VoiceState == null || ctx.Member.VoiceState.Channel == null)
                {
                    await ctx.RespondAsync("Vous n'etes pas dans un channel vocal.");
                    return;
                }

                var lava = ctx.Client.GetLavalink();
                var node = lava.ConnectedNodes.Values.First();
                var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);

                if (conn == null)
                {
                    await ctx.RespondAsync("Lavalink n'est pas connecter.");
                    return;
                }
                if (conn.CurrentState.CurrentTrack == null)
                {
                    await ctx.RespondAsync("Pas de musique.");
                }
                await conn.PauseAsync();
                await ctx.Channel.SendMessageAsync("La musique est en pause");

            }
            
            [Command ("resume")]

            public async Task Resume(CommandContext ctx)
            {
                if (ctx.Member.VoiceState == null || ctx.Member.VoiceState.Channel == null)
                {
                    await ctx.RespondAsync("Vous n'etes pas dans un channel vocal.");
                    return;
                }

                var lava = ctx.Client.GetLavalink();
                var node = lava.ConnectedNodes.Values.First();
                var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);

                if (conn == null)
                {
                    await ctx.RespondAsync("Lavalink n'est pas connecter.");
                    return;
                }
                if (conn.CurrentState.CurrentTrack == null)
                {
                    await ctx.RespondAsync("Pas de musique.");
                }
                await conn.ResumeAsync();
                await ctx.Channel.SendMessageAsync("La musique a repris");

            }
            
            [Command ("seek")]

            public async Task Seek(CommandContext ctx,TimeSpan seek)
            {
                if (ctx.Member.VoiceState == null || ctx.Member.VoiceState.Channel == null)
                {
                    await ctx.RespondAsync("Vous n'etes pas dans un channel vocal.");
                    return;
                }

                var lava = ctx.Client.GetLavalink();
                var node = lava.ConnectedNodes.Values.First();
                var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);

                if (conn == null)
                {
                    await ctx.RespondAsync("Lavalink n'est pas connecter.");
                    return;
                }
                if (conn.CurrentState.CurrentTrack == null)
                {
                    await ctx.RespondAsync("Pas de musique.");
                }

                await conn.SeekAsync(seek);
                await ctx.Channel.SendMessageAsync($"La musique c'est avancé a {seek}");


            }
            [Command ("volume")]

            public async Task Volume(CommandContext ctx,int volume)
            {
                if (ctx.Member.VoiceState == null || ctx.Member.VoiceState.Channel == null)
                {
                    await ctx.RespondAsync("Vous n'etes pas dans un channel vocal.");
                    return;
                }

                var lava = ctx.Client.GetLavalink();
                var node = lava.ConnectedNodes.Values.First();
                var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);

                if (conn == null)
                {
                    await ctx.RespondAsync("Lavalink n'est pas connecter.");
                    return;
                }
                if (conn.CurrentState.CurrentTrack == null)
                {
                    await ctx.RespondAsync("Pas de musique.");
                }

                await conn.SetVolumeAsync(volume);
                await ctx.Channel.SendMessageAsync($"Le volume est maintenant a {volume}");

            }
            
            [Command ("stop")]

            public async Task Stop(CommandContext ctx)
            {
                if (ctx.Member.VoiceState == null || ctx.Member.VoiceState.Channel == null)
                {
                    await ctx.RespondAsync("Vous n'etes pas dans un channel vocal.");
                    return;
                }

                var lava = ctx.Client.GetLavalink();
                var node = lava.ConnectedNodes.Values.First();
                var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);

                if (conn == null)
                {
                    await ctx.RespondAsync("Lavalink n'est pas connecter.");
                    return;
                }
                if (conn.CurrentState.CurrentTrack == null)
                {
                    await ctx.RespondAsync("Pas de musique.");
                }

                await conn.StopAsync();
                await ctx.Channel.SendMessageAsync("La musique est arreter");
            }
            
        
    }
}