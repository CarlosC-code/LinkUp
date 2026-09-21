
using LinkUp.Core.Application.Interfaces;
using LinkUp.Core.Application.ViewModels.Social;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinkUp.Controllers
{
    [Authorize]
    public class FriendRequestsController : Controller
    {
        private readonly IFriendRequestService _requests;
        private readonly IFriendshipService _friendships;
        private readonly IAccountServiceForWebApp _users;

        public FriendRequestsController(
            IFriendRequestService requests,
            IFriendshipService friendships,
            IAccountServiceForWebApp users)
        {
            _requests = requests;
            _friendships = friendships;
            _users = users;
        }

        private string CurrentUserId => User.Identity!.Name!;

        
        // LISTADOS: recibidas + enviadas
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var me = CurrentUserId;

            var pending = await _requests.GetPendingReceivedAsync(me);
            var sent = await _requests.GetSentByUserAsync(me);

            
            foreach (var r in pending)
            {
                var sender = await _users.GetUserByUserName(r.SenderUserId);
                r.SenderUserName = sender?.UserName ?? r.SenderUserId;   
                r.SenderProfileImage = sender?.ProfileImage;                // <-- foto si la tienes
                r.MutualFriends = await _friendships.GetMutualFriendsCountAsync(me, r.SenderUserId);
            }

            foreach (var r in sent)
            {
                var receiver = await _users.GetUserByUserName(r.ReceiverUserId);
                r.ReceiverUserName = receiver?.UserName ?? r.ReceiverUserId; 
                r.ReceiverProfileImage = receiver?.ProfileImage;               // <-- foto si la tienes
                r.MutualFriends = await _friendships.GetMutualFriendsCountAsync(me, r.ReceiverUserId);
            }

            var vm = new FriendRequestsIndexViewModel
            {
                PendingReceived = pending.Select(r => new PendingRequestViewModel
                {
                    RequestId = r.Id,
                    SenderUserId = r.SenderUserId,
                    SenderUserName = r.SenderUserName ?? r.SenderUserId,
                    SenderProfileImage = r.SenderProfileImage,
                    MutualFriends = r.MutualFriends,
                    SentAtUtc = r.SentAtUtc
                }).ToList(),

                Sent = sent.Select(r => new SentRequestViewModel
                {
                    RequestId = r.Id,
                    ReceiverUserId = r.ReceiverUserId,
                    ReceiverUserName = r.ReceiverUserName ?? r.ReceiverUserId,
                    ReceiverProfileImage = r.ReceiverProfileImage,
                    MutualFriends = r.MutualFriends,
                    SentAtUtc = r.SentAtUtc,
                    StatusText = r.Status switch
                    {
                        1 => "En espera",
                        2 => "Aceptada",
                        3 => "Rechazada",
                        _ => "?"
                    }
                }).ToList()
            };

            return View(vm);
        }

        
        // CONFIRMAR ACEPTAR
      
        [HttpGet]
        public IActionResult ConfirmAccept(int id, string senderName)
        {
            ViewBag.RequestId = id;
            ViewBag.SenderName = senderName;
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptConfirmed(int id)
        {
            await _requests.AcceptAsync(id, CurrentUserId);
            return RedirectToAction(nameof(Index));
        }

       
        // CONFIRMAR RECHAZAR
      
        [HttpGet]
        public IActionResult ConfirmReject(int id, string senderName)
        {
            ViewBag.RequestId = id;
            ViewBag.SenderName = senderName;
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectConfirmed(int id)
        {
            await _requests.RejectAsync(id, CurrentUserId);
            return RedirectToAction(nameof(Index));
        }

       
        // CONFIRMAR ELIMINAR (solicitudes enviadas)
        
        [HttpGet]
        public IActionResult ConfirmDelete(int id, string receiverName)
        {
            ViewBag.RequestId = id;
            ViewBag.ReceiverName = receiverName;
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _requests.DeleteAsync(id, CurrentUserId);
            return RedirectToAction(nameof(Index));
        }

        
        // CREAR SOLICITUD
        
        [HttpGet]
        public async Task<IActionResult> Create(string? search)
        {
            var me = CurrentUserId;

            // Usuarios activos desde tu servicio de cuentas
            var allUsers = await _users.GetAllUser();

            // Excluir: yo mismo
            var pool = allUsers
                .Where(u => !string.Equals(u.UserName, me, StringComparison.OrdinalIgnoreCase))
                .ToList();

            // Excluir: amigos actuales y solicitudes pendientes (ambas direcciones)
            var friendIds = await _friendships.GetFriendIdsAsync(me);
            var pendingReceived = await _requests.GetPendingReceivedAsync(me);
            var pendingSent = await _requests.GetSentByUserAsync(me);

            var excluded = new HashSet<string>(friendIds, StringComparer.OrdinalIgnoreCase);
            foreach (var r in pendingReceived) excluded.Add(r.SenderUserId);
            foreach (var r in pendingSent) excluded.Add(r.ReceiverUserId);
            excluded.Add(me);

            var filtered = pool.Where(u => !excluded.Contains(u.UserName)).ToList();

            // Construir candidatos con amigos en comun
            var candidates = new List<SelectUserViewModel>();
            foreach (var u in filtered)
            {
                var mutual = await _friendships.GetMutualFriendsCountAsync(me, u.UserName);
                candidates.Add(new SelectUserViewModel
                {
                    UserId = u.UserName,
                    UserName = u.UserName,          
                    ProfileImage = u.ProfileImage,  // <- foto si existe
                    MutualFriends = mutual
                });
            }

            if (!string.IsNullOrWhiteSpace(search))
                candidates = candidates
                    .Where(c => c.UserName.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            return View(new FriendRequestCreateViewModel
            {
                Search = search,
                Candidates = candidates
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FriendRequestCreateViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            if (string.IsNullOrWhiteSpace(vm.TargetUserId))
            {
                ModelState.AddModelError("", "Debe seleccionar un usuario.");
                return View(vm);
            }

            var me = CurrentUserId;

            if (!await _requests.CanRequestAsync(me, vm.TargetUserId))
            {
                ModelState.AddModelError("", "No puede enviar una solicitud a este usuario (ya son amigos o hay una solicitud pendiente).");
                return View(vm);
            }

            await _requests.CreateAsync(me, vm.TargetUserId);
            return RedirectToAction(nameof(Index));
        }
    }
}
