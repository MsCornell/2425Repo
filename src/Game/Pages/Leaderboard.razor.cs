using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Pages
{
    public partial class Leaderboard : ComponentBase
    {
        public enum DifficultyEnum { Easy, Medium, Hard }
        public enum GameModeEnum { Single, LocalMulti, OnlineMulti }
        public enum TimeFilterEnum { Today, Week, Month, All }
        private bool IsRulesModalVisible { get; set; } = false;

        // Game Mode
        private GameModeEnum GameMode = GameModeEnum.Single;

        // AI Difficulty
        private DifficultyEnum Difficulty = DifficultyEnum.Easy;

        // AI Win Rate
        private string WinRate = "75%";

        // Time Filter
        private TimeFilterEnum SelectedTimeFilter = TimeFilterEnum.Today;

        private bool IsDropdownOpen = false;

        // Search Bar
        private string SearchTerm = string.Empty;

        // Player Data
        private List<Player> Players = new List<Player>();

        // Player Data after filtering
        private List<Player> FilteredPlayers = new List<Player>();

        // Individual Top 3 Players
        private Player? FirstPlacePlayer;
        private Player? SecondPlacePlayer;
        private Player? ThirdPlacePlayer;

        // Last Updated Time
        private DateTime LastUpdated = DateTime.Now;

        // Selected Time Filter Display
        private string SelectedTime => SelectedTimeFilter switch
        {
            TimeFilterEnum.Today => "Today",
            TimeFilterEnum.Week => "This Week",
            TimeFilterEnum.Month => "This Month",
            TimeFilterEnum.All => "All Time",
            _ => "All Time"
        };

        // init
        protected override void OnInitialized()
        {
            GenerateMockData();
            FilterPlayers();
            UpdateTopThree();
        }

        private void GenerateMockData()
        {
            Random random = new Random();
            Players = Enumerable.Range(1, 20).Select(i => new Player
            {
                Name = $"Player {i}",
                Score = random.Next(1500, 3500),
                AvatarUrl = $"/Images/Avatar/Avatar{random.Next(1, 3)}.png" 
            })
            .OrderByDescending(p => p.Score)
            .Select((p, index) => new Player
            {
                Rank = index + 1,
                Name = p.Name,
                Score = p.Score,
                AvatarUrl = p.AvatarUrl
            })
            .ToList();
        }

        private void FilterPlayers()
        {
            int scoreThreshold = GetScoreThreshold();

            FilteredPlayers = Players.Where(p =>
                (string.IsNullOrEmpty(SearchTerm) || (p.Name != null && p.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))) &&
                (SelectedTimeFilter == TimeFilterEnum.All || p.Score >= scoreThreshold)
            )
            .OrderByDescending(p => p.Score)
            .Select((p, index) => new Player
            {
                Rank = index + 1,
                Name = p.Name,
                Score = p.Score,
                AvatarUrl = p.AvatarUrl
            })
            .ToList();
        }

        private int GetScoreThreshold()
        {
            return SelectedTimeFilter switch
            {
                TimeFilterEnum.Today => 2000,
                TimeFilterEnum.Week => 1800,
                TimeFilterEnum.Month => 1600,
                _ => 0
            };
        }

        // Update Top 3 Players
        private void UpdateTopThree()
        {
            var topThree = FilteredPlayers.Take(3).ToList();
            FirstPlacePlayer = topThree.ElementAtOrDefault(0);
            SecondPlacePlayer = topThree.ElementAtOrDefault(1);
            ThirdPlacePlayer = topThree.ElementAtOrDefault(2);
        }

        // Set AI Difficulty
        private void SetDifficulty(DifficultyEnum difficulty)
        {
            Difficulty = difficulty;
            WinRate = difficulty switch
            {
                DifficultyEnum.Easy => "75%",
                DifficultyEnum.Medium => "50%",
                DifficultyEnum.Hard => "25%",
                _ => WinRate
            };
        }

        // Set Game Mode
        private void SetGameMode(GameModeEnum mode)
        {
            GameMode = mode;
        }

        // Set Time Filter
        private void SetTimeFilter(TimeFilterEnum timeFilter)
        {
            SelectedTimeFilter = timeFilter;
            IsDropdownOpen = false;
            FilterPlayers();
            UpdateTopThree();
            LastUpdated = DateTime.Now;
        }

        // Toggle Time Filter Dropdown
        private void ToggleDropdown()
        {
            IsDropdownOpen = !IsDropdownOpen;
        }

        // Refresh Leaderboard
        private void RefreshLeaderboard()
        {
            GenerateMockData();
            FilterPlayers();
            UpdateTopThree();
            LastUpdated = DateTime.Now;
        }

        // Handle Search
        private void HandleSearch()
        {
            FilterPlayers();
            UpdateTopThree();
        }

        private string GetGameModeButtonClass(GameModeEnum mode)
        {
            return mode == GameMode ? "active" : "";
        }

        private string GetDifficultyButtonClass(DifficultyEnum difficulty)
        {
            return difficulty == Difficulty ? "active" : "";
        }

        private string GetTimeFilterButtonClass(TimeFilterEnum timeFilter)
        {
            return timeFilter == SelectedTimeFilter ? "active" : "";
        }

        // Player Class
        public class Player
        {
            public int Rank { get; set; }
            public string? Name { get; set; }
            public int Score { get; set; }
            public string? AvatarUrl { get; set; }
        }

        // Method to show the modal
        private void ShowRulesModal()
        {
            IsRulesModalVisible = true;
        }

        // Method to hide the modal
        private void CloseRulesModal()
        {
            IsRulesModalVisible = false;
        }
    }
}
