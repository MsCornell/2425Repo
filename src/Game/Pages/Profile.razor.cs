using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Pages
{
    public partial class Profile : ComponentBase
    {
        // Player Info
        private string PlayerName = "Player Name";
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
                Score = "5-4",
                Time = "3:45",
                Date = new DateTime(2024, 2, 20)
            });
            MatchHistory.Add(new Match
            {
                Result = "Draw",
                Mode = "Local Multiplayer",
                Score = "4-4",
                Time = "4:20",
                Date = new DateTime(2024, 2, 19)
            });
            MatchHistory.Add(new Match
            {
                Result = "Victory",
                Mode = "Single Player",
                Difficulty = "Medium",
                Score = "6-3",
                Time = "2:55",
                Date = new DateTime(2024, 2, 18)
            });
            MatchHistory.Add(new Match
            {
                Result = "Defeat",
                Mode = "Single Player",
                Difficulty = "Hard",
                Score = "3-6",
                Time = "5:10",
                Date = new DateTime(2024, 2, 17)
            });
            MatchHistory.Add(new Match
            {
                Result = "Victory",
                Mode = "Local Multiplayer",
                Score = "5-2",
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

                if (points >= 2000 && pointsPerGame >= 50 && totalProgress >= 80) return "Grandmaster";
                if (points >= 1500 && pointsPerGame >= 40 && totalProgress >= 60) return "Master";
                if (points >= 1000 && pointsPerGame >= 30 && totalProgress >= 40) return "Expert";
                if (points >= 500 && pointsPerGame >= 20 && totalProgress >= 20) return "Veteran";
                if (points >= 200 && pointsPerGame >= 10) return "Skilled";
                if (points >= 100) return "Apprentice";
                return "Novice";
            }
        }
    }
}

