using Microsoft.AspNetCore.Components;
using Game.Services;
using Logic; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Pages
{
    public partial class Leaderboard : ComponentBase
    {
        // Inject Services
        [Inject]
        public PlayerStateService PlayerStateService { get; set; } 

        [Inject]
        private Logic.PlayerWinRateRepository PlayerWinRateRepository { get; set; } = default!;

        [Inject]
        private Logic.GameDetailRepository GameDetailRepository { get; set; } = default!; 

        // Enums
        public enum DifficultyEnum { Easy, Medium, Hard }
        public enum GameModeEnum { Single, LocalMulti, OnlineMulti }
        public enum TimeFilterEnum { Today, Week, Month, All }

        // Setup
        private GameModeEnum GameMode = GameModeEnum.Single; 
        private DifficultyEnum? Difficulty = DifficultyEnum.Easy; 
        private TimeFilterEnum SelectedTimeFilter = TimeFilterEnum.All; 
        private bool IsDropdownOpen = false;
        private string SearchTerm = string.Empty; 
        private DateTime LastUpdated = DateTime.Now; 
        private bool IsRulesModalVisible { get; set; } = false; 

        // Leaderboard Data
        private string WinRate = "75%"; // AI win rate

        // Player Data
        private List<LeaderboardPlayer> Players = new List<LeaderboardPlayer>(); 
        private List<LeaderboardPlayer> FilteredPlayers = new List<LeaderboardPlayer>(); // After filtering

        // Top 3 Players
        private LeaderboardPlayer? FirstPlacePlayer;
        private LeaderboardPlayer? SecondPlacePlayer;
        private LeaderboardPlayer? ThirdPlacePlayer;

        // Current Player
        private LeaderboardPlayer? CurrentPlayer;

        // Time Filter Options
        private string SelectedTime => SelectedTimeFilter switch
        {
            TimeFilterEnum.Today => "Today",
            TimeFilterEnum.Week => "This Week",
            TimeFilterEnum.Month => "This Month",
            _ => "All Time"
        };

        // Initialize
        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            FilterPlayers();
            UpdateTopThree();
        }

        private async Task LoadDataAsync()
        {
            var playerWinRates = await PlayerWinRateRepository.GetAllPlayerAsync();
            var gameDetails = await GameDetailRepository.GetAllGameDetailsAsync();

            if (playerWinRates != null && gameDetails != null)
            {
                Random random = new Random();

                var gameGroups = gameDetails.GroupBy(g => new { g.PlayerName, g.GameMode });

                var playerScores = gameGroups.Select(g => new
                {
                    PlayerName = g.Key.PlayerName,
                    GameMode = g.Key.GameMode,
                    TotalScoreInMode = g.Sum(game => game.GameScore),
                    TotalGamesInMode = g.Count(),
                    WinsInMode = g.Count(game => game.GameWinner == g.Key.PlayerName), 
                    LastGameDate = g.Max(game => game.Ended)
                }).ToList();

                var totalScores = gameDetails.GroupBy(g => g.PlayerName).Select(g => new
                {
                    PlayerName = g.Key,
                    TotalScore = g.Sum(game => game.GameScore),
                    TotalGames = g.Count(),
                    TotalWins = g.Count(game => game.GameWinner == g.Key),
                    LastGameDate = g.Max(game => game.Ended)
                }).ToList();

                Players = playerWinRates.Select(pwr =>
                {
                    var (gameModeEnum, difficultyEnum) = MapGameMode(pwr.GameMode);

                    var playerScore = playerScores.FirstOrDefault(ps => ps.PlayerName == pwr.PlayerName && ps.GameMode.Equals(pwr.GameMode, StringComparison.OrdinalIgnoreCase));
                    var totalScore = totalScores.FirstOrDefault(ts => ts.PlayerName == pwr.PlayerName);

                    return new LeaderboardPlayer
                    {
                        Name = pwr.PlayerName,
                        Score = playerScore != null ? playerScore.TotalScoreInMode : 0,
                        AvatarUrl = $"/Images/Avatar/Avatar{random.Next(1, 3)}.png", 
                        GameMode = gameModeEnum,
                        Difficulty = difficultyEnum,
                        TotalGamesInMode = playerScore != null ? playerScore.TotalGamesInMode : 0,
                        WinsInMode = playerScore != null ? playerScore.WinsInMode : 0,
                        WinRateInMode = playerScore != null && playerScore.TotalGamesInMode > 0 ? (double)playerScore.WinsInMode / playerScore.TotalGamesInMode * 100 : 0,
                        TotalGames = totalScore != null ? totalScore.TotalGames : 0,
                        TotalWins = totalScore != null ? totalScore.TotalWins : 0,
                        OverallWinRate = totalScore != null && totalScore.TotalGames > 0 ? (double)totalScore.TotalWins / totalScore.TotalGames * 100 : 0,
                        TotalScoreInMode = playerScore != null ? playerScore.TotalScoreInMode : 0,
                        TotalScore = totalScore != null ? totalScore.TotalScore : 0,
                        LastGameDate = totalScore != null ? totalScore.LastGameDate : DateTime.MinValue
                    };
                })
                .OrderByDescending(p => p.Score)
                .Select((p, index) =>
                {
                    p.Rank = index + 1;
                    return p;
                })
                .ToList();
            }
            else
            {
                Players = new List<LeaderboardPlayer>();
            }
        }

        private (GameModeEnum, DifficultyEnum?) MapGameMode(string gameMode)
        {
            switch (gameMode.ToLower())
            {
                case "easy":
                    return (GameModeEnum.Single, DifficultyEnum.Easy);
                case "medium":
                    return (GameModeEnum.Single, DifficultyEnum.Medium);
                case "hard":
                    return (GameModeEnum.Single, DifficultyEnum.Hard);
                case "local":
                    return (GameModeEnum.LocalMulti, null);
                default:
                    return (GameModeEnum.OnlineMulti, null);
            }
        }

        private void FilterPlayers()
        {
            var filteredPlayers = Players.AsEnumerable();

            // Filter by game mode
            filteredPlayers = filteredPlayers.Where(p => p.GameMode == GameMode);

            // Filter by difficulty
            if (GameMode == GameModeEnum.Single && Difficulty != null)
            {
                filteredPlayers = filteredPlayers.Where(p => p.Difficulty == Difficulty);
            }

            // Time filter
            DateTime fromDate = SelectedTimeFilter switch
            {
                TimeFilterEnum.Today => DateTime.Today,
                TimeFilterEnum.Week => DateTime.Today.AddDays(-7),
                TimeFilterEnum.Month => DateTime.Today.AddMonths(-1),
                _ => DateTime.MinValue
            };

            filteredPlayers = filteredPlayers.Where(p => p.LastGameDate >= fromDate);

            // Filter by search term
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                filteredPlayers = filteredPlayers.Where(p => p.Name != null && p.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            // Re-rank players
            FilteredPlayers = filteredPlayers
                .OrderByDescending(p => p.Score)
                .Select((p, index) =>
                {
                    p.Rank = index + 1;
                    return p;
                })
                .ToList();

            // Set CurrentPlayer
            if (PlayerStateService.CurrentPlayer != null)
            {
                var currentPlayerName = PlayerStateService.CurrentPlayer.Name;
                CurrentPlayer = FilteredPlayers.FirstOrDefault(p => p.Name == currentPlayerName);
            }
            else
            {
                CurrentPlayer = null;
            }
        }

        private void UpdateTopThree()
        {
            var topThree = FilteredPlayers.Take(3).ToList();
            FirstPlacePlayer = topThree.ElementAtOrDefault(0);
            SecondPlacePlayer = topThree.ElementAtOrDefault(1);
            ThirdPlacePlayer = topThree.ElementAtOrDefault(2);
        }

        // Set AI difficulty
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
            FilterPlayers();
            UpdateTopThree();
        }

        // Set game mode
        private void SetGameMode(GameModeEnum mode)
    {
    GameMode = mode;
    if (GameMode != GameModeEnum.Single)
    {
        Difficulty = null;
    }
    else
    {
        if (Difficulty == null)
        {
            Difficulty = DifficultyEnum.Easy; 
            WinRate = "75%"; 
        }
    }
    FilterPlayers();
    UpdateTopThree();
    }

        // Set time filter
        private void SetTimeFilter(TimeFilterEnum timeFilter)
        {
            SelectedTimeFilter = timeFilter;
            IsDropdownOpen = false;
            FilterPlayers();
            UpdateTopThree();
            LastUpdated = DateTime.Now;
        }

        // Toggle dropdown
        private void ToggleDropdown()
        {
            IsDropdownOpen = !IsDropdownOpen;
        }

        // Handle search
        private void HandleSearch()
        {
            FilterPlayers();
            UpdateTopThree();
        }

        // LeaderboardPlayer Model
        public class LeaderboardPlayer
        {
            public int? Rank { get; set; }
            public string? Name { get; set; }
            public int? Score { get; set; }
            public string? AvatarUrl { get; set; }
            public GameModeEnum GameMode { get; set; }
            public DifficultyEnum? Difficulty { get; set; }
            public int TotalGamesInMode { get; set; }
            public int WinsInMode { get; set; }
            public double WinRateInMode { get; set; }
            public int TotalGames { get; set; }
            public int TotalWins { get; set; }
            public double OverallWinRate { get; set; }
            public int TotalScoreInMode { get; set; }
            public int TotalScore { get; set; }
            public DateTime LastGameDate { get; set; } 
        }

        // Refresh Leaderboard
        private async Task RefreshLeaderboard()
        {
            await LoadDataAsync();
            FilterPlayers();
            UpdateTopThree();
            LastUpdated = DateTime.Now;
        }

        // CSS Classes
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

        private void ShowRulesModal() 
        {
            IsRulesModalVisible = true;
        }

        private void CloseRulesModal() 
        {
            IsRulesModalVisible = false;
        }
    }
}
