using Microsoft.AspNetCore.Components;
using Logic;
using Game.Services;
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

        [Inject]
        private PlayerWinRateRepository PlayerWinRateRepo { get; set; }
        [Inject]
        private GameDetailRepository GameDetailRepo { get; set; }
        [Inject]
        private PlayerStateService PlayerStateService { get; set; }

        // Initial values
        private GameModeEnum GameMode = GameModeEnum.Single;
        private DifficultyEnum Difficulty = DifficultyEnum.Easy;
        private TimeFilterEnum TimeFilter = TimeFilterEnum.All;

        private List<PlayerWinRate> AllPlayers = new();
        private List<PlayerWinRate> FilteredPlayers = new();
        private PlayerWinRate? FirstPlacePlayer;
        private PlayerWinRate? SecondPlacePlayer;
        private PlayerWinRate? ThirdPlacePlayer;
        private string SearchTerm = string.Empty;
        private bool IsDropdownOpen = false;
        private DateTime LastUpdated;

        // AI statistics
        private double WinRate;

        // Current player statistics
        private int MyRank;
        private int MyScore;

        // Modal visibility
        private bool IsRulesModalVisible = false;

        protected override async Task OnInitializedAsync()
        {
            await LoadLeaderboardData();
        }

        private async Task LoadLeaderboardData()
        {
            LastUpdated = DateTime.Now;

            // Time filter
            DateTime? startDate = TimeFilter switch
            {
                TimeFilterEnum.Today => DateTime.Today,
                TimeFilterEnum.Week => DateTime.Today.AddDays(-7),
                TimeFilterEnum.Month => DateTime.Today.AddMonths(-1),
                _ => null
            };

            // Selected game mode
            string selectedGameMode = GameMode switch
            {
                GameModeEnum.Single => Difficulty.ToString(),
                GameModeEnum.LocalMulti => "Local",
                GameModeEnum.OnlineMulti => "Online",
                _ => string.Empty
            };

            // Get player win rates
            var playerWinRates = await PlayerWinRateRepo.GetPlayerWinRatesAsync(selectedGameMode, startDate);

            // Order players by total score in selected game mode
            AllPlayers = playerWinRates
                .OrderByDescending(p => p.TotalScoreInMode)
                .ToList();

            for (int i = 0; i < AllPlayers.Count; i++)
            {
                AllPlayers[i].Rank = i + 1;
            }

            // Set top 3 players
            FirstPlacePlayer = AllPlayers.ElementAtOrDefault(0);
            SecondPlacePlayer = AllPlayers.ElementAtOrDefault(1);
            ThirdPlacePlayer = AllPlayers.ElementAtOrDefault(2);

            // Calculate AI win rate
            if (GameMode == GameModeEnum.Single)
            {
                await CalculateHumanWinRate(selectedGameMode, startDate);
            }
            else
            {
                WinRate = 0;
            }

            UpdateFilteredPlayers();
        }

        private async Task CalculateHumanWinRate(string difficulty, DateTime? startDate)
        {
            var filteredGames = await GameDetailRepo.GetGameDetailsAsync(difficulty, startDate);
            int totalGames = filteredGames.Count();
            int humanWins = filteredGames.Count(g => g.GameWinner == g.PlayerCharacter);

            WinRate = totalGames > 0 ? (double)humanWins / totalGames * 100 : 0;
        }

        private void UpdateFilteredPlayers()
        {
            FilteredPlayers = AllPlayers.Skip(3).ToList();

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                FilteredPlayers = FilteredPlayers
                    .Where(p => p.PlayerName.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            int? currentPlayerId = GetCurrentPlayerId();
            if (currentPlayerId.HasValue)
            {
                var myPlayer = AllPlayers.FirstOrDefault(p => p.PlayerId == currentPlayerId.Value);
                if (myPlayer != null)
                {
                    MyRank = AllPlayers.IndexOf(myPlayer) + 1;
                    MyScore = (int)myPlayer.TotalScoreInMode;
                }
                else
                {
                    MyRank = -1;
                    MyScore = 0;
                }
            }
        }

        private void SetGameMode(GameModeEnum mode)
        {
            GameMode = mode;
            if (GameMode != GameModeEnum.Single)
            {
                Difficulty = DifficultyEnum.Easy;
            }
            _ = LoadLeaderboardData();
        }

        private void SetDifficulty(DifficultyEnum difficulty)
        {
            Difficulty = difficulty;
            _ = LoadLeaderboardData();
        }

        private void SetTimeFilter(TimeFilterEnum timeFilter)
        {
            TimeFilter = timeFilter;
            IsDropdownOpen = false;
            _ = LoadLeaderboardData();
        }

        private void ToggleDropdown()
        {
            IsDropdownOpen = !IsDropdownOpen;
        }

        private void HandleSearch()
        {
            UpdateFilteredPlayers();
        }

        private void RefreshLeaderboard()
        {
            _ = LoadLeaderboardData();
        }

        private void ShowRulesModal()
        {
            IsRulesModalVisible = true;
        }

        private void CloseRulesModal()
        {
            IsRulesModalVisible = false;
        }

        private int? GetCurrentPlayerId()
        {
            var currentPlayer = PlayerStateService.CurrentPlayer;
            return currentPlayer?.Id;
        }

        private string GetRandomAvatarUrl(int? playerId)
        {
            if (playerId.HasValue)
            {
                int avatarIndex = (playerId.Value % 2) + 1;
                return $"/Images/Avatar/Avatar{avatarIndex}.png";
            }
            return "/Images/Avatar/AvatarDefault.png";
        }

        private string GetGameModeButtonClass(GameModeEnum mode)
        {
            return GameMode == mode ? "active" : "";
        }

        private string GetDifficultyButtonClass(DifficultyEnum difficulty)
        {
            return Difficulty == difficulty ? "active" : "";
        }

        private string GetTimeFilterButtonClass(TimeFilterEnum timeFilter)
        {
            return TimeFilter == timeFilter ? "selected" : "";
        }

        private string SelectedTime => TimeFilter switch
        {
            TimeFilterEnum.Today => "Today",
            TimeFilterEnum.Week => "This Week",
            TimeFilterEnum.Month => "This Month",
            _ => "All Time"
        };
    }
}
