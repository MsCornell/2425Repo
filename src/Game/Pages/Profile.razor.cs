using Microsoft.AspNetCore.Components;
using Game.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;

namespace Game.Pages
{
    public partial class Profile : ComponentBase
    {
        // Inject Services
        [Inject]
        public PlayerStateService PlayerStateService { get; set; } 

        // Current Player Info
        private string PlayerName;
        private string PlayerRank = string.Empty;
        private string AvatarUrl = "/Images/Avatar/Avatar1.png";

        // Stats
        private List<Stat> Stats = new List<Stat>();

        // Unlocked Badges
        private List<Badge> UnlockedBadges = new List<Badge>();

        // Achievements
        private List<Achievement> Achievements = new List<Achievement>();

        // Match History
        private List<Match> MatchHistory = new List<Match>();

        // Tabs
        private List<TabItem> Tabs = new List<TabItem>
        {
            new TabItem { Id = "history", Title = "Match History" },
            new TabItem { Id = "achievements", Title = "Achievements" }
        };

        private string ActiveTab = "history";

        protected override void OnInitialized()
        {
            PlayerName = PlayerStateService.CurrentPlayer? .Name ?? "Player";
            InitializeData();
        }

        private void SwitchTab(string tabId)
        {
            ActiveTab = tabId;
        }

        private void InitializeData()
        {
            // Initialize Stats
            Stats.Add(new Stat { Title = "Win Rate", Value = "65%" });
            Stats.Add(new Stat { Title = "Games", Value = "42" });
            Stats.Add(new Stat { Title = "Points", Value = "2,456" });

            // Initialize Unlocked Badges
            UnlockedBadges.Add(new Badge { Name = "Speed Master", Icon = "⚡" });

            // Initialize Achievements
            Achievements.Add(new Achievement
            {
                Title = "Quick Thinker",
                Description = "Win 3 games under 3 minutes",
                Icon = "⚡",
                Progress = 2,
                Total = 3,
                Unlocked = false,
                Badge = "Speed Master"
            });
            Achievements.Add(new Achievement
            {
                Title = "Strategic Mind",
                Description = "Win 5 games against Hard AI",
                Icon = "🎯",
                Progress = 3,
                Total = 5,
                Unlocked = false,
                Badge = "Strategic Master"
            });
            Achievements.Add(new Achievement
            {
                Title = "Social Player",
                Description = "Play 8 Local Multiplayer games",
                Icon = "🤝",
                Progress = 5,
                Total = 8,
                Unlocked = false,
                Badge = "Party Champion"
            });

            // Initialize Match History
            MatchHistory.Add(new Match
            {
                Result = "Victory",
                Mode = "Single Player",
                Difficulty = "Hard",
                Score = "+30",
                Time = "3:45",
                Date = new DateTime(2024, 2, 20)
            });
            MatchHistory.Add(new Match
            {
                Result = "Draw",
                Mode = "Local Multiplayer",
                Score = "+0",
                Time = "4:20",
                Date = new DateTime(2024, 2, 19)
            });
            MatchHistory.Add(new Match
            {
                Result = "Victory",
                Mode = "Single Player",
                Difficulty = "Medium",
                Score = "+20",
                Time = "2:55",
                Date = new DateTime(2024, 2, 18)
            });
            MatchHistory.Add(new Match
            {
                Result = "Defeat",
                Mode = "Single Player",
                Difficulty = "Hard",
                Score = "+30",
                Time = "5:10",
                Date = new DateTime(2024, 2, 17)
            });
            MatchHistory.Add(new Match
            {
                Result = "Victory",
                Mode = "Local Multiplayer",
                Score = "+40",
                Time = "3:30",
                Date = new DateTime(2024, 2, 16)
            });

            // Calculate Player Rank teir
            int totalPoints = 2456;
            int totalGames = 42;
            PlayerRank = RankSystem.GetRank(totalPoints, totalGames, Achievements);
        }

        // Models
        public class Stat
        {
            public string Title { get; set; } = string.Empty;
            public string Value { get; set; } = string.Empty;
        }

        public class Badge
        {
            public string Name { get; set; } = string.Empty;
            public string Icon { get; set; } = string.Empty;
        }

        public class Achievement
        {
            public string Title { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string Icon { get; set; } = string.Empty;
            public int Progress { get; set; }
            public int Total { get; set; }
            public bool Unlocked { get; set; }
            public string Badge { get; set; } = string.Empty;

            public double ProgressPercentage => (double)Progress / Total * 100;
        }

        public class Match
        {
            public string Result { get; set; } = string.Empty;
            public string Mode { get; set; } = string.Empty;
            public string Difficulty { get; set; } = string.Empty;
            public string Score { get; set; } = string.Empty;
            public string Time { get; set; } = string.Empty;
            public DateTime Date { get; set; }

            public string ModeDisplay => Mode == "Single Player" ? $"{Mode} ({Difficulty})" : Mode;
        }

        public class TabItem
        {
            public string Id { get; set; } = string.Empty;
            public string Title { get; set; } = string.Empty;
        }

        public static class RankSystem
        {
            public static string GetRank(int points, int totalGames, List<Achievement> achievementsProgress)
            {
                double totalProgress = achievementsProgress
                    .Select(a => (double)a.Progress / a.Total)
                    .Average() * 100;
                double pointsPerGame = points / (double)totalGames;

                if (points >= 2000) return "Grandmaster";
                if (points >= 1500) return "Master";
                if (points >= 1000) return "Expert";
                if (points >= 500) return "Veteran";
                if (points >= 200) return "Skilled";
                if (points >= 100) return "Apprentice";
                return "Novice";
            }
        }

     // Tooltip State
    private bool IsTooltipVisible { get; set; } = false;
    private string TooltipContent { get; set; } = string.Empty;
    private (string Top, string Left) TooltipPosition { get; set; } = ("0px", "0px");

    // Tooltip Mappings
    private readonly Dictionary<string, string> StatTooltips = new()
    {
        { "Win Rate", "Percentage of games won" },
        { "Games", "Total number of games played" },
        { "Points", "Total score points earned" }
    };

    private readonly Dictionary<string, string> RankTooltips = new()
    {
        { "Grandmaster", "2000+ points, exceptional achievement progress" },
        { "Master", "1500-1999 points, strong performance" },
        { "Expert", "1000-1499 points, solid progress" },
        { "Veteran", "500-999 points, consistent play" },
        { "Skilled", "200-499 points, improving" },
        { "Apprentice", "100-199 points, beginner level" },
        { "Novice", "0-99 points, just starting out" }
    };

    // Show Tooltip for PlayerRank
    private void ShowRankTooltip(MouseEventArgs e)
    {
        if (RankTooltips.TryGetValue(PlayerRank, out var tooltipText))
        {
            TooltipContent = tooltipText;
            TooltipPosition = ($"{e.ClientY - 10}px", $"{e.ClientX + 10}px");
            IsTooltipVisible = true;
        }
    }

    // Show Tooltip for Stats
    private void ShowStatTooltip(Stat stat, MouseEventArgs e)
    {
        if (StatTooltips.TryGetValue(stat.Title, out var tooltipText))
        {
            TooltipContent = tooltipText;
            TooltipPosition = ($"{e.ClientY - 10}px", $"{e.ClientX + 10}px");
            IsTooltipVisible = true;
        }
    }

    private void HideTooltip()
    {
        IsTooltipVisible = false;
        TooltipContent = string.Empty;
    }

    }
}

