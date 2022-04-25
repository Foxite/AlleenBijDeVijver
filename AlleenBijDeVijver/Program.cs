// See https://aka.ms/new-console-template for more information

using DSharpPlus;
using DSharpPlus.Entities;
using Microsoft.Extensions.Logging;

var discord = new DiscordClient(new DiscordConfiguration() {
	Token = Environment.GetEnvironmentVariable("BOT_TOKEN")
});

ulong guildId = ulong.Parse(Environment.GetEnvironmentVariable("GUILD_ID")!);
ulong channelId = ulong.Parse(Environment.GetEnvironmentVariable("CHANNEL_ID")!);
string empty = Environment.GetEnvironmentVariable("NAME_0")!;
string one = Environment.GetEnvironmentVariable("NAME_1")!;
string multiple = Environment.GetEnvironmentVariable("NAME_M")!;

var tcs = new TaskCompletionSource();

discord.Ready += (sender, eventArgs) => {
	tcs.SetResult();
	return Task.CompletedTask;
};

await discord.ConnectAsync();
await tcs.Task;

await Task.Delay(TimeSpan.FromSeconds(5));
while (true) {
	DiscordChannel channel = (await discord.GetGuildAsync(guildId)).GetChannel(channelId);

	if (channel.Name != empty && channel.Name != one && channel.Name != multiple) {
		break;
	}
	
	string? newName = channel.Users.Count(member => !member.IsBot) switch {
		0 when channel.Name == one || channel.Name == multiple => empty,
		1 when channel.Name == empty || channel.Name == multiple => one,
		> 1 when channel.Name == empty || channel.Name == one => multiple,
		_ => null
	};
	if (newName != null) {
		await channel.ModifyAsync(cem => cem.Name = newName);
		await Task.Delay(TimeSpan.FromSeconds(295));
	}

	await Task.Delay(TimeSpan.FromSeconds(5));
}
