
using LinkUp.Core.Application.Interfaces;
using LinkUp.Core.Application.ViewModels.Battleship;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinkUp.Controllers
{
    [Authorize]
    public class BattleshipController : Controller
    {
        private readonly IBattleshipService _bs;
        private readonly IFriendshipService _friendships;
        private readonly IAccountServiceForWebApp _accounts; 

        public BattleshipController(
            IBattleshipService bs,
            IFriendshipService friendships,
            IAccountServiceForWebApp accounts) 
        {
            _bs = bs;
            _friendships = friendships;
            _accounts = accounts;             
        }

        
        private async Task<string> GetMeAsync()
        {
            
            var raw = User.Identity!.Name!;
            var dto = await _accounts.GetUserByUserName(raw);
            return dto?.UserName ?? raw;
        }

        // LISTADOS: activas + historial
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var me = await GetMeAsync();

            // Cerrar por timeout si aplica (48h sin mover)
            var activeList = await _bs.GetActiveGamesAsync(me);
            foreach (var g in activeList)
                await _bs.FinishIfTimeoutAsync(g.Id);

            var active = await _bs.GetActiveGamesAsync(me);
            var history = await _bs.GetFinishedGamesAsync(me);

            var vm = new BattleshipIndexViewModel
            {
                ActiveGames = active.Select(a => new ActiveGameItemViewModel
                {
                    GameId = a.Id,
                    OpponentUserId = a.CreatorUserId == me ? a.OpponentUserId : a.CreatorUserId,
                    OpponentUserName = a.OpponentUserName ?? (a.CreatorUserId == me ? a.OpponentUserId : a.CreatorUserId),
                    OpponentProfileImage = a.OpponentProfileImage,
                    StartedAtUtc = a.StartedAtUtc,
                    IsMyTurn = a.IsMyTurn
                }).ToList(),
                History = history.Select(h => new HistoryGameItemViewModel
                {
                    GameId = h.Id,
                    OpponentUserName = h.OpponentUserName ?? (h.CreatorUserId == me ? h.OpponentUserId : h.CreatorUserId),
                    StartedAtUtc = h.StartedAtUtc,
                    FinishedAtUtc = h.FinishedAtUtc ?? DateTime.UtcNow,
                    IsWinner = h.WinnerUserId == me,
                    WinnerText = h.WinnerUserId == me ? "Yo" :
                                 (h.CreatorUserId == me ? h.OpponentUserId : h.CreatorUserId)
                }).ToList()
            };

            return View(vm);
        }

        // NUEVA PARTIDA
        [HttpGet]
        public async Task<IActionResult> NewGame(string? search)
        {
            var me = await GetMeAsync();
            var friendIds = await _friendships.GetFriendIdsAsync(me);

            var friends = new List<FriendCandidateViewModel>();
            foreach (var id in friendIds)
            {
                var u = await _accounts.GetUserByUserName(id);
                friends.Add(new FriendCandidateViewModel
                {
                    UserId = id,
                    UserName = u?.UserName ?? id,
                    ProfileImage = u?.ProfileImage
                });
            }

            if (!string.IsNullOrWhiteSpace(search))
                friends = friends.Where(f => f.UserName.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

            var vm = new NewGameViewModel
            {
                Search = search,
                Friends = friends
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewGame(NewGameViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            var me = await GetMeAsync();

            var gameId = await _bs.StartNewGameAsync(me, vm.SelectedFriendUserId!);
            if (gameId == null)
            {
                ModelState.AddModelError("", "No se pudo iniciar la partida (ya existe una activa o la selección no es válida).");
                return View(vm);
            }
            return RedirectToAction(nameof(SelectShip), new { gameId = gameId.Value });
        }

        // FASE 1: seleccionar barco restante
        [HttpGet]
        public async Task<IActionResult> SelectShip(int gameId)
        {
            var me = await GetMeAsync();
            var remaining = await _bs.GetRemainingShipsAsync(gameId, me);
            if (!remaining.Any())
            {
                // Si ambos listos -> pasa a fase de ataque
                if (await _bs.AreBothReadyAsync(gameId))
                    return RedirectToAction(nameof(AttackBoard), new { gameId });

                return View("WaitingOpponent", gameId);
            }
            var vm = new ShipSelectionViewModel { GameId = gameId, RemainingShips = remaining };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectShip(int gameId, int selectedShip)
            => RedirectToAction(nameof(PlaceOnBoard), new { gameId, shipLength = selectedShip });

        [HttpGet]
        public async Task<IActionResult> PlaceOnBoard(int gameId, int shipLength)
        {
            var me = await GetMeAsync();
            var occupied = await _bs.GetMyOccupiedCellsAsync(gameId, me);
            var board = new List<BoardCellViewModel>();
            var occ = occupied.ToHashSet();
            for (int r = 0; r < 12; r++)
                for (int c = 0; c < 12; c++)
                    board.Add(new BoardCellViewModel
                    {
                        Row = r,
                        Col = c,
                        IsOccupied = occ.Contains((r, c)),
                        IsSelectable = !occ.Contains((r, c))
                    });

            var vm = new PlaceOnBoardViewModel
            {
                GameId = gameId,
                SelectedShipLength = shipLength,
                Board = board
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChooseDirection(int gameId, int shipLength, int row, int col)
            => RedirectToAction(nameof(ChooseDirection), new { gameId, shipLength, row, col });

        [HttpGet]
        public IActionResult ChooseDirection(int gameId, int shipLength, int row, int col, string? error = null)
        {
            var vm = new ChooseDirectionViewModel
            {
                GameId = gameId,
                ShipLength = shipLength,
                StartRow = row,
                StartCol = col,
                ErrorMessage = error
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Place(int gameId, int shipLength, int startRow, int startCol, int direction)
        {
            var me = await GetMeAsync();
            var ok = await _bs.PlaceShipAsync(gameId, me, shipLength, startRow, startCol, direction);
            if (!ok)
                return RedirectToAction(nameof(ChooseDirection),
                    new { gameId, shipLength, row = startRow, col = startCol, error = "Posición/Dirección inválida o superpuesta." });

            return RedirectToAction(nameof(SelectShip), new { gameId });
        }

        // FASE 2: ataque por turnos
        [HttpGet]
        public async Task<IActionResult> AttackBoard(int gameId)
        {
            var me = await GetMeAsync();

            await _bs.FinishIfTimeoutAsync(gameId);

            // Si aún no está en juego, redirige a colocar barcos / o refresca si ambos listos.
            var ready = await _bs.AreBothReadyAsync(gameId);
            var isMyTurn = await _bs.IsMyTurnAsync(gameId, me);

            if (!ready)
                return RedirectToAction(nameof(SelectShip), new { gameId });

            // Si ambos listos pero el servicio aún no marcó InProgress por milisegundos de diferencia,
            // vuelve a entrar (fuerza recálculo de turno).
            if (!isMyTurn)
            {
                
            }

            var marks = await _bs.GetMyAttackMarksAsync(gameId, me);

            var board = new List<BoardCellViewModel>();
            var dic = marks.ToDictionary(k => (k.Row, k.Col), v => v.IsHit);
            for (int r = 0; r < 12; r++)
                for (int c = 0; c < 12; c++)
                {
                    dic.TryGetValue((r, c), out var hit);
                    board.Add(new BoardCellViewModel
                    {
                        Row = r,
                        Col = c,
                        IsHit = dic.ContainsKey((r, c)) ? hit : null
                    });
                }

            var vm = new AttackBoardViewModel
            {
                GameId = gameId,
                IsMyTurn = isMyTurn,
                TurnMessage = isMyTurn ? "" : "Es turno del oponente. Usa 'Refrescar pantalla'.",
                Board = board
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Attack(int gameId, int row, int col)
        {
            var me = await GetMeAsync();
            var (success, _, error) = await _bs.AttackAsync(gameId, me, row, col);
            if (!success && !string.IsNullOrEmpty(error))
                TempData["Error"] = error;

            return RedirectToAction(nameof(AttackBoard), new { gameId });
        }

        // Rendicion
        [HttpGet]
        public IActionResult ConfirmSurrender(int gameId) => View(model: gameId);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Surrender(int gameId)
        {
            var me = await GetMeAsync();
            await _bs.SurrenderAsync(gameId, me);
            return RedirectToAction(nameof(Index));
        }

        // Resultado final (historial)
        [HttpGet]
        public async Task<IActionResult> Result(int gameId)
        {
            var me = await GetMeAsync();

            var g = await _bs.GetByIdAsync(gameId, me);
            if (g == null || g.Status != (int)LinkUp.Core.Domain.Common.Enums.GameStatus.Finished) return NotFound();

            var myAttack = await _bs.GetMyAttackMarksAsync(gameId, me);
            var oppAttack = await _bs.GetMyAttackMarksAsync(gameId, g.OpponentUserId);
            var myPlacement = await _bs.GetMyOccupiedCellsAsync(gameId, me);

            List<BoardCellViewModel> ToBoard(IEnumerable<(int Row, int Col, bool IsHit)> marks)
            {
                var list = new List<BoardCellViewModel>();
                var dic = marks.ToDictionary(k => (k.Row, k.Col), v => v.IsHit);
                for (int r = 0; r < 12; r++)
                    for (int c = 0; c < 12; c++)
                    {
                        dic.TryGetValue((r, c), out var hit);
                        list.Add(new BoardCellViewModel
                        {
                            Row = r,
                            Col = c,
                            IsHit = dic.ContainsKey((r, c)) ? hit : null
                        });
                    }
                return list;
            }

            List<BoardCellViewModel> ToPlacement(IEnumerable<(int Row, int Col)> occ)
            {
                var set = occ.ToHashSet();
                var list = new List<BoardCellViewModel>();
                for (int r = 0; r < 12; r++)
                    for (int c = 0; c < 12; c++)
                        list.Add(new BoardCellViewModel
                        {
                            Row = r,
                            Col = c,
                            IsOccupied = set.Contains((r, c))
                        });
                return list;
            }

            var opp = await _accounts.GetUserByUserName(g.OpponentUserId);
            var niceOpponentName = opp?.UserName ?? g.OpponentUserId;

            var vm = new GameResultViewModel
            {
                GameId = gameId,
                OpponentUserName = niceOpponentName,
                StartedAtUtc = g.StartedAtUtc,
                FinishedAtUtc = g.FinishedAtUtc ?? DateTime.UtcNow,
                IsWinner = g.WinnerUserId == me,
                MyAttackBoard = ToBoard(myAttack),
                OpponentAttackBoard = ToBoard(oppAttack),
                MyPlacementBoard = ToPlacement(myPlacement)
            };

            return View(vm);
        }
    }
}

